using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserIdentity.Controllers
{
    public class ClaimsController : Controller
    {
        public IActionResult Index()
        {
            return View(User?.Claims);
        }
    }
}
