using MovieAPI.models;

namespace MovieAPI.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Genre> Genres { get;  }
        public IRepository<Movie> Movies { get;  }
        public IRepository<Review> Reviews { get; }
        public IRepository<Actor> Actors { get; }
        public IRepository<Award> Awards { get; }
        public IRepository<User> Users { get; }
        public IRepository<MoviesActors> MovieActors { get; }
        public IRepository<ActorAwards> ActorAwards { get; }



        int SaveChanges();
    }
}
