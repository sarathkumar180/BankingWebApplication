using System;
using System.Collections.Generic;
using DAL.Entities;
using System.Linq;
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
                return  false;
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

        public List<Customer> GetAllCustomer()
        {

            #region GetAllCustomers From Database
            // cmd.ExecuteReader()
            //Customer customer = null;
            //StringBuilder sb = new StringBuilder();
            //sb.Append("SELECT * FROM Customer");

            //using (SqlConnection conn = new SqlConnection(_connStr))
            //{
            //    using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
            //    {
            //        conn.Open();
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            customer = new Customer();
            //            customer.CustomerNo = (int)reader["CustomerNo"];
            //            customer.FirstName = reader["FirstName"].ToString();
            //            customer.LastName = reader["LastName"].ToString();
            //            customer.PhoneNumber = reader["PhoneNumber"].ToString();
            //            customer.Address = reader["Address"].ToString();

            //            databaseList.Add(customer);


            //        }

            //    }
            //}
            //return databaseList;
            #endregion
            #region Fetch data from SQL - Disconnected Mode 
            //var qry = "Select * FROM Customer";

            //using (var conn = new SqlConnection(_connStr))
            //{
            //    using (var da = new SqlDataAdapter(qry, conn))
            //    {
            //        DataSet ds = new DataSet();
            //        da.Fill(ds);// fire querry and fill with result data.

            //        DataTable dt = ds.Tables[0]; 
            //        foreach(DataRow row in dt.Rows)
            //        {
            //            Console.Write($"{row[1]}");
            //            Console.Write($"{row[2]}");
            //            Console.Write($"{row[3]}");
            //            Console.Write($"{row["PhoneNumber"]}");
            //            Console.WriteLine();
            //        }

            //    }
            //}

            #endregion
            return customers;

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
                                CustomerRole = urMapping != null ? (RoleEnum)urMapping.RoleId : RoleEnum.Customer
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
                    CustomerRole = urMapping != null ? (RoleEnum)urMapping.RoleId : RoleEnum.Customer
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
                    CustomerRole = m != null ? (RoleEnum)m.RoleId : RoleEnum.Customer
                }).AsEnumerable();

            return allCUstomers;
        }
    }
}
