using System.Collections.Generic;
using DAL.Entities;
using System.Linq;
using DAL.Enum;

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
        public void Register(Customer customer, ApplicationDbContext _context)
        {

            #region insert data to database
            //connect to database, store a record
            // cmd.ExecuteNonQuery()


            //StringBuilder sb = new StringBuilder();
            //sb.Append("INSERT INTO Customer");
            //sb.Append("(FirstName, LastName, PhoneNumber, Address)");
            //sb.Append("Values(");
            //sb.Append($"'{cus.FirstName}', '{cus.LastName}', ' {cus.PhoneNumber}', ' {cus.Address}' ");
            //sb.Append(")");


            //using (SqlConnection conn = new SqlConnection(_connStr))
            //{
            //    using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
            //    {
            //        conn.Open();
            //        cmd.ExecuteNonQuery();

            //    }
            //}


            #endregion


            //for unit testing
            customers.Add(customer);

            //add data to database;

            _context.Add(customer);
            _context.SaveChanges();


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



        public int GenerateCustomerId()
        {

            //int range: -2.147483648 x 10^9 to 2.147483647 x 10^9

            int intMax = int.MaxValue;
            int id = 900000001;
            //not yet checking the repeated value from database;
            while (customersId.Contains(id) && id < intMax) //prevent never ending loop if all numbers are taken.
            {
                id++;

            }

            customersId.Add(id);

            return id;

        }

        public Customer Login(User user, ApplicationDbContext context)
        {
            var userinfo = (from c in context.Customer
                            join m in context.UserRolesMapping on c.Id equals m.CustomerId
                            where c.UserName == user.UserName && c.Password == user.Password
                            select new Customer()
                            {
                                Id = c.Id,
                                UserName = c.UserName,
                                CustomerNo = c.CustomerNo,
                                Address = c.Address,
                                Email = c.Email,
                                FirstName = c.FirstName,
                                LastName = c.LastName,
                                CustomerRole = m != null ? (RoleEnum)m.RoleId : RoleEnum.Customer
                            }).FirstOrDefault();

            return userinfo;
        }
    }
}
