using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.models;

namespace MovieAPI.configuration
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(p => p.Genre).
                 WithMany().
                 HasForeignKey(x=>x.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(p =>p.Title).IsRequired().HasMaxLength(250);
            builder.Property(p=>p.StoreLine).HasMaxLength(2500);
        }
    }
}
