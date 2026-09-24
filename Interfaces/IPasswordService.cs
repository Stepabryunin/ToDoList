namespace ToDoList.Interfaces
{
    public interface IPasswordService
    {
        public string ToHashPassword(string password);
        public bool VerifyPassword(string password, string Hash);
    }
}