using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using RestSharp;

namespace MovieApp.Services
{
    public interface ITheMovieDbApi
    {
        Task<Result<T>> GetAsync<T>(string resource, object queryString = null) where T : new();
    }
}