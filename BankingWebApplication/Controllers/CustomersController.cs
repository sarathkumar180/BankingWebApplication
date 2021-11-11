using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using BusinessLayer;
using DAL;

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
            return View(await _context.Customer.ToListAsync());
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("CustomerNo,UserName,Password,FirstName,LastName,Email,Address")] Customer customer)
        {
            if (id != customer.CustomerNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Home");
            }
            return View(customer);
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
                //user found in the database
                ViewBag.LoginSucceed = "Succeed!! Welcome.";
                HttpContext.Session.SetString("CustomerNo", userinfo.CustomerNo.ToString());//set CustomerNo value use in Welcome Action
                HttpContext.Session.SetString("UserName", userinfo.UserName.ToString()); //set UserName value use in Welcome Action
                HttpContext.Session.SetString("UserRole", userinfo.CustomerRole.ToString()); //set UserName value use in Welcome Action


                return RedirectToAction("index", "Home");

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
                
                ViewBag.CustomerId = HttpContext.Session.GetString("CustomerNo");//determine differnt nav view for login and logout
                //also use CustomerNo to determine if the user is login or not

                return RedirectToAction($"Edit/{ViewBag.CustomerId}","Customers");
               
                
            }
            else
            {

                return RedirectToAction("Customers", "Login");
      
            }
        }

        

        

       
    }
}
