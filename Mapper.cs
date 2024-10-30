using AutoMapper;
using MovieAPI.Dtos;
using MovieAPI.models;

namespace MovieAPI
{
    public class Mapper: Profile
    {
        public Mapper() 
        {
            CreateMap<AddMovieDto, Movie>()
                .ForMember(m => m.Poster, op => op.Ignore())
                .ForMember(m => m.NoOfReview, op => op.Ignore())
                .ForMember(m => m.SumOfReview, op => op.Ignore())
                .ForMember(m=>m.Actors,op=>op.Ignore());

            CreateMap<UpdateMovieDto, Movie>()
                .ForMember(x=>x.Poster,op=>op.Ignore())
                .ForMember(x=>x.Actors,op=>op.Ignore())
                .ForMember(x=>x.Id,op=>op.UseDestinationValue())
                .ForMember(x=>x.GenreId,op=>op.MapFrom((x,y)=>x.GenreId.HasValue?x.GenreId:y.GenreId))
                .ForMember(x=>x.StoreLine,op=>op.MapFrom((x,y)=>string.IsNullOrEmpty(x.StoreLine)?y.StoreLine:x.StoreLine))
                .ForMember(x=>x.Title,op=>op.MapFrom((x,y)=>string.IsNullOrEmpty(x.Title)?y.Title:x.Title))
                .ForMember(x=>x.year,op=>op.MapFrom((x,y)=>x.year.HasValue?x.year:y.year))
                .ForMember(x => x.Link, op => op.MapFrom((x, y) => string.IsNullOrEmpty(x.Link) ? y.Link : x.Link))
                ;
            CreateMap<Movie, ReturnMovieDto>();

            /// 
            CreateMap<AddActorDto, Actor>()
                .ForMember(x=>x.Awards,op=>op.Ignore());

            CreateMap<Actor, ReturnActorDto>();
            CreateMap<UpdateActorDto, Actor>()
                .ForMember(x=>x.Awards,op=>op.Ignore())
                .ForMember(x => x.Id, op => op.UseDestinationValue())
                .ForMember(x => x.Email, op => op.MapFrom(((x, y) => (string.IsNullOrEmpty(x.Email) ? y.Email : x.Email))))
                .ForMember(x => x.Biography, op => op.MapFrom(((x, y) => (x.Biography is not null ? x.Biography : y.Biography))))
                .ForMember(x => x.Name, op => op.MapFrom(((x, y) => x.Name is not null ? x.Name : y.Name)));
                
            ///
            CreateMap<Review, ReturnReviewDto>();
            CreateMap<AddReviewDto, Review>();
            CreateMap<UpdateReviewDto,Review>()
                .ForMember(x => x.Id, op => op.UseDestinationValue())
                .ForMember(x => x.UserId, op =>op.UseDestinationValue())
                .ForMember(x => x.MovieId, op => op.UseDestinationValue())
                .ForMember(x => x.Description, op => op.MapFrom(((x, y) => (string.IsNullOrEmpty(x.Description)? y.Description: x.Description))))
                .ForMember(x => x.Rate, op => op.MapFrom(((x, y) => x.Rate.HasValue? x.Rate : y.Rate)));
            ;
            CreateMap<AddUserDto, User>();
        }
    }
}
