using System.Data.Entity;
using LetsDoOnion.Domain.Core;
using LetsDoOnion.Infrastructure.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LetsDoOnion.Infrastructure.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("AppContext", throwIfV1Schema: false)
        {
        }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<UnderIssue> UnderIssues { get; set; }
        public DbSet<Category> Categories { get; set; }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}