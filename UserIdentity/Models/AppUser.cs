using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserIdentity.Models
{
    public enum Cities { none, London,Paris,Chicaho,Yaounde }
    public enum QualificationLevels { none,Basic, Advanced}
    public class AppUser : IdentityUser
    {
        public Cities City { get; set; }
        public QualificationLevels Qualifications { get; set; }
    }
}
