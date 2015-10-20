using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LetsDoOnion.Domain.Core;
using LetsDoOnion.Domain.Interfaces;

namespace LetsDoOnion.Infrastructure.Data.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Categories.ToList();
        }

        public Category Get(int id)
        {
            return _db.Categories.Find(id);
        }

        public void Create(Category item)
        {
            _db.Categories.Add(item);
        }

        public void Update(Category item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _db.Categories.Find(id);
            if (item!=null)
            {
                _db.Categories.Remove(item);
            }
        }
    }
}