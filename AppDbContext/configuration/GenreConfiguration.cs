﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.models;

namespace MovieAPI.AppDbContext.configuration
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(b => b.Name).HasMaxLength(100);
            builder.HasKey(b => b.Id);

        }
    }
}
