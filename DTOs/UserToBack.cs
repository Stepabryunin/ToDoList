using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class UserToBack
    {
        [Required]
        public string FirtsName{get;set;}=string.Empty;
        [Required]
        public string LastName{get;set;}=string.Empty;
        [Required]
        [EmailAddress]
        public string Email{get;set;}=string.Empty;
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string Password=string.Empty;

        
    }
}
