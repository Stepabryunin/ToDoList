using System.ComponentModel.DataAnnotations;

namespace ToDoList.Entities
{
    public class User
    {
        [Required]
        public Guid Id {get;set;}
        [Required]
        public string FirstName {get;set;}
        [Required]
        public string LastName {get;set;}
        [EmailAddress]
        [Required]
        public string Email {get;set;}
        [Required]
        public string PasswordHash{get;set;}
        [Required]
        public DateTime CreatedAt {get;set;}

        public List<Item> Items {get;set;} = new List<Item>();
        
    }
}