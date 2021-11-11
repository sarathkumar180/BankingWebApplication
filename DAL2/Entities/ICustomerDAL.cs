using System.Collections.Generic;

namespace DAL.Entities
{
    public interface ICustomerDAL
    {
        Customer Login(User user, ApplicationDbContext _context);
        bool Register(Customer customer, ApplicationDbContext _context);
        List<Customer> GetAllCustomer();
        int GenerateCustomerNo(ApplicationDbContext _context);
        Customer GetCustomerFromCustomerNo(int customerNo, ApplicationDbContext context);
        bool UpdateCustomer(Customer customer, ApplicationDbContext _context);





    }
}


