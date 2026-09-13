namespace ToDoList.Entities
{
    public class Item
    {
        public Guid Id {get; set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}

        public Guid UserId {get;set;}
        public User? User {get;set;}

        public Item()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt=null;
        }

    }
}