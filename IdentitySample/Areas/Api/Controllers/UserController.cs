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

    /// <summary>
    /// This class handles all user based transactions for the Beagle Cloud
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// A beagle tag user can log in to receive a list of tags that are associated to them
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
         public async Task<JsonResult> Login (string email, string password)
        {
            if (email == null || password == null)
            {
                return Json(new
                {
                    success = false,
                    message = "missing one or more parameters",
                    UID = "",
                }, JsonRequestBehavior.AllowGet);
            }

             string UID = "";
             List<IdentitySample.Models.Tag> tags = new List<Models.Tag>();
             bool success = false;
             var message = "";

             //make the login call
            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true

            var uM = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var aM = HttpContext.GetOwinContext().Authentication;
            var signInHelper = new SignInHelper(uM, aM);
            var result = await signInHelper.PasswordSignIn(email, password, false, shouldLockout: false);

             if(result == SignInStatus.Success)
             {
                 var test2 = ZDB.Users;
                 var user = await ZDB.Users.Find(x => x.UserName == email).FirstOrDefaultAsync();
                 if(user != null)
                 {
                     //get the fields we need
                     UID = user.Id;
                     tags = user.Tags;
                     success = true;
                     //update the user
                     await ZDB.Users.UpdateOneAsync(x => x.UserName == email, Builders<ApplicationUser>.Update.Set(x => x.LastLogin, DateTime.UtcNow));
                 }
                 else
                 {
                     message = "error talking to database";
                 } 
             }
             else
             {
                 message = result.ToString();
             }
             var test = User.Identity.GetUserId();
            return Json(new
            {
                success = success,
                message = message,
                UID = UID,
                tags = tags

            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create a user to tag association by appending a new tag to the user's list of tags
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public async Task<JsonResult> RegisterTag (string pin, string label, string UID)
        {

            if (pin == null || label == null || UID == null)
            {
                return Json(new
                    {
                        success = false, 
                        message = "missing one or more parameters"
                    }, JsonRequestBehavior.AllowGet);
            }

            var success = false;
            var message = "";
            var ObjectId = new ObjectId(UID);


            //make sure the user exists
            var user = await ZDB.Users.Find(x => x.Id == UID).FirstOrDefaultAsync();
            if(user!= null)
            {
                success = true;

              IdentitySample.Models.Tag currentTag = new Models.Tag { label = label, pin = pin };
              var result =  await ZDB.Users.UpdateOneAsync(x => x.Id == UID, Builders<ApplicationUser>.Update.Push(x => x.Tags, currentTag));

            }
            else
            {
                message = "no such user found";
            }

            return Json(new
            {
                success = success

            }, JsonRequestBehavior.AllowGet);

            
        }

        /// <summary>
        /// Return a list of tags for a user
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetTags (string UID)
        {
            if(UID == null)
            {
                return Json(new
                {
                    success = false,
                    message = "missing one or more parameters"
                }, JsonRequestBehavior.AllowGet);
            }
            var success = false;
            var message = "";
            var tags = new List<IdentitySample.Models.Tag>();

            //make sure the user exists
            var user = await ZDB.Users.Find(x => x.Id == UID).FirstOrDefaultAsync();
            if (user != null)
            {
                success = true;
                tags = user.Tags;
            }
            else
            {
                message = "no such user found";
            }

            return Json(new
            {
                success = success,
                message = message,
                tags = tags
            }, JsonRequestBehavior.AllowGet);

        }

    }







}