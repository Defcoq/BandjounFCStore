using DevExpress.AspNetCore.Bootstrap;
using DevExpress.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserIdentity.Models;
using UserIdentity.ViewModels;

namespace UserIdentity.Controllers
{
    [Authorize(Roles ="Admin,Users")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;

        public AdminController(UserManager<AppUser> usrMgr,
                IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }

        public IActionResult Index()
        {
          List<AppUser> users = userManager.Users.ToList();
            List<AppUserVM> usersVM = new List<AppUserVM>();
            foreach (AppUser user in users)
            {
                AppUserVM usVM = new AppUserVM();
                usVM.UserName = user.UserName;
                usVM.Email = user.Email;
                usVM.Id = user.Id;
                usVM.City = user.City.ToString();
                usersVM.Add(usVM);

            }
            return View(usersVM);
            //return View(userManager.Users);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if(ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await userManager.CreateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            
                AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }


                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

               
            }
            return View("Index", userManager.Users);


        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email,
                string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager,
                        user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                            password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                        || (validEmail.Succeeded
                        && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        #region add by devexpress
        public IActionResult GridViewPartial()
        {
            var db = HttpContext.RequestServices.GetService(typeof(UserIdentity.Models.AppIdentityDbContext)) as UserIdentity.Models.AppIdentityDbContext;
            return PartialView("~/Views/Admin/GridViewPartial.cshtml", db.Users);
        }
        public IActionResult GridViewPartialAddNew(UserIdentity.Models.AppUser item)
        {
            var db = HttpContext.RequestServices.GetService(typeof(UserIdentity.Models.AppIdentityDbContext)) as UserIdentity.Models.AppIdentityDbContext;
            try
            {
                if (ModelState.IsValid)
                {
                    item.Id = db.Users.Max(p => p.Id) + 1; // Remove this line if your Primary Key is automatically incremented at your database/model level
                    db.Users.Add(item);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
            }
            return PartialView("~/Views/Admin/GridViewPartial.cshtml", db.Users);
        }
        public IActionResult GridViewPartialUpdate(UserIdentity.Models.AppUser item)
        {
            var db = HttpContext.RequestServices.GetService(typeof(UserIdentity.Models.AppIdentityDbContext)) as UserIdentity.Models.AppIdentityDbContext;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Users.Update(item);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
            }
            return PartialView("~/Views/Admin/GridViewPartial.cshtml", db.Users);
        }
        public IActionResult GridViewPartialDelete(System.String Id)
        {
            var db = HttpContext.RequestServices.GetService(typeof(UserIdentity.Models.AppIdentityDbContext)) as UserIdentity.Models.AppIdentityDbContext;
            try
            {
                var item = db.Users.FirstOrDefault(i => i.Id == Id);
                if (ModelState.IsValid)
                {
                    db.Users.Remove(item);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
            }
            return PartialView("~/Views/Admin/GridViewPartial.cshtml", db.Users);
        }
        #endregion
    }
}
