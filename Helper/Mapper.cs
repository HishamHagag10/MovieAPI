
namespace MovieAPI.Helper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AddMovieCommand, Movie>()
                .ForMember(m => m.Poster, op => op.Ignore())
                .ForMember(m => m.NoOfReview, op => op.Ignore())
                .ForMember(m => m.SumOfReview, op => op.Ignore())
                .ForMember(m => m.Actors, op => op.Ignore());

            CreateMap<UpdateMovieCommand, Movie>()
                .ForMember(x => x.Poster, op => op.Ignore())
                .ForMember(x => x.Actors, op => op.Ignore())
                .ForMember(x => x.Id, op => op.UseDestinationValue())
                .ForMember(x => x.GenreId, op => op.MapFrom((x, y) => x.GenreId.HasValue ? x.GenreId : y.GenreId))
                .ForMember(x => x.StoreLine, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.StoreLine) ? y.StoreLine : x.StoreLine))
                .ForMember(x => x.Title, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.Title) ? y.Title : x.Title))
                .ForMember(x => x.year, op => op.MapFrom((x, y) => x.year.HasValue ? x.year : y.year))
                .ForMember(x => x.Link, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.Link) ? y.Link : x.Link))
                ;
            CreateMap<Movie, MovieResponseDto>();

            CreateMap<MoviesActors, MovieActorResponseDto>();


            CreateMap<WatchMovieCommand, UserMovies>()
                .ForMember(x=>x.WatchedAt,op=>op.MapFrom(y=>DateTime.Now));
            
            CreateMap<UserMovies, UserMovieResponse>();
            // Actors
            CreateMap<AddActorCommand, Actor>();

            
            CreateMap<Actor, ActorResponseDto>();

            CreateMap<UpdateActorCommand, Actor>()
                .ForMember(x => x.Id, op => op.UseDestinationValue())
                .ForMember(x => x.Email, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.Email) ? y.Email : x.Email))
                .ForMember(x => x.Biography, op => op.MapFrom((x, y) => x.Biography is not null ? x.Biography : y.Biography))
                .ForMember(x => x.Name, op => op.MapFrom((x, y) => x.Name is not null ? x.Name : y.Name));


            // Award 
            CreateMap<AddAwardCommand, Award>();
            CreateMap<Award,AwardResponseDto>();
            CreateMap<UpdateAwardCommand, Award>()
                .ForMember(x => x.Id, op => op.UseDestinationValue())
                .ForMember(x => x.Name, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.Name) ? y.Name : x.Name));

            CreateMap<AddAwardToActorCommand,ActorAward>();
            CreateMap<ActorAward,ActorAwardResponseDto>();


            // Genre
            CreateMap<AddGenreCommand, Genre>();
            CreateMap<Genre,GenreResponseDto>();
            CreateMap<UpdateGenreCommand, Genre>()
               .ForMember(x => x.Id, op => op.UseDestinationValue())
               .ForMember(x => x.Name, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.Name) ? y.Name : x.Name));



            // Review
            CreateMap<Review, ReviewResponseDto>();
            CreateMap<AddReviewCommand, Review>()
                   .ForMember(x => x.CreatedAt, op => op.MapFrom(a => DateTime.Now))
                   .ForMember(x => x.UpdatedAt, op => op.MapFrom(a => DateTime.Now));
            ;

            CreateMap<UpdateReviewCommand, Review>()
                .ForMember(x => x.Id, op => op.UseDestinationValue())
                .ForMember(x => x.UserId, op => op.UseDestinationValue())
                .ForMember(x => x.MovieId, op => op.UseDestinationValue())
                .ForMember(x => x.CreatedAt, op => op.UseDestinationValue())
                .ForMember(x => x.Description, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.Description) ? y.Description : x.Description))
                .ForMember(x => x.Rate, op => op.MapFrom((x, y) => x.Rate.HasValue ? x.Rate : y.Rate))
                .ForMember(x=>x.UpdatedAt,op=>op.MapFrom(a=>DateTime.Now));
            ;


            // User
            CreateMap<RegisterUserCommand, User>();
            CreateMap<UpdateUserCommand, User>()
                .ForMember(x => x.Email, op => op.MapFrom((s, d) => string.IsNullOrEmpty(s.Email) ? d.Email : s.Email))
                .ForMember(x => x.Name, op => op.MapFrom((s, d) => s.Name is not null ? s.Name : d.Name));

        }
    }
}
