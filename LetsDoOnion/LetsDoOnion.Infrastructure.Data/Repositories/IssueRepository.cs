using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LetsDoOnion.Domain.Core;
using LetsDoOnion.Domain.Interfaces;

namespace LetsDoOnion.Infrastructure.Data.Repositories
{
    public class IssueRepository : IRepository<Issue>
    {
        private readonly ApplicationDbContext _db;

        public IssueRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Issue> GetAll()
        {
            return _db.Issues.ToList();
        }

        public Issue Get(int id)
        {
            return _db.Issues.Find(id);
        }

        public void Create(Issue item)
        {
            _db.Issues.Add(item);
        }

        public void Update(Issue item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _db.Issues.Find(id);
            if (item != null)
            {
                _db.Issues.Remove(item);
            }
        }
    }
}