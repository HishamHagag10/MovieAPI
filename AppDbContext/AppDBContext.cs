using Microsoft.EntityFrameworkCore;
using MovieAPI.models;
using System.Reflection;

namespace MovieAPI.AppDbContext
{
    public class AppDBContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorAward> ActorAwards { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<UserMovies> UserMovies { get; set; }

        // public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            var constr = config.GetConnectionString("constr");
            optionsBuilder.UseSqlServer(constr);
            
        }
    }
}
