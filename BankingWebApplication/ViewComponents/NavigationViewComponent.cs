using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingWebApplication.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public NavigationViewComponent()
        {

        }

        public IViewComponentResult Invoke() {

            var partialViewName = "_NavigationPublic";
            if (HttpContext.Session.GetString("CustomerNo") != null)
            {
                var role = HttpContext.Session.GetString("UserRole");
                if (!string.IsNullOrEmpty(role))
                {
                    if (role == RoleEnum.Admin.ToString())
                    {
                        partialViewName = "_NavigationAdmin";
                    }
                    else if (role == RoleEnum.Teller.ToString())
                    {
                        partialViewName = "_NavigationTeller";
                    }
                    else
                    {
                        partialViewName = "_NavigationCustomer";
                    }
                }

            }

            return View(partialViewName);
        }
    }
}
