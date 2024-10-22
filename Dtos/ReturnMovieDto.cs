namespace MovieAPI.Dtos
{
    public class ReturnMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int year { get; set; }
        public double Rate { get; set; }
        public string StoreLine { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public byte[] Poster { get; set; }

    }
}
