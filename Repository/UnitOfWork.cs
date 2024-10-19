using MovieAPI.models;

namespace MovieAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
            Genres = new BaseRepository<Genre>(_context);
            Movies = new BaseRepository<Movie>(_context);
            Reviews = new BaseRepository<Review>(_context);
            Actors = new BaseRepository<Actor>(_context);
            Users = new BaseRepository<User>(_context);
            Awards = new BaseRepository<Award>(_context);
            MovieActors = new BaseRepository<MoviesActors>(_context);
            ActorAwards = new BaseRepository<ActorAwards>(_context);
            /*if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role { Id = 1, Name = "Admin" });
                _context.Roles.Add(new Role { Id = 2, Name = "User" });
                _context.SaveChanges();
            }*/
        }
        public IRepository<Genre> Genres { get ; }
        public IRepository<Movie> Movies { get; }
        public IRepository<Review> Reviews { get; }
        public IRepository<Actor> Actors { get; }
        public IRepository<Award> Awards { get; }
        public IRepository<User> Users { get; }

        public IRepository<MoviesActors> MovieActors { get; }

        public IRepository<ActorAwards> ActorAwards { get; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
