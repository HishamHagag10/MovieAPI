namespace MovieAPI.models
{
    public class Actor:Person
    {
        public string? Biography { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Award> Awards { get; set; }
    }
}
