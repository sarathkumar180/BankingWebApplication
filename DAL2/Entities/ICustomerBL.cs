using System.Collections.Generic;

namespace DAL.Entities
{
    public interface ICustomerBL
    {

        void Register(Customer customer);
        List<Customer> GetAllCustomer();
        int GenerateCustomerId();

       


    }
}
