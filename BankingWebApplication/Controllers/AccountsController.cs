using System;
using System.Linq;
using System.Threading.Tasks;
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

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            
            if (HttpContext.Session.GetString("CustomerNo") != null)
            {
                var customers = accountbl.GetAllAccount(_context);
                return View(customers);
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

        // GET: Accounts/Create
        public IActionResult OpenAccount()
        {
            ViewData["CustomerNo"] = new SelectList(_context.Customer, "CustomerNo", "UserName");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenAccount([Bind("Balance,AccountNo,AccountType,CustomerNo,AccountStatus,Interestrate")] Account account)
        {

            if (ModelState.IsValid)
            {
                account.AccountNo = accountbl.GenerateAccountno();

                if (account.AccountType == "Savings")
                {

                    ca = new SavingsAccount()
                    {
                        AccountNo = accountbl.GenerateAccountno(),
                      
                        CustomerNo = account.CustomerNo,
                        Balance = account.Balance,
                        AccountStatus = accountbl.GetAccountStatus(),
                        AccountType = account.AccountType,
                       
                    };
                 
                    ca.Interestrate = ca.GetIntrestrate();
                    accountbl.OpenAccount(ca, _context);

                }

                return RedirectToAction(nameof(Index));
            
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
            IAccount acc = _context.Account.Find(id);


       
            return View(acc);
        }

        [HttpPost]
        
        public IActionResult Deposit(int? id, decimal amount)
        {

            if (id == null)
            {
                return NotFound();
            }

            IAccount acc = _context.Account.FirstOrDefault(a => a.AccountNo == id) ;// get account info 

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
                IAccount acc = _context.Account.Find(id);
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

                IAccount acc = _context.Account.Find(id); // get account info 

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
        
        
        public  IActionResult TransactionList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var account = _context.Account.Where(x => x.AccountNo == id).FirstOrDefault();

                var list = _context.Transaction.Where(x => x.Accountno == account.AccountNo && x.CustomerId == account.CustomerNo).Take(10).OrderByDescending(t => t.Time);

                return View(list);
            }
        }

       
        public IActionResult TransactionRange(int? id)
        {

            var acc = _context.Account.Find(id);
       
                return View(acc);
            

        }

        [HttpPost]
        public IActionResult TransactionView(int id, DateTime startTime, DateTime endTime)
        {
            

            var account = _context.Account.Where(x => x.AccountNo == id).FirstOrDefault();

            var list = _context.Transaction
                .Where(x => x.Accountno == account.AccountNo && x.CustomerId == account.CustomerNo &&  x.Time > startTime && x.Time <= endTime);
           

                return View(list);
            
        

        }



    }
}
