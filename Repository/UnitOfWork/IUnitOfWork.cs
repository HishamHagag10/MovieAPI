using MovieAPI.models;

namespace MovieAPI.Repository.UnitOfWork
{
    public interface IUnitOfWork //: IDisposable
    {
        public GenreRepo GenreRepo { get; }
        public MovieRepo MovieRepo { get; }
        public ReviewRepo ReviewRepo { get; }
        public ActorRepo ActorRepo { get; }
        public AwardRepo AwardRepo { get; }
        public ActorAwardRepo ActorAwardRepo { get; }
        public UserRepo UserRepo { get; }
        public MovieActorsRepo MovieActorsRepo { get; }
        public UserMovieRepo UserMovieRepo { get; }
        public RecommendationRepo Recommendation { get; }
    }
}
