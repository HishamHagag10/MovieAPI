using MovieAPI.AppDbContext;
using MovieAPI.models;

namespace MovieAPI.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(AppDBContext _context,ActorRepo actorRepo
            ,AwardRepo awardRepo,ActorAwardRepo actorAwardRepo,MovieRepo movieRepo,
            GenreRepo genreRepo,MovieActorsRepo movieActorsRepo,ReviewRepo reviewRepo,
            UserRepo userRepo,UserMovieRepo userMovieRepo)
        {
            ActorRepo = actorRepo;
            AwardRepo= awardRepo;
            ActorAwardRepo= actorAwardRepo;
            GenreRepo = genreRepo;
            MovieRepo = movieRepo;

            ReviewRepo = reviewRepo;
            UserRepo = userRepo;
            MovieActorsRepo = movieActorsRepo;
            UserMovieRepo = userMovieRepo;
            Recommendation = new RecommendationRepo(_context);
        }
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

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
