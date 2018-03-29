using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieApp.Services.ApiResultTypes
{
    public class MovieData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> GenreIds { get; set; }
    }
}
