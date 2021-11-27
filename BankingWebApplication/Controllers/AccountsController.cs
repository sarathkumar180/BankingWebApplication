using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Entities;
using BusinessLayer;
using Microsoft.AspNetCore.Http;

namespace BankingWebApplication.Controllers
{
    public class AccountsController : Controller
    {
        static IAccount ca;
        static IAccount ba;
        static IAccount la;
        static IAccount td;
        private readonly ApplicationDbContext _context;

        AccountBL accountbl = new AccountBL();
        CustomerBL customerBl = new CustomerBL();

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts/{Customer No}
        public async Task<IActionResult> Index(int? id)
        {
            string header = "All Accounts";
            if (id != null)
            {
                var customer = customerBl.GetCustomerFromCustomerNo(id.Value, _context);
                if (customer != null)
                {
                    header = $"Accounts : {customer.CustomerNo} / {customer.FirstName} {customer.LastName}";
                    TempData["CustomerNo"] = id;
                }
            }
            else
            {
                TempData["CustomerNo"] = null;
            }

            TempData["Header"] = header;
            if (HttpContext.Session.GetString("CustomerNo") != null)
            {
                var accounts = accountbl.GetAllAccount(_context);
                if (id != null && accounts != null)
                {
                    accounts = accounts.Where(x => x.CustomerNo == id);

                }
                return View(accounts);
            }
            else
            {
                return RedirectToAction("Login", "Customers");
            }

        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.customer)
                .FirstOrDefaultAsync(m => m.AccountNo == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create/{customerNo}
        public IActionResult OpenAccount()
        {
            var id = TempData["CustomerNo"];
            TempData.Keep("CustomerNo");
            var custList = GetAllCustomers()?.AsEnumerable();
            string customerName = null;
            Account acc = new Account();
            acc.Interestrate = 0.02m;
            acc.AccountStatus = true;
            acc.Balance = 500.00m;
            if (id != null && custList != null)
            {
                var customer = custList.FirstOrDefault(w => w.Value == id.ToString());
                if (customer != null)
                {
                    customerName = customer.Text;
                    acc.CustomerNo =int.Parse(customer.Value);
                }
            }

            ViewBag.CustomerName = customerName;
            ViewBag.CustList = custList;
            
            return View(acc);
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenAccount([Bind("Balance,AccountNo,AccountType,CustomerNo,AccountStatus,Interestrate")] Account account)
        {
            var customerNo = TempData["CustomerNo"];
            if (ModelState.IsValid)
            {
                account.AccountNo = accountbl.GenerateAccountno();

                if (account.AccountType == "Savings")
                {
                    var customer = customerBl.GetCustomerFromCustomerNo(account.CustomerNo, _context);
                    if (customer == null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ca = new SavingsAccount()
                    {
                        AccountNo = accountbl.GenerateAccountno(),
                        CustomerId = customer.Id,
                        CustomerNo = account.CustomerNo,
                        Balance = account.Balance,
                        AccountStatus = accountbl.GetAccountStatus(),
                        AccountType = account.AccountType,

                    };

                    ca.Interestrate = ca.GetIntrestrate();
                    accountbl.OpenAccount(ca, _context);

                }

                return RedirectToAction(nameof(Index), new {id = customerNo});

            }
            //ViewData["CustomerNo"] = new SelectList(_context.Customer, "CustomerNo", "Email", account.CustomerNo);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["CustomerNo"] = new SelectList(_context.Customer, "CustomerNo", "Email", account.CustomerNo);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Balance,AccountNo,AccountType,CustomerNo,AccountStatus,MaturityDateTime,Interestrate")] Account account)
        {
            if (id != account.AccountNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerNo"] = new SelectList(_context.Customer, "CustomerNo", "Email", account.CustomerNo);
            return View(account);
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.AccountNo == id);
        }

        public IActionResult Deposit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var acc = accountbl.GetAccount(id.Value, _context);
            ViewBag.BalanceText = $"Account balance : {acc?.Balance.ToString("C")}";
            return View(acc);
        }

        [HttpPost]

        public IActionResult Deposit(int? id, decimal amount)
        {

            if (id == null)
            {
                return NotFound();
            }

            var acc = accountbl.GetAccount(id.Value, _context);

            accountbl.Deposit(acc, amount, _context); // calling businesslayer


            return RedirectToAction("Index", "Accounts"); // redirect link to index

        }


        public IActionResult Withdraw(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var acc = accountbl.GetAccount(id.Value, _context);
                ViewBag.BalanceText = $"Account balance : {acc?.Balance.ToString("C")}";
                return View(acc);

            }

        }

        [HttpPost]
        public ActionResult Withdraw(int? id, decimal amount)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {

                var acc = accountbl.GetAccount(id.Value, _context);// get account info 

                accountbl.Withdraw(acc, amount, _context); // calling businesslayer

                return RedirectToAction("Index", "Accounts"); // redirect link to index

            }

        }

        public IActionResult Transfer(int? id, decimal amount)
        {

            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Account acc = _context.Account.Find(id); // get account info 



                var AccList =
                    _context.Account
                    .Where(a => a.CustomerNo == acc.CustomerNo)
                    .Where(a => a.AccountNo != acc.AccountNo).ToList();

                TransferAccount ta = new TransferAccount()
                {
                    account = acc,
                    accounts = AccList

                };



                return View(ta);
            }//end
        }

        [HttpPost]
        public IActionResult Transfer(int? id, int ToAccountno, decimal amount)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {

                Account fromAccount = _context.Account.Find(id); // find transfer account from
                Account toAccount = _context.Account.Find(ToAccountno); /// get to acc

                accountbl.Transfer(fromAccount, toAccount, amount, _context); // calling businesslayer


                return RedirectToAction("Index", "Accounts"); // redirect link to index
            }//end else
        }


        public IActionResult TransactionList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var account = accountbl.GetAccount(id.Value,_context);

                var allTxns = accountbl.GetTransaction(account.AccountNo, _context);
                var list = allTxns.Where(x => x.Accountno == account.AccountNo && x.CustomerId == account.CustomerNo).Take(10).OrderByDescending(t => t.Time);

                return View(list);
            }
        }


        public IActionResult TransactionRange(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var acc =  accountbl.GetAccount(id.Value,_context);

            return View(acc);


        }

        [HttpPost]
        public IActionResult TransactionView(int id, DateTime startTime, DateTime endTime)
        {

            endTime = endTime.AddDays(1);
            var account = accountbl.GetAccount(id, _context);

            var txnList = accountbl.GetTransaction(account.AccountNo, _context);
            var list = txnList.Where(x => x.Accountno == account.AccountNo && x.CustomerId == account.CustomerId && x.Time > startTime && x.Time <= endTime);


            return View(list);



        }

        private List<SelectListItem> GetAllCustomers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var allCustomers = customerBl.GetAllCustomer(_context);

            if (allCustomers.Any())
            {
                foreach (var cust in allCustomers)
                {
                    list.Add(new SelectListItem { Text = cust.FirstName + " " + cust.LastName, Value = cust.CustomerNo.ToString() });
                }
            }

            return list;
        }




    }
}
