using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class CreatedItem
    {
        [Required]
        public string Name {get;set;}
        [Required]
        public string Description {get;set;}
        public CreatedItem()
        {
            Name = string.Empty;
            Description = string.Empty;

        }
    }
}