using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.models;

namespace MovieAPI.AppDbContext.configuration
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.ToTable("Actors");
            builder.HasKey(p => p.Id);

            builder.HasMany(x => x.Movies)
                .WithMany(x => x.Actors)
                .UsingEntity<MoviesActors>(
                            l => l.HasOne(x => x.Movie).WithMany().HasForeignKey(x => x.MovieId),
                            r => r.HasOne(x => x.Actor).WithMany().HasForeignKey(x => x.ActorId)
                           )
                .HasKey(x => new { x.MovieId, x.ActorId })
                ;

            builder.HasMany(x => x.Awards)
                .WithMany(x => x.Actors)
                .UsingEntity<ActorAward>(
                l => l.HasOne(x => x.Award).WithMany().HasForeignKey(x => x.AwardId),
                r => r.HasOne(x => x.Actor).WithMany().HasForeignKey(x => x.ActorId)
                )
                .HasKey(x => new { x.ActorId, x.AwardId, x.YearOfHonor });
        }
    }
}
