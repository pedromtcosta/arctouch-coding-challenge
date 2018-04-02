using CSharpFunctionalExtensions;
using MovieApp.Models;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public interface IMovieService
    {
        Task<Result<Movie[]>> GetUpcomingMovies(int page);
        Task<Result<Movie[]>> SearchMovies(string query, int page);
    }
}
