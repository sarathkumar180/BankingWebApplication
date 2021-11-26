using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;

namespace BankingWebApplication.Models
{
    public class AccountsViewModel
    {
        public IEnumerable<Account> accounts;

        public Customer customer;
    }
}
