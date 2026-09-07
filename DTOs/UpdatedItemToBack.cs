namespace ToDoList.DTOs
{
    public class UpdatedItemToBack
    {
        public Guid Id {get; set;}
        public string Name {get;set;}
        public string Description {get;set;}

        public UpdatedItemToBack()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;

        }
    }
}