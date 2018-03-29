using MovieApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieApp.Services.ApiResultTypes
{
    public class GenresListResponse
    {
        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }
    }
}
