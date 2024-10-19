namespace MovieAPI.models
{
    public class Award
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Actor> Actors { get; set; }
    }
}
