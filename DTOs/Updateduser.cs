using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class UpdatedUser
    {
        public string FirtsName{get;set;}=string.Empty;
        public string LastName{get;set;}=string.Empty;
        [EmailAddress]
        public string Email{get;set;}=string.Empty;
        
    }
}
