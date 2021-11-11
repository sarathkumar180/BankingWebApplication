using System.Collections.Generic;

namespace DAL.Entities
{
    public interface ICustomerDAL
    {
        Customer Login(User user, ApplicationDbContext _context);
        void Register(Customer customer, ApplicationDbContext _context);
        List<Customer> GetAllCustomer();
        int GenerateCustomerId();




    }
}


