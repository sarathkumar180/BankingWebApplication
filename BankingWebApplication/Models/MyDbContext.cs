using System.Data.Entity;
using DAL.Entities;

namespace BankingWebApplication.Models
{
    public class MyDbContext: DbContext
    {
        //public MyDbContext(DbContextOptions<MyDbContext> context) : base(context)
        //{

        //}
        public DbSet<Customer> Customers { get; set; }
        public DbSet<IAccount> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
