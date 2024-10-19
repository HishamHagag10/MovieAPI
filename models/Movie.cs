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

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
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
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Actor> Actors { get;}
    }
}
