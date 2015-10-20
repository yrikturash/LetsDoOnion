using System;
using LetsDoOnion.Infrastructure.Data.Repositories;

namespace LetsDoOnion.Infrastructure.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private CategoryRepository _categoryRepository;
        private IssueRepository _issueRepository;
        private UnderIssueRepository _underIssueRepository;

        public CategoryRepository Categories => _categoryRepository ?? (_categoryRepository = new CategoryRepository(_db));
        public IssueRepository Issues => _issueRepository ?? (_issueRepository = new IssueRepository(_db));
        public UnderIssueRepository UnderIssues => _underIssueRepository ?? (_underIssueRepository = new UnderIssueRepository(_db));

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}