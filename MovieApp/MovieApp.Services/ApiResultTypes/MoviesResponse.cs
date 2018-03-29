using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieApp.Services.ApiResultTypes
{
    public class MoviesResponse
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("results")]
        public List<MovieData> Results { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}
