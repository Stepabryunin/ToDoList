namespace ToDoList.Entities
{
    public class RefreshToken
    {
        public Guid TokenId {get;set;}
        public string TokenHash {get;set;}
        public bool IsRevoked {get;set;}
        public string? ReplaceByToken {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime ExpireAt {get;set;}


        public Guid UserId {get;set;}
        public User? User {get;set;}

        public RefreshToken(string hash, string? oldToken, Guid userId, DateTime expireAt)
        {
            TokenId = Guid.NewGuid();
            TokenHash = hash;
            IsRevoked = false;
            ReplaceByToken = oldToken;
            CreatedAt = DateTime.UtcNow;
            ExpireAt = expireAt;
            UserId = userId;

        }
        private RefreshToken(){}
    }
}