using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public interface ISavingsAccount: IAccount
    {



        List<IAccount> GetAllAccounts(uint id);
      

    }
}
