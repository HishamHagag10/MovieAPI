namespace MovieAPI.models
{
    /*public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set;}
    }*/
    public enum Role
    {
        User = 0, 
        Admin = 1,
    }
}
