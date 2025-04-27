using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.models;

namespace MovieAPI.AppDbContext.configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Rate).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.MovieId);
        }
    }
}
