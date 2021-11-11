using System.Collections.Generic;

namespace DAL.Entities
{
    public class TransferAccount
    {
        public Account account { get; set; }
        public List<Account> accounts { get; set; }
    }
}
