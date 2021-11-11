using DAL.Entities;
using DAL;
using System.Collections.Generic;


namespace BusinessLayer
{
    public class CustomerBL
    {

        

        ICustomerDAL customerdal = new CustomerDAL();
     

        public CustomerBL()
        {

        }

        public  bool Register(Customer customer , ApplicationDbContext _context)
        {
             return customerdal.Register(customer,_context);
       
        }

        public bool UpdateCustomer(Customer customer, ApplicationDbContext _context)
        {
            return customerdal.UpdateCustomer(customer, _context);

        }

        public List<Customer> GetAllCustomer()
        {
            return customerdal.GetAllCustomer();
        }

        public int GenerateCustomerNo(ApplicationDbContext context)
        {
            return customerdal.GenerateCustomerNo(context);
        }

        public Customer Login(User user, ApplicationDbContext context)
        {
            return customerdal.Login(user, context);
        }

        public Customer GetCustomerFromCustomerNo(int customerNo, ApplicationDbContext context)
        {
            return customerdal.GetCustomerFromCustomerNo(customerNo, context);
        }

    }
}
