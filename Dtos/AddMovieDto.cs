namespace MovieAPI.Dtos
{
    public class AddMovieDto
    {

        public string Title { get; set; }
        public int year { get; set; }
        public string StoreLine { get; set; }
        public string Link { get; set; }
        public int GenreId { get; set; }
        public IFormFile Poster { get; set; }
        public IEnumerable<MoviesActorsDto> Actors { get; set; }
    }
}
