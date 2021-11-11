                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class SavingsAccount : Account, ISavingsAccount//must inherited account class before interfaces.
    {
        public SavingsAccount()
        {

        }

      
        //private properties
        private static List<IAccount> _savingsAccounts = new List<IAccount>();

        
        public override void OpenAccount(IAccount account)
        {
            _savingsAccounts.Add(account);
        }

        public override bool Deposit(IAccount account, decimal amount)
        {
            return base.Deposit(account, amount);
        }


        public override bool Withdraw(IAccount account, decimal amount)
        {

           return base.Withdraw(account, amount);
               
         }
        public override void Transfer(IAccount fromAccount, IAccount toAccount, decimal amount)
        {

            if (toAccount.AccountStatus)
            {
                if (toAccount.AccountType == "Savings")
                {
                    base.Transfer(fromAccount, toAccount, amount);
                }

            }//end if account is active
            else
            {
                //account is inative
            }

        }

        public override string GetAccountType(IAccount account)
        {
            return AccountType = "Savings";
        }

        public override decimal GetIntrestrate()
        {
            return Interestrate = 0.02M;
        }


        public List<IAccount> GetAllAccounts(uint id)
        {
            List<IAccount> tempList = new List<IAccount>();

            foreach(var acc in _savingsAccounts)
            {
                if(acc.CustomerId == id)
                {
                    tempList.Add(acc);
                }
            }
            return tempList;
        }

    }

}
