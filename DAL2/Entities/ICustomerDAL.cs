using System.Collections.Generic;

namespace DAL.Entities
{
    public interface ICustomerDAL
    {
        Customer Login(User user, ApplicationDbContext _context);
        bool Register(Customer customer, ApplicationDbContext _context);
        List<Customer> GetAllCustomer(ApplicationDbContext _context);
        int GenerateCustomerNo(ApplicationDbContext _context);
        Customer GetCustomerFromCustomerNo(int customerNo, ApplicationDbContext context);
        bool UpdateCustomer(Customer customer, ApplicationDbContext _context);

        bool AddOrUpdateUserRole(int customerNo, int roleId, ApplicationDbContext context);



    }
}


