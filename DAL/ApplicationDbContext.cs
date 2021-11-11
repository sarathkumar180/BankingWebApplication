using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;


//BankingWebApplication.Data

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<IAccount>()
            //    .HasOne(c => c.customer)
            //    .WithMany(a => a.accounts)
            //    .HasForeignKey(f => f.CustomerNo)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .IsRequired();
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Account> Account {get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<UserRolesMapping> UserRolesMapping { get; set; }
        public DbSet<Payee> Payee { get; set; }
        public DbSet<PaymentHistory> PaymentHistoy { get; set; }
    }
}
