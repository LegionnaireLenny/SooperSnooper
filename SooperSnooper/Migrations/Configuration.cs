namespace SooperSnooper.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SooperSnooper.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        //protected override void Seed(SooperSnooper.Models.ApplicationDbContext context)
        //{
        //    //  This method will be called after migrating to the latest version.

        //    //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //    //  to avoid creating duplicate seed data.
        //}

        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private void InitializeIdentityForEF(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string username = "admin@admin.net";
            string password = "123qwe!@#QWE";
            string email = "admin@admin.net";
            string role = "Admin";

            if (UserManager.FindByEmail(email) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = email
                };

                var userResult = UserManager.Create(user, password);
                var roleResult = RoleManager.Create(new IdentityRole(role));
                var result = UserManager.AddToRole(user.Id, role);
            }
        }
    }
}
