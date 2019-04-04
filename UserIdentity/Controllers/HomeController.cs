using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserIdentity.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View(GetData(nameof(Index)));
        }

        [Authorize(Roles = "Users")]
        public IActionResult OtherAction()
        {
            return View("Index", GetData(nameof(OtherAction)));
        }

        private Dictionary<string, object> GetData(string actionName) { 
           return new Dictionary<string, object>
           {
               ["Action"] = actionName,
               ["User"] = HttpContext.User.Identity.Name,
               ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
               ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
               ["In Users Role"] = HttpContext.User.IsInRole("Users")
           };
        }
    }
}
