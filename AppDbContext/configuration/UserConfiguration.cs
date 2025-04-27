using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.models;

namespace MovieAPI.AppDbContext.configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasMany(x => x.WatchedMovies)
                .WithMany(x => x.UsersWatched)
                .UsingEntity<UserMovies>(
                r => r.HasOne(x => x.Movie).WithMany().HasForeignKey(x => x.MovieId),
                l => l.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
                ).HasKey(x => x.Id);

        }
    }
}
