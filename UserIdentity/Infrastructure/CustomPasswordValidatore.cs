using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserIdentity.Models;

namespace UserIdentity.Infrastructure
{
    public class CustomPasswordValidatore : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if(password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = "Password cannot contains username" });
            }
            if (password.ToLower().Contains("12345"))
            {
                errors.Add(new IdentityError { Code = "PasswordContainsSequence", Description = "Password cannot contains numeric sequence" });
            }

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
