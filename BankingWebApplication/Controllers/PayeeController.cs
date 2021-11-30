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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingWebApplication.Controllers
{
    public class PayeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        CustomerBL customerbl = new CustomerBL();
        AccountBL _accountBl = new AccountBL();

        public PayeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") == RoleEnum.Customer.ToString() &&
                !string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerNo")))
            {
                int customerNo = int.Parse(HttpContext.Session.GetString("CustomerNo"));
                var accounts = GetAllAccounts(customerNo);
                if (accounts != null && accounts.Any())
                {
                    var payees = customerbl.GetPayeesForCustomerNo(customerNo, _context);
                    payees?.ForEach(s =>
                    {
                        s.CustomerNo = int.Parse(HttpContext.Session.GetString("CustomerNo"));
                        s.FromAccountNo = int.Parse(accounts[0].Value);
                    });
                    ViewBag.Accounts = accounts;
                    return View(payees);
                }
                else
                {
                    ViewBag.Accounts = null;
                    return View();
                }
            }

            return View("Error", new ErrorViewModel { RequestId = "Unauthorized Request.Authorization Error" });
        }

        [HttpPost]
        public IActionResult Index(IEnumerable<Payee> model)
        {
            var enumerable = model as Payee[] ?? model.ToArray();
            if (enumerable.Any(x => x.IsChecked && x.AmountToPay > 0))
            {
                foreach (var payee in enumerable.Where(x=>x.IsChecked && x.AmountToPay > 0))
                {
                    _accountBl.PayeeTransfer(payee.FromAccountNo,payee.PayeeAccountNumber,payee.AmountToPay,_context);
                }

                return RedirectToAction("Index", "Accounts");
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

        private List<SelectListItem> GetAllAccounts(int customerNo)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var allAccounts = customerbl.GetAllAccountsForCustomer(customerNo, _context);

            if (allAccounts != null && allAccounts.Any())
            {
                foreach (var acc in allAccounts)
                {
                    list.Add(new SelectListItem
                    {
                        Text = acc.AccountNo + " / " + acc.AccountType + " / Balance : " + $"{acc.Balance:C}",
                        Value = acc.AccountNo.ToString()
                    });
                }
            }

            return list;
        }
    }
}
