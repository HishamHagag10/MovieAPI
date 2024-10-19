namespace MovieAPI.models
{
    public class MoviesActors
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public double Salary { get; set; } 
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }

}
