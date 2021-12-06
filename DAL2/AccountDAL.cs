﻿using System.Collections.Generic;
using DAL.Entities;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AccountDAL: IAccountDAL
    {

      
        public AccountDAL()
        {
        }
        private static List<IAccount> accounts = new List<IAccount>();
  


        //public delegate void TransactionInfo(IAccount account, double amount);
        private static List<int> accountsno = new List<int>();
      




        #region List<IAccount> GetAllAccount()
        public IEnumerable<Account> GetAllAccount(ApplicationDbContext _context)
        {
            var allAccounts = _context.Account.Include(a => a.Customer);

            if (allAccounts != null && allAccounts.Any())
            {
                
                var accounts =  allAccounts.ToList();
                accounts.ForEach( x => x.CustomerNo =  x.Customer?.CustomerNo ?? 0);
                return accounts;
            }

            //return new List<Account>();
            return null;
            //get all accounts without customerId

        }
        #endregion

        #region List<IAccount> GetAllAccount(int acc)
        public List<IAccount> GetAllAccount(int id)
        {

            List<IAccount> tempList = new List<IAccount>();
            foreach(var account in accounts)
            {
                if(account.CustomerNo == id && account.AccountStatus)
                {
                    tempList.Add(account);
                }
            }

            return tempList;
        }

        
        #endregion

        
       #region void OpenAccount(IAccount account)
        public void OpenAccount(IAccount account, ApplicationDbContext _context)
        {
           
            _context.Add(account);
            _context.SaveChanges();


        }
        #endregion

        #region bool GetAccountStatus()
        public bool GetAccountStatus()
        {
            return true;
        }
        #endregion

        #region decimal GetBalance()
        public decimal GetBalance()
        {
            return 0M;
        }
        #endregion

        #region int GenerateAccountno()
        public int GenerateAccountno()
        {
       
            //int range: -2.147483648 x 10^9 to 2.147483647 x 10^9
            int intMax = int.MaxValue;

            Random random = new Random();
            int randomint = random.Next(1, intMax);

            while (accountsno.Contains(randomint)) //never ending loop if all numbers are taken
            {
                randomint = random.Next(1, intMax);
     
            }

            accountsno.Add(randomint);
            
            return randomint;
        }
        #endregion


        #region string GetAcountType(IAccount account)
        public string GetAcountType(IAccount account)
        {
            return account.AccountType;
        }
        #endregion

        #region GetIntrestrate(IAccount account)
        public decimal GetIntrestrate(IAccount account)
        {
            return account.GetIntrestrate();
        }
        #endregion

        #region int GetCustomerId(Customer customer)
        public int GetCustomerId(Customer customer)
        {
            return customer.CustomerNo;
        }
        #endregion


        #region  void Deposit(IAccount account, decimal amount)
        public void  Deposit(IAccount acc, decimal amount, ApplicationDbContext _context)
        {
            if (acc.AccountStatus)
            {
                if (acc.AccountType == "Savings")
                {
                    SavingsAccount ca = new SavingsAccount();
                    ca.Deposit(acc, amount);

                }
                if(amount > 0)
                {
                    //condition check to prevent negative amount transaction
                    Transaction transaction = CreateTransaction(acc, acc,null, amount, "Deposit", _context);
                    _context.Add(transaction);
                }
                

                _context.Update(acc);
                _context.SaveChanges();
            }//end if account is active
            else
            {
                //account is inactive
            }

        }
        #endregion


        #region void Withdraw(IAccount account,  decimal amount, ApplicationDbContext _context)
        public void Withdraw(IAccount account, decimal amount, ApplicationDbContext _context)
        {
            if (account.AccountStatus)
            {
                if (account.AccountType == "Savings")
                {
                    SavingsAccount ca = new SavingsAccount();
                    ca.Withdraw(account, amount);
                    
                }
                if (amount > 0)
                {
                    //condition check to prevent negative amount transaction
                    var transaction = Transaction.CreateTransaction(account, account, null, amount, "Withdraw");
                    _context.Transaction.Add(transaction);
                }


                _context.Update(account);
                _context.SaveChanges();

            }//end if account is active
            else
            {
                //account is not active
            }

        }
        #endregion

        #region bool AcccountIsFound(int accountno)
        public bool AcccountIsFound(int accountno)
        {
            IAccount account = new Account();
            return account.AcccountIsFound(accountno);

        }
        #endregion


        #region void Transfer(int fromAccountno, int toAccountno, decimal amount)
        public void Transfer(IAccount fromAccount, IAccount toAccount, decimal amount, ApplicationDbContext _context)
        {

            if (fromAccount.AccountStatus)
            {
                if (amount > 0 && fromAccount.Balance >= amount)
                {
                    // aamount must greater than 0 to avoid unnecessary transaction
                    Transaction transactionfromAccount = CreateTransaction(fromAccount, toAccount, null,amount, "Transfer", _context);
                    Transaction transactiontoAccount = CreateTransaction(fromAccount, toAccount, null,amount, "Received", _context);

                    _context.Add(transactionfromAccount);
                    _context.Add(transactiontoAccount);
                }

                if (fromAccount.AccountType == "Savings")
                {
                    SavingsAccount ca = new SavingsAccount();
                    ca.Transfer(fromAccount, toAccount, amount);
                }

                _context.Update(fromAccount);
                _context.Update(toAccount);

                _context.SaveChanges();

            }//end if account is active
            else
            {
                //account is not active
            }

        }//end

        public void PayeeTransfer(int fromAccountno, int toAccountno, decimal amount, ApplicationDbContext _context)
        {
            Account fromAccount = _context.Account.FirstOrDefault(x => x.AccountNo == fromAccountno);
            Payee toAccount = _context.Payee.FirstOrDefault(x => x.PayeeAccountNumber == toAccountno);
            if (fromAccount != null && fromAccount.AccountStatus && toAccount != null)
            {
                if (amount > 0 && fromAccount.Balance >= amount)
                {
                    // aamount must greater than 0 to avoid unnecessary transaction
                    Transaction transactionfromAccount = CreateTransaction(fromAccount, null, toAccount, amount, "PayeeTransfer", _context);
                    _context.Transaction.Add(transactionfromAccount);
                }

                if (fromAccount.AccountType == "Savings")
                {
                    SavingsAccount ca = new SavingsAccount();
                    ca.PayeeTransfer(fromAccount, toAccount, amount);
                }

                _context.Update(fromAccount);
                _context.SaveChanges();

            }//end if account is active
            else
            {
                //account is not active
            }

        }//end
        #endregion

        #region IAccount GetAccount(int accountno)
        public IAccount GetAccount(int accountno, ApplicationDbContext _context)
        {

            IAccount ca = new SavingsAccount();
            
            Account acc = _context.Account.FirstOrDefault(x => x.AccountNo == accountno);

            if(acc.AccountType == "Savings")
            {
                ca = acc;
                return ca;
            }

            return ca;

        }
        #endregion

        #region List<Transaction> GetTransaction(int accountno)

        public List<Transaction> GetTransaction(int accountno, ApplicationDbContext _context)
        {
            try
            {
                var transactions = _context.Transaction.Where(x => x.Accountno == accountno);
                return transactions.ToList();
            }
            catch (Exception)
            {
                return new List<Transaction>();
            }
           
        }
        #endregion

        #region Transaction CreateTransaction(IAccount account, IAccount account2, decimal amount, string info, ApplicationDbContext _context)
        public Transaction CreateTransaction(IAccount account, IAccount account2, Payee payeeAccountNo, decimal amount, string info, ApplicationDbContext _context)
        { 
            return Transaction.CreateTransaction(account, account2, payeeAccountNo,amount, info);
        }

        
        #endregion

    }
}
