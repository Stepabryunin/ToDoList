using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class UserToLogin
    {
        [Required]
        [EmailAddress]
        public string Email{get;set;}=string.Empty;
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string Password{get;set;}=string.Empty;

        
    }
}
