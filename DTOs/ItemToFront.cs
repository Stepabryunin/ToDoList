using ToDoList.Entities;

namespace ToDoList.DTOs
{
    public class ItemToFront
    {
        public Guid Id {get; set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}

        public ItemToFront()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt=null;
        }
    }

    
}