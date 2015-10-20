using System.Data.Entity;
using LetsDoOnion.Domain.Core;

namespace LetsDoOnion.Infrastructure.Data.Models
{
    public class DbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            db.Categories.Add(new Category() {Name = "Everything"});

            base.Seed(db);
        }
    }
}