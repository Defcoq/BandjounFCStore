﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserIdentity.Models;

namespace UserIdentity.Infrastructure
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
           if(user.Email.ToLower().EndsWith("@example.com"))
            {
                return Task.FromResult(IdentityResult.Success);
            }
           else
            {

                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "EmailDomainError", Description = "Only @example.com domain are allow" }));
            }
        }
    }
}
