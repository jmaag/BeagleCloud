using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Identity.MongoDB;
using IdentitySample.Models;
using MongoDB.Driver;

namespace BeagleCloud.Models
{

   
    public class DBContext
    {
        //these are the collections we have access to
        public IMongoCollection<IdentityRole> Roles { get; set; }
        public IMongoCollection<ApplicationUser> Users { get; set; }




        public DBContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("beagledb");
        }





    }
}