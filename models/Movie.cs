
namespace MovieAPI.models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int year { get; set; }
        public int NoOfReview {  get; set; }
        public double SumOfReview { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double Rate { set; get; }
        public string StoreLine { get; set; }
        public byte[] Poster { get; set; }
        public string Link { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<User> UsersWatched { get; set; }
    }
}
