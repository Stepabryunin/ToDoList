namespace ToDoList.DTOs
{
    public class AuthResult
    {
        public string accessToken {get;set;}
        public UserToFront user {get;set;} 
    
        public AuthResult(string token, UserToFront us)
        {
            accessToken = token;
            user=us;
        }
    }

}