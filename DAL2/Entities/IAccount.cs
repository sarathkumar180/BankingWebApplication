using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public interface IAccount
    {
        
         //properties
        //List<Account> getAllAccount(uint id);
        string AccountType { get; set; }
        int AccountNo { get; set; }

        decimal Balance { get; set; }
        bool AccountStatus { get; set; }

        [NotMapped]
        int CustomerNo { get; set; }
        //Customer customer { get; set; } // account belong to 1 customer
        decimal Interestrate { get; set; }
        int CustomerId { get; set; }
        Customer customer { get; set; }

     


        //List<Transaction> transactions { get; set; }


        //methods
        void OpenAccount(IAccount account);
        void CloseAccount(IAccount account);
        string GetAccountType(IAccount account);
        bool Withdraw(IAccount account, decimal amount);
        bool Deposit(IAccount account, decimal amount);
        void Transfer(IAccount fromAccount, IAccount toAccount, decimal amount);
        void PayeeTransfer(IAccount fromAccount, Payee toAccount, decimal amount);
        bool AcccountIsFound(int accountno);
        IAccount GetAccount(int accountno);
        decimal GetIntrestrate();

        


    }
}
