namespace MovieAPI.models.Dtos.Response_Dtos
{
    public class MovieResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int year { get; set; }
        public double Rate { get; set; }
        public string StoreLine { get; set; }
        public string Link { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        //public byte[] Poster { get; set; }

    }
}
