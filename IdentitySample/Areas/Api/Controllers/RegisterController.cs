using System.Globalization;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using BeagleCloud.Controllers;
using BeagleCloud.Models;
using MongoDB.Driver;
using MongoDB.Bson;
namespace IdentitySample.Areas.Api.Controllers
{
    public class RegisterController : BaseController
    {

        /// <summary>
        /// This method registers a Beagle Tag User. 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<JsonResult> RegisterUser(string email, string password)
        {
            bool success = false;
            var roles = new List<string> { "Admin" };
      
            //create a new user for this person
            var user = new ApplicationUser { UserName = email, Email = email, Tags = new List<IdentitySample.Models.Tag>(), Roles = roles };
           
        
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var result = await UserManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                success = true;
            }

            //await ZDB.Users.UpdateOneAsync(x => x.Email == email, Builders<ApplicationUser>.Update.Push(x => x.Roles, "Admin"));


        
            return Json(new
            {
                success = success,
                UID = user.Id
               
            }, JsonRequestBehavior.AllowGet);
        }



    }
}