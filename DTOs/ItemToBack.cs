using System.ComponentModel.DataAnnotations;
using ToDoList.Entities;

namespace ToDoList.DTOs
{
    public class ItemToBack
    {
        [Required]
        public Guid Id {get; set;}
        public string Name {get;set;}
        public string Description {get;set;}


        public ItemToBack()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
        }
    }

    
}