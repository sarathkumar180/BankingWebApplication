using System;
using DAL.Entities;
using DAL;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class AccountBL
    {
        IAccountDAL _accountdaltest;
        public AccountBL()
        {
            _accountdaltest = new AccountDAL();
           
        }
        public AccountBL(IAccountDAL context)
        {
            _accountdaltest = context;
        }


        public int Accountno { get; set; }
        IAccountDAL accountdal = new AccountDAL();

        public IEnumerable<Account> GetAllAccount(ApplicationDbContext _context)
        {
            return accountdal.GetAllAccount(_context);
        }

        public List<IAccount> GetAllAccount(int id)
        {
            return accountdal.GetAllAccount(id);
        }

       public void OpenAccount(IAccount account, ApplicationDbContext _context)
        {
            accountdal.OpenAccount(account, _context);
        }

         
         public int GenerateAccountno()
         {
            return accountdal.GenerateAccountno();
         }

    

        public string GetAcountType(IAccount account)
        {
            return accountdal.GetAcountType(account);
        }


        public decimal GetIntrestrate(IAccount account)
        {
            return accountdal.GetIntrestrate(account);
        }


        public decimal GetBalance()
        {
            return accountdal.GetBalance();
        }

        public bool GetAccountStatus()
        {
            return accountdal.GetAccountStatus();
        }
        
        public void  Deposit(IAccount account, decimal amount, ApplicationDbContext _context) 
        {
            if (amount > 0)
            {
                accountdal.Deposit(account, amount, _context);
        }


    }

        public void Withdraw(IAccount account, decimal amount, ApplicationDbContext _context)
        {
            if (amount > 0)
            {
                accountdal.Withdraw(account, amount, _context);
            }
             
        }

        public bool AcccountIsFound(int accoutno)
        {
            return accountdal.AcccountIsFound(accoutno);
        }

        public void Transfer(IAccount fromAccount, IAccount toAccount, decimal amount, ApplicationDbContext _context)
        {
            accountdal.Transfer(fromAccount, toAccount, amount, _context);
        }

       #region Transaction CreateTransaction(IAccount account, IAccount account2, decimal amount, string info,ApplicationDbContext _context)
        public Transaction CreateTransaction(IAccount account, IAccount account2, decimal amount, string info,ApplicationDbContext _context)
        {
            return accountdal.CreateTransaction(account, account2, amount, info,_context);
        }
        #endregion

        #region IAccount GetAccount(int id, ApplicationDbContext _context)
        public IAccount GetAccount(int id, ApplicationDbContext _context)
        {
            return accountdal.GetAccount(id,_context);
        }
        #endregion

        #region List<Transaction> GetTransaction(int accountno)
        public List<Transaction> GetTransaction(int accountno)
        {
            return accountdal.GetTransaction(accountno);

        }
        #endregion



    }
}
