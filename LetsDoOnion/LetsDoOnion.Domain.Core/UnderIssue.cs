namespace LetsDoOnion.Domain.Core
{
    public class UnderIssue
    {
        //[Key]
        public int Id { get; set; }

        public string Text { get; set; }
        public bool IsFinished { get; set; }

        public int IssueId { get; set; }
        // Navigation properties 
        //[ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }

    }
}