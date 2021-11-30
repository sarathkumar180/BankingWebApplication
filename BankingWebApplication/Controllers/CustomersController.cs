using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using BusinessLayer;
using DAL;
using DAL.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingWebApplication.Controllers
{
    public class CustomersController : Controller
    {
        private  readonly ApplicationDbContext _context;

        CustomerBL customerbl = new CustomerBL();

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: Customers
        public async Task<IActionResult> Index()
        {
            IEnumerable<Customer> customers = customerbl.GetAllCustomer(_context);
            if (HttpContext.Session.GetString("UserRole") == RoleEnum.Teller.ToString())
            {
                customers = customers.Where(x => x.CustomerRole != RoleEnum.Teller.ToString() && !string.IsNullOrEmpty(x.CustomerRole));
            }
            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerNo == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CustomerNo,UserName,Password,FirstName,LastName,Email,Address")] Customer customer)
        {
            if (_context.Customer.Any(x => x.UserName == customer.UserName))
            {
                ViewBag.UserExistedMessage = "User name already exist.";
                return View("Create", customer);
            }
            else if (ModelState.IsValid )
            {
                customerbl.Register(customer, _context);

                return RedirectToAction("Login","Customers");
            }

            
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? customerno)
        {
            if (customerno == null)
            {
                return NotFound();
            }

            var customer = customerbl.GetCustomerFromCustomerNo(customerno.Value,_context);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CustomerNo, [Bind("Id,CustomerNo,UserName,Password,FirstName,LastName,PhoneNumber,Email,Address")] Customer customer)
        {
            if (CustomerNo != customer.CustomerNo)
            {
                return NotFound();
            }

            
            if (ModelState.IsValid)
            {
                bool success = customerbl.UpdateCustomer(customer, _context);
                if (success)
                {
                    var userRole = HttpContext.Session.GetString("UserRole");
                    if (string.IsNullOrEmpty(userRole) || userRole == RoleEnum.Customer.ToString())
                    {
                        return RedirectToAction("Index", "Accounts");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Customers");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError(nameof(customer.CustomerNo), "Failed to update!");
                    return View(customer);
                }
            }
            return View(customer);
        }


        public IActionResult EditRole(int customerNo, string userName, string roleName)
        {
            List<SelectList> roles = new List<SelectList>();
           roles.Add(new SelectList(RoleEnum.Teller.ToString(), RoleEnum.Teller.ToString()));
           roles.Add(new SelectList(RoleEnum.Customer.ToString(), RoleEnum.Customer.ToString()));
           EditRoleViewModel model = new EditRoleViewModel();
           model.RolesList = roles;
           model.SelectedRole = string.IsNullOrEmpty(roleName) ? RoleEnum.Customer.ToString() : roleName;
           model.CustomerNo = customerNo;
           model.UserName = userName;
           return View(model);
        }

        [HttpPost]
        public IActionResult EditRole(EditRoleViewModel model)
        {
            if (model != null)
            {
                Enum.TryParse(model.SelectedRole, out RoleEnum role);
                bool success = customerbl.AddOrUpdateUserRole(model.CustomerNo, (int) role, _context);
                if(success)
                    return RedirectToAction("Index", "Customers");
                return View("Error", new ErrorViewModel { RequestId = "Update User Role" });
            }
            else
            {
                return View("Error", new ErrorViewModel { RequestId = "Update User Role" });
            }

            
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerNo == id);
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
     
        public IActionResult Login(User user)
        {

            var userinfo = customerbl.Login(user, _context);// check if username and password 


            if (userinfo != null)
            {
                if(string.IsNullOrEmpty(userinfo.CustomerRole))
                    return View("Error", new ErrorViewModel { RequestId = "Authorization Error - User Role not assigned" });
                //user found in the database
                ViewBag.LoginSucceed = "Succeed!! Welcome.";
                HttpContext.Session.SetString("CustomerNo", userinfo.CustomerNo.ToString());//set CustomerNo value use in Welcome Action
                HttpContext.Session.SetString("UserName", userinfo.UserName); //set UserName value use in Welcome Action
                HttpContext.Session.SetString("UserRole", userinfo.CustomerRole); //set UserName value use in Welcome Action


                return RedirectToAction("index", "Accounts");

            }
            else
            {
                ViewBag.LoginFailed = $"Failed, incorrect UserName or Password";
                //display loginfailed message to user
                
            }

            return View(user);
        }

        
        public IActionResult Logout()
        {
           
            HttpContext.Session.Clear();//remove current session
            return RedirectToAction("Login", "Customers");
        }

        
       
        public IActionResult Welcome()
        {
            if(HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName"); //use to display username for upper right nav 
                
                ViewBag.CustomerId = int.Parse((HttpContext.Session.GetString("CustomerNo")));//determine differnt nav view for login and logout
                                                                                              //also use CustomerNo to determine if the user is login or not
                var customer = customerbl.GetCustomerFromCustomerNo(ViewBag.CustomerId, _context);
                if (customer == null)
                {
                    return NotFound();
                }

                return View($"Details", customer);

            }
            else
            {
                return RedirectToAction("Login", "Customers");
            }
        }

        #region Payee Information

        
        #endregion






    }
}
