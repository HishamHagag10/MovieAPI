# MovieAPI

## Description

MovieAPI is a RESTful web service designed for managing a database of movies, including their genres, actors, and reviews. This API allows users to perform CRUD (Create, Read, Update, Delete) operations on movie records and manage associated data.

## Features

- **User Authentication**: Secure access to the API with user accounts.
- **Movie Management**: Create, retrieve, update, and delete movies.
- **Genre Management**: Manage genres for categorizing movies.
- **Actor Management**: Keep track of actors and their associated movies.
- **Review System**: Users can submit reviews for movies and retrieve existing reviews.
- **Search and Filter**: Search for movies by title, genre, or actor.
- **Data Validation**: Ensure data integrity with model validation.

## Technologies Used

- **Programming Language**: C#
- **Framework**: ASP.NET Core
- **ORM**: Entity Framework Core
- **Database**: SQL Server (or your choice of database)
- **Authentication**: JWT (JSON Web Tokens)
- **API Testing**: Postman or similar tools

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- SQL Server or other compatible database

### Installation

1. Clone the repository:

   ```bash
      git clone https://github.com/HishamHagag10/MovieAPI.git
   ```
2. Navigate to the project directory:

   ```bash
      cd MovieAPI
   ```
3. Restore the NuGet packages:

   ```bash
      dotnet restore
   ```
4. Update the database connection string in appsettings.json to point to your SQL Server instance.

Run the database migrations:

   ```bash
      dotnet ef database update
   ```
5. Start the application:

   ```bash
    dotnet run
   ```
## Authentication and Authorization

This API uses **JWT (JSON Web Tokens)** for authentication and authorization. Users must register and log in to access certain endpoints.

### User Registration

- **Endpoint**: `POST /api/auth/register`
- **Description**: Register a new user.
- **Request Body**: 
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Response**:
  ```json
  {
        "token": "string"
  }
  ```
### User Login

- **Endpoint**: `POST /api/auth/login`
- **Description**: Authenticate a user and receive a JWT token.
- **Request Body**: 
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Response**:
  ```json
  {
        "token": "string"
  }
  ```
## API Endpoints

### Movies

| Method | Endpoint          | Description                                |
|--------|-------------------|--------------------------------------------|
| GET    | /api/movies       | Retrieve all movies                        |
| GET    | /api/movies/{id}  | Retrieve a specific movie                  |
| POST   | /api/movies       | Create a new movie (Requires Auth)         |
| PUT    | /api/movies/{id}  | Update an existing movie (Requires Auth)   |
| DELETE | /api/movies/{id}  | Delete a specific movie (Requires Auth)    |

### Genres

| Method | Endpoint          | Description                                |
|--------|-------------------|--------------------------------------------|
| GET    | /api/genres       | Retrieve all genres                        |
| GET    | /api/genres/{id}  | Retrieve a specific genre                  |
| POST   | /api/genres       | Create a new genre (Requires Auth)         |
| PUT    | /api/genres/{id}  | Update an existing genre (Requires Auth)   |
| DELETE | /api/genres/{id}  | Delete a specific genre (Requires Auth)    |

### Actors

| Method | Endpoint          | Description                                |
|--------|-------------------|--------------------------------------------|
| GET    | /api/actors       | Retrieve all actors                        |
| GET    | /api/actors/{id}  | Retrieve a specific actor                  |
| POST   | /api/actors       | Create a new actor (Requires Auth)         |
| PUT    | /api/actors/{id}  | Update an existing actor (Requires Auth)   |
| DELETE | /api/actors/{id}  | Delete a specific actor (Requires Auth)    |

### Reviews

| Method | Endpoint          | Description                                |
|--------|-------------------|--------------------------------------------|
| GET    | /api/reviews      | Retrieve all reviews                       |
| GET    | /api/reviews/{id} | Retrieve a specific review                 |
| POST   | /api/reviews      | Submit a new review (Requires Auth)        |
| PUT    | /api/reviews/{id} | Update an existing review (Requires Auth)  |
| DELETE | /api/reviews/{id} | Delete a specific review (Requires Auth)   |

### Search

| Method | Endpoint                                     | Description                                             |
|--------|----------------------------------------------|---------------------------------------------------------|
| GET    | /api/Search/MoviesOfGenre/{genreId}          | Retrieve movies of a specific genre                     |
| GET    | /api/Search/MoviesOfActor/{actorId}          | Retrieve movies featuring a specific actor              |
| GET    | /api/Search/GetMoviesByReleaseyear/{year}    | Retrieve movies released in a specific year             |
| GET    | /api/Search/ReviewsOfMovie/{movieId}         | Retrieve reviews for a specific movie                   |
| GET    | /api/Search/ActorsOfMovie/{movieId}          | Retrieve actors of a specific movie                     |
| GET    | /api/Search/AwardsOfActor/{actorId}          | Retrieve awards won by a specific actor                 |
| GET    | /api/Search/ActorsGotAward/{awardId}         | Retrieve actors who received a specific award           |
| GET    | /api/Search/ReviewsofUser                    | Retrieve reviews by the authenticated user              |
| GET    | /api/Search/MoviesWatchedByUser              | Retrieve movies watched by the authenticated user       |
| Get    | /api/Search/MoviesWithTitleStartWith/{title} | Retrieve movies whose titles start with specific string |
### Recommendation

| Method | Endpoint                                          | Description                                                   |
|--------|---------------------------------------------------|---------------------------------------------------------------|
| GET    | /api/Recommendation/RecommendedMoviesBasedOnGenre | Retrieve recommended movies based on user’s genre preferences |
| GET    | /api/Recommendation/RecommendedMoviesBasedOnActor | Retrieve recommended movies based on user’s favorite actors   |

## Pagination and Ordering
Endpoints support pagination and ordering of results.
Use the query parameters pageIndex and pageSize to control the number of results returned per page.
Additionally, the sortBy parameter allows users to specify the attribute (rating or releaseyear) by which to sort the results,
while sortDirection can be set to either "asc" for ascending or "desc" for descending order.

## Repository Pattern

This project implements the Repository Pattern to provide an abstraction layer for data access.

### Key Components

- **IRepository Interface**: Defines the basic CRUD operations that all repositories must implement.
- **BaseRepository Class**: Implements the `IRepository` interface, providing common data access functionalities that can be reused across different repositories.
- **RecommendationRepository Class**: Contains specific data access logic for handling recommendations.

This structure promotes clean code and enhances testability by decoupling the data access logic from the business logic.

## Error Handling

The API provides structured error responses to ensure clear communication of issues. Common error responses include:

- **404 Not Found**: Returned when a requested resource does not exist.
- **400 Bad Request**: Returned for invalid input or malformed requests.
- **401 Unauthorized**: Returned when authentication is required but not provided.
- **500 Internal Server Error**: Returned for unexpected server issues.

## Error Handling

The API provides structured error responses to ensure clear communication of issues. Common error responses include:

- **404 Not Found**: Returned when a requested resource does not exist.
- **400 Bad Request**: Returned for invalid input or malformed requests.
- **401 Unauthorized**: Returned when authentication is required but not provided.
- **500 Internal Server Error**: Returned for unexpected server issues.

## Contact

For any questions, feedback, or contributions, please contact:

- **Email**: [hishamhagag18@gmail.com](mailto:hishamhagag18@gmail.com)
- **GitHub**: [HishamHagag10](https://github.com/HishamHagag10)
