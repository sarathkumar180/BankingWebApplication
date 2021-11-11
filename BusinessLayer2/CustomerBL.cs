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

        public  void Register(Customer customer , ApplicationDbContext _context)
        {
             customerdal.Register(customer,_context);
       
        }

        public List<Customer> GetAllCustomer()
        {
            return customerdal.GetAllCustomer();
        }

        public int GenerateCustomerId()
        {
            return customerdal.GenerateCustomerId();
        }

        public Customer Login(User user, ApplicationDbContext context)
        {
            return customerdal.Login(user, context);
        }

      

    }
}
