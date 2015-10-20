using System;

namespace LetsDoOnion.Domain.Core
{
    public class Issue
    {
        public int Id { get; set; }
        public DateTime? CreatedTime { get; set; }

        public string Text { get; set; }

        public bool IsFinished { get; set; }

        // Navigation property 
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}