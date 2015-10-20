using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LetsDoOnion.Domain.Core;
using LetsDoOnion.Domain.Interfaces;

namespace LetsDoOnion.Infrastructure.Data.Repositories
{
    public class UnderIssueRepository: IRepository<UnderIssue>
    {
        private readonly ApplicationDbContext _db;

        public UnderIssueRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<UnderIssue> GetAll()
        {
            return _db.UnderIssues.ToList();
        }

        public UnderIssue Get(int id)
        {
            return _db.UnderIssues.Find(id);
        }

        public void Create(UnderIssue item)
        {
            _db.UnderIssues.Add(item);
        }

        public void Update(UnderIssue item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _db.UnderIssues.Find(id);
            if (item != null)
            {
                _db.UnderIssues.Remove(item);
            }
        }
    }
}