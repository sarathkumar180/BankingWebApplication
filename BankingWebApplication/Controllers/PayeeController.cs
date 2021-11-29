using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingWebApplication.Models;
using BusinessLayer;
using DAL;
using DAL.Entities;
using DAL.Enum;
using Microsoft.AspNetCore.Http;

namespace BankingWebApplication.Controllers
{
    public class PayeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        CustomerBL customerbl = new CustomerBL();

        public PayeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") == RoleEnum.Customer.ToString() &&
                !string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerNo")))
            {
                var payees = customerbl.GetPayeesForCustomerNo(int.Parse(HttpContext.Session.GetString("CustomerNo")),_context);
                payees?.ForEach(s => s.CustomerNo = int.Parse(HttpContext.Session.GetString("CustomerNo")));
                return View(payees);
            }

            return View("Error", new ErrorViewModel { RequestId = "Unauthorized Request.Authorization Error" });
        }

        public IActionResult Payee(int customerNo, int payeeId, bool newPayee)
        {
            var loggedInUser = HttpContext.Session.GetString("CustomerNo");
            var customer = customerbl.GetCustomerFromCustomerNo(customerNo, _context);
            if (customer != null && loggedInUser == customer.CustomerNo.ToString())
            {
                Payee model = null;
                if (payeeId > 0)
                {
                    model = customerbl.GetPayeesForCustomerNo(customerNo, _context)?.FirstOrDefault();
                    if (model != null) model.CustomerNo = customer.CustomerNo;
                    ;
                }
                else
                {
                    model = new Payee();
                    model.CustomerNo = customer.CustomerNo;
                    model.CustomerId = customer.Id;
                    model.IsActive = true;

                }


                ViewBag.IsNewPayee = newPayee;
                return View(model);
            }

            if (customer == null)
            {
                return View("Error", new ErrorViewModel { RequestId = "Invalid Request" });
            }

            return View("Error", new ErrorViewModel { RequestId = "Authorization Error" });
        }

        [HttpPost]
        public IActionResult Payee(Payee model)
        {
            if (ModelState.IsValid)
            {
                model.IsActive = true;
                model.CreatedDateTime = DateTime.Now;
                model.IsDeleted = false;
                bool success = customerbl.AddOrUpdatePayee(model, _context);
                if (success)
                {
                    return RedirectToAction("Index", "Payee");
                }
                else
                {
                    return View("Error", new ErrorViewModel { RequestId = "Database Error" });
                }
            }

            return View(model);
        }

    }
}
