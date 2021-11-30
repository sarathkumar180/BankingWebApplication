using System;
using System.Collections.Generic;
using System.ComponentModel;
using DAL.Entities;
using System.Linq;
using System.Net.Sockets;
using DAL.Enum;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class CustomerDAL : ICustomerDAL
    {


        //private properties


        private static List<Customer> customers = new List<Customer>();
        private static List<int> customersId = new List<int>();


        //private static List<Customer> databaseList = new List<Customer>();

        //private string _connStr = "Server=tcp:jlsql666.database.windows.net,1433;Initial Catalog=SQLClass;Persist Security Info=False;User ID=jladmin;Password=Ss26279205;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private string _connStr;
        //private string _appSetting;

        //public methods



        #region async void Register(Customer customer, ApplicationDbContext _context)
        public bool Register(Customer customer, ApplicationDbContext _context)
        {
            try
            {
                customer.CustomerNo = GenerateCustomerNo(_context);
                //for unit testing
                customers.Add(customer);

                //add data to database;

                _context.Add(customer);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool UpdateCustomer(Customer customer, ApplicationDbContext context)
        {
            try
            {
                context.Update(customer);
                context.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
        #endregion

        public List<Customer> GetAllCustomer(ApplicationDbContext context)
        {
            try
            {
                var userInfo = (from c in context.Customer
                                join m in context.UserRolesMapping on c.Id equals m.CustomerId into urm
                                from urMapping in urm.DefaultIfEmpty()
                                select new Customer()
                                {
                                    Id = c.Id,
                                    UserName = c.UserName,
                                    CustomerNo = c.CustomerNo,
                                    Address = c.Address,
                                    PhoneNumber = c.PhoneNumber,
                                    Email = c.Email,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    CustomerRole = urMapping != null ? ((RoleEnum)urMapping.RoleId).ToString() : string.Empty,
                                }).ToList();

                return userInfo.Where(x => x.CustomerRole != RoleEnum.Admin.ToString()).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return customers;
            }

        }

        public List<UserRolesMapping> GetAllCustomersWithRoles(ApplicationDbContext context)
        {
            try
            {

                var returnList = (from c in context.Customer
                                  join crm in context.UserRolesMapping on c.Id equals crm.CustomerId
                                  where crm.RoleId != 3 //Ignore admin and Teller users
                                  select new UserRolesMapping()
                                  {
                                      Id = crm.Id,
                                      RoleId = crm.RoleId,
                                      CustomerId = crm.CustomerId,
                                      CustomerName = c.FirstName + " " + c.LastName,
                                      CustomerNo = c.CustomerNo,
                                      RoleName = crm.RoleId == 0 ? string.Empty : ((RoleEnum)crm.RoleId).ToString()
                                  }).ToList();

                return returnList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public int GenerateCustomerNo(ApplicationDbContext context)
        {
            //int range: -2.147483648 x 10^9 to 2.147483647 x 10^9

            int intMax = int.MaxValue;
            int id = 90000000;

            customersId = GetAllCustiomers(context).Select(x => x.CustomerNo).ToList();
            //not yet checking the repeated value from database;
            while (customersId != null && customersId.Contains(id) && id < intMax) //prevent never ending loop if all numbers are taken.
            {
                id++;

            }

            return id;

        }

        public Customer Login(User user, ApplicationDbContext context)
        {
            var userinfo = (from c in context.Customer
                            join m in context.UserRolesMapping on c.Id equals m.CustomerId into urm
                            from urMapping in urm.DefaultIfEmpty()
                            where c.UserName == user.UserName && c.Password == user.Password
                            select new Customer()
                            {
                                Id = c.Id,
                                UserName = c.UserName,
                                CustomerNo = c.CustomerNo,
                                Address = c.Address,
                                PhoneNumber = c.PhoneNumber,
                                Email = c.Email,
                                FirstName = c.FirstName,
                                LastName = c.LastName,
                                CustomerRole = urMapping != null ? ((RoleEnum)urMapping.RoleId).ToString() : string.Empty,
                            }).FirstOrDefault();

            return userinfo;
        }

        public Customer GetCustomerFromCustomerNo(int customerNo, ApplicationDbContext context)
        {

            var userinfo = (from c in context.Customer
                            join m in context.UserRolesMapping on c.Id equals m.CustomerId into urm
                            from urMapping in urm.DefaultIfEmpty()
                            where c.CustomerNo == customerNo
                            select new Customer()
                            {
                                Id = c.Id,
                                UserName = c.UserName,
                                CustomerNo = c.CustomerNo,
                                Address = c.Address,
                                PhoneNumber = c.PhoneNumber,
                                Email = c.Email,
                                FirstName = c.FirstName,
                                LastName = c.LastName,
                                CustomerRole = urMapping != null ? ((RoleEnum)urMapping.RoleId).ToString() : string.Empty,
                                Password = c.Password
                            }).FirstOrDefault();

            return userinfo;
        }

        public IEnumerable<Customer> GetAllCustiomers(ApplicationDbContext context)
        {

            var allCUstomers = (from c in context.Customer
                                join m in context.UserRolesMapping on c.Id equals m.CustomerId
                                select new Customer()
                                {
                                    Id = c.Id,
                                    UserName = c.UserName,
                                    CustomerNo = c.CustomerNo,
                                    Address = c.Address,
                                    PhoneNumber = c.PhoneNumber,
                                    Email = c.Email,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    CustomerRole = m != null ? ((RoleEnum)m.RoleId).ToString() : string.Empty
                                }).AsEnumerable();

            return allCUstomers;
        }

        public bool AddOrUpdateUserRole(int customerNo, int roleId, ApplicationDbContext context)
        {
            try
            {
                var customerId = context.Customer.Where(x => x.CustomerNo == customerNo).Select(x => x.Id).FirstOrDefault();
                var userRole = context.UserRolesMapping
                    .FirstOrDefault(x => x.CustomerId == customerId);
                if (userRole == null)
                {
                    userRole = new UserRolesMapping();
                    userRole.RoleId = roleId;
                    userRole.CustomerId = customerId;
                    context.UserRolesMapping.Add(userRole);
                }
                else
                {
                    userRole.RoleId = roleId;
                    userRole.CustomerId = customerId;
                    context.UserRolesMapping.Update(userRole);
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<Payee> GetPayeesForCustomerNo(int customerNo, ApplicationDbContext context)
        {
            try
            {
                var customerId = context.Customer.Where(x => x.CustomerNo == customerNo).Select(x => x.Id).FirstOrDefault();
                return context.Payee.Where(x => x.CustomerId == customerId).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool AddOrUpdatePayee(Payee model, ApplicationDbContext context)
        {
            try
            {
                var customerId = context.Customer.Where(x => x.CustomerNo == model.CustomerNo).Select(x => x.Id).FirstOrDefault();
                if (customerId != 0)
                {
                    model.CustomerId = customerId;
                    if (model.Id > 0)
                    {
                        context.Payee.Update(model);
                    }
                    else
                    {
                        context.Payee.Add(model);
                    }
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Account> GetAllAccountsForCustomer(int customerNo, ApplicationDbContext context)
        {
            try
            {
                var customerId = context.Customer.Where(x => x.CustomerNo == customerNo).Select(x => x.Id).FirstOrDefault();
                if (customerId != 0)
                {
                    return context.Account.Where(x => x.CustomerId == customerId).ToList();
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
