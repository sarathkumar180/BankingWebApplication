using System.Collections.Generic;



namespace DAL.Entities
{
    public interface IAccountDAL
    {
        IEnumerable<Account> GetAllAccount(ApplicationDbContext _context);
        List<IAccount> GetAllAccount(int id);

        int GenerateAccountno();
        string GetAcountType(IAccount account);
        decimal GetBalance();
        bool AcccountIsFound(int accoutno);

        void OpenAccount(IAccount account, ApplicationDbContext context);
        decimal GetIntrestrate(IAccount account);
        bool GetAccountStatus();

        void Deposit(IAccount acc, decimal amount, ApplicationDbContext _context);
        void Withdraw(IAccount account, decimal amount, ApplicationDbContext _context);
        void Transfer(IAccount fromAccountno, IAccount toAccountno, decimal amount, ApplicationDbContext _context);
        void PayeeTransfer(int fromAccountno, int toAccountno, decimal amount, ApplicationDbContext _context);
        IAccount GetAccount(int accountno, ApplicationDbContext _context);

        Transaction CreateTransaction(IAccount account, IAccount account2, Payee payeeAccountNo, decimal amount, string info,
            ApplicationDbContext _context);
        List<Transaction> GetTransaction(int accountno, ApplicationDbContext context);




    }
}
