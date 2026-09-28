using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Application.Entities;

namespace Application.Entites
{
   
   public class AppUsers: IdentityUser<int>
    {
        public string Name { get; set; }
        
        public String OTP { get; set; }

        public DateTime? OTPExpiryTime { get; set; }

        public bool IsDeleted { get; set; }


        public string? ImageSrc { get; set; }
        public string Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String PostalCode { get; set; }
        public String CnicNumber { get; set; }
        public DateTime CnicExpiry { get; set; }
        public int Role { get; set; }
        //public List<AppUserRole> AppUserRole { get; set; }

        public List<AppProperties> AppProperties { get; set; }
        //public List<UserJobs> UserJobs { get; set; }

        public UserSettings UserSettings { get; set; }



    }
}
