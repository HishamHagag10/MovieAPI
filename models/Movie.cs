using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int year { get; set; }
        public int NoOfReview {  get; set; }
        public double SumOfReview { get; set; }

        [NotMapped]
        public double Rate { 
            get {
                return NoOfReview==0?0d:SumOfReview/NoOfReview;
            }
        }
        public string StoreLine { get; set; }
        public byte[] Poster { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<User> UsersWatched { get; set; }
    }
}
