using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using BeagleCloud.Models;
using System;
namespace IdentitySample.Models {

    public class ApplicationUser : IdentityUser {


        //Below are customizations for Beagle Cloud
        public List<Tag> Tags { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime UserCreated { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


   
       
    }
}