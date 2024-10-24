﻿namespace MovieAPI.models
{
    public class User:Person
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Movie> WatchedMovies { get; set; }

    }
}
