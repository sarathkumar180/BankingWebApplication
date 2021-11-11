using System.Collections.Generic;

namespace DAL.Entities
{
    public interface IAccountBL
    {
        List<IAccount> GetAllAccount(ApplicationDbContext _context);
        List<IAccount> GetAllAccount(int id);

        int GenerateAccountno();
        string GetAcountType(IAccount account);
        decimal GetBalance();
        bool AcccountIsFound(int accoutno);

        void OpenAccount(IAccount account, ApplicationDbContext _context);
        void CloseAccount(IAccount accountno);
        void Deposit(IAccount account, decimal amount, ApplicationDbContext _context);
        void Withdraw(IAccount account, decimal amount, ApplicationDbContext _context);
        void Transfer(IAccount fromAccountno, IAccount toAccountno, decimal amount);

        IAccount GetAccount(int accountno, ApplicationDbContext _context);
        List<Transaction> GetTransaction(int accountno);




    }

}
