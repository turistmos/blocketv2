using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace blocket_lite_v2.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }
    }
}

