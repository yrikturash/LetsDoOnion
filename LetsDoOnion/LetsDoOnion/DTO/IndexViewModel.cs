using System.Collections.Generic;
using LetsDoOnion.Domain.Core;

namespace LetsDoOnion.DTO
{
    public class IndexViewModel
    {
        public IList<Issue> Issues { get; set; }
        public IList<UnderIssue> UnderIssues { get; set; }
        public IList<Category> CategoriesList { get; set; }
        public int? ActiveCategoryId { get; set; }
    }
}