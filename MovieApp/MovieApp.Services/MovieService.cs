using CSharpFunctionalExtensions;
using MovieApp.Models;
using MovieApp.Services.ApiResultTypes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly ITheMovieDbApi _api;
        private const string PosterPathBase = "http://image.tmdb.org/t/p/w185";
        private const string PosterPathBigBase = "http://image.tmdb.org/t/p/w500";

        public MovieService(ITheMovieDbApi api)
        {
            _api = api;
        }

        public async Task<Result<Movie[]>> GetUpcomingMovies(int page)
        {
            var result = await _api.GetAsync<MoviesResponse>("movie/upcoming", new { page });
            return await GenerateMoviesFromResult(result);
        }

        public async Task<Result<Movie[]>> SearchMovies(string query, int page)
        {
            var result = await _api.GetAsync<MoviesResponse>("search/movie", new { query, page });
            return await GenerateMoviesFromResult(result);
        }

        private async Task<Result<Movie[]>> GenerateMoviesFromResult(Result<MoviesResponse> result)
        {
            var genresResult = await GetGenres();

            if (result.IsSuccess && genresResult.IsSuccess)
            {
                var genres = genresResult.Value;
                var response = result.Value;
                var movies = response.Results.Select(r => new Movie
                {
                    Id = r.Id,
                    Overview = r.Overview,
                    PosterPath = PosterPathBase + r.PosterPath,
                    PosterPathBig = PosterPathBigBase + r.PosterPath,
                    ReleaseDate = r.ReleaseDate,
                    Title = r.Title,
                    Genres = string.Join(", ", genres.Where(g => r.GenreIds != null && r.GenreIds.Contains(g.Id)).Select(g => g.Name).ToArray())
                }).ToArray();

                return Result.Ok(movies);
            }
            else
            {
                return Result.Fail<Movie[]>("Error retrieving Movie List");
            }
        }

        private async Task<Result<List<Genre>>> GetGenres()
        {
            var result = await _api.GetAsync<GenresListResponse>("genre/movie/list");
            if (result.IsSuccess)
            {
                return Result.Ok(result.Value.Genres);
            }
            return Result.Fail<List<Genre>>("Error retrieving genres");
        }
    }
}
