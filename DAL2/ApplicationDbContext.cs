using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Enum;


//BankingWebApplication.Data

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Payee> Payee { get; set; }
        public virtual DbSet<PaymentHistory> PaymentHistory { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<UserRolesMapping> UserRolesMapping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //                optionsBuilder.UseSqlServer("Server=127.0.0.1;initial catalog=OnlineBanking;integrated security=True;MultipleActiveResultSets=True;");
            //            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interestrate).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Account_Customer_Id");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.PhoneNumber).HasMaxLength(30);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>()
                .HasData(
                    new Customer()
                    {
                        Id = 1,
                        CustomerNo = 1111,
                        UserName = "admin",
                        Password = "admin",
                        FirstName = "Administrator",
                        LastName = "Administrator",
                        Email = "Admin@banking.com",
                        Address = "Times Square Arena,New York, USA",
                        PhoneNumber = "+17023456789"
                    }
                );




            modelBuilder.Entity<Payee>(entity =>
            {
                entity.Property(e => e.PayeeName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Payee)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payee_Customer_Id");
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PaymentHistory)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_PaymentHistory_Account_Id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.PaymentHistory)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentHistory_Customer_Id");

                entity.HasOne(d => d.Payee)
                    .WithMany(p => p.PaymentHistory)
                    .HasForeignKey(d => d.PayeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentHistory_Payee_Id");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Roles>()
                .HasData(
                    new Roles()
                    {
                        Id = 1,
                        Name = RoleEnum.Admin.ToString()
                    }, new Roles()
                    {
                        Id = 2,
                        Name = RoleEnum.Teller.ToString()

                    }, new Roles()
                    {
                        Id = 3,
                        Name = RoleEnum.Customer.ToString()

                    }
                );

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Transaction_Customer_Id");
            });

            modelBuilder.Entity<UserRolesMapping>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.UserRolesMapping)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_UserRolesMapping_CUstomer_Id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRolesMapping)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<UserRolesMapping>().HasData(
                new UserRolesMapping()
                {
                    Id = 1,
                    CustomerId = 1,
                    RoleId = 1
                }

                );


            base.OnModelCreating(modelBuilder);
        }


    }
}
