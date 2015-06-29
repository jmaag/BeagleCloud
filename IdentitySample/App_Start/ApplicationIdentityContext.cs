namespace IdentitySample
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using AspNet.Identity.MongoDB;
	using Models;
	using MongoDB.Driver;

	public class ApplicationIdentityContext : IDisposable
	{
		public static ApplicationIdentityContext Create()
		{
			// todo add settings where appropriate to switch server & database in your own application
            var client = new MongoClient("mongodb://talyn:beagle@dogen.mongohq.com:10054/beagle-dev");
			var database = client.GetDatabase("beagle-dev");
			var users = database.GetCollection<ApplicationUser>("users");
			var roles = database.GetCollection<IdentityRole>("roles");
			return new ApplicationIdentityContext(users, roles);
		}

		private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<IdentityRole> roles)
		{
			Users = users;
			Roles = roles;
		}

		public IMongoCollection<IdentityRole> Roles { get; set; }

		public IMongoCollection<ApplicationUser> Users { get; set; }

		public Task<List<IdentityRole>> AllRolesAsync()
		{
			return Roles.Find(r => true).ToListAsync();
		}

		public void Dispose()
		{
		}
	}
}