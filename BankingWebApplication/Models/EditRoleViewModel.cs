using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingWebApplication.Models
{
    public class EditRoleViewModel
    {
        public List<SelectList> RolesList { get; set; }
        public string SelectedRole { get; set; }
        public int CustomerNo { get; set; }
        public string UserName { get; set; }


    }
}
