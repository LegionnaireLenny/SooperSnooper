using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SooperSnooper.Models.Twitter;

namespace SooperSnooper.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<User> TwitterUsers { get; set; }
        public DbSet<Tweet> Tweets { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}