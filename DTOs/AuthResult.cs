namespace ToDoList.DTOs
{
    public class AuthResult
    {
        public string AccessToken {get;set;}
        public string RefreshToken {get;set;}
        public UserToFront User {get;set;} 
    
        public AuthResult(string accessToken, string refreshToken, UserToFront us)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            User=us;
        }
    }

}