namespace LetsDoOnion.Domain.Core
{
    //[Table("Category")]
    public class Category
    {
        //[Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

    }
}