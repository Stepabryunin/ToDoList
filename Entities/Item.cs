namespace ToDoList.Entities
{
    public class Item
    {
        public Guid Id {get; set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}

        public Item()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt=null;
        }
        public Item(string _name, string _description)
        {
            Id = Guid.NewGuid();
            Name = _name;
            Description = _description;
            CreatedAt = DateTime.Now;
            UpdatedAt=null;
        }
        public void Update (string _name=null, string _description=null)
        {
            bool UpdateFlag=false;
            if (_name != null && _name != string.Empty)
            {
                UpdateFlag=true;
                Name = _name;
            }
            if (_description != null && _description != string.Empty)
            {
                UpdateFlag=true;
                Description =_description;
            }
            if (UpdateFlag)
                UpdatedAt=DateTime.Now;
        }
    }
}