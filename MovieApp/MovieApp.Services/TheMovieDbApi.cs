using CSharpFunctionalExtensions;
using RestSharp;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public class TheMovieDbApi : ITheMovieDbApi
    {
        private readonly RestClient _restClient;
        private const string ApiKey = "1f54bd990f1cdfb230adb312546d765d";

        public TheMovieDbApi()
        {
            _restClient = new RestClient("https://api.themoviedb.org/3/");
        }

        public async Task<Result<T>> GetAsync<T>(string resource, object queryString = null) where T : new()
        {
            return await Task.Run(() =>
            {
                var restRequest = new RestRequest(resource);
                restRequest.AddQueryParameter("api_key", ApiKey);

                if (queryString != null)
                {
                    var props = queryString.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        restRequest.AddQueryParameter(prop.Name, prop.GetValue(queryString).ToString());
                    }
                }

                var response = _restClient.Get<T>(restRequest);

                if (response.ErrorException != null)
                {
                    return Result.Fail<T>(response.ErrorMessage);
                }

                return Result.Ok(response.Data);
            });
        }
    }
}
