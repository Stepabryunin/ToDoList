using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class UpdatedUser
    {
        public string? FirtName{get;set;}
        public string? LastName{get;set;}
        [EmailAddress(ErrorMessage ="некорректный email")]
        public string? Email{get;set;}

        public string? Password {get;set;} 
        
    }
}
