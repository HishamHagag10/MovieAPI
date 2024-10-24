﻿namespace MovieAPI.models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double Rate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
