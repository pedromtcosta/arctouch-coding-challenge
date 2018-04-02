using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieApp.Models;
using MovieApp.Services.ApiResultTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApp.Services.Spec
{
    [TestClass]
    public class MovieServiceSpec
    {
        private Mock<ITheMovieDbApi> _mockApi;
        private MovieService _service;

        [TestMethod]
        public async Task Should_Get_Upcoming_Movies()
        {
            _mockApi = new Mock<ITheMovieDbApi>();

            var upcomingMovies = new MoviesResponse
            {
                Results = new List<MovieData>
                {
                    new MovieData { Id = 1, Title = "Movie 1", Overview = "Some overview", PosterPath = "/movie1.jpg", GenreIds = new List<int> { 1, 2, 3 }  },
                    new MovieData { Id = 2, Title = "Movie 2", Overview = "Great movie", PosterPath = "/movie2.jpg", GenreIds = new List<int> { 3 }  },
                    new MovieData { Id = 3, Title = "Movie 3", Overview = "Could be better", PosterPath = "/movie3.jpg", GenreIds = new List<int> { 5 }  },
                    new MovieData { Id = 4, Title = "Movie 4", Overview = "No genres", PosterPath = "/movie4.jpg", GenreIds = new List<int> { }  },
                    new MovieData { Id = 5, Title = "Movie 5", Overview = "Genres null", PosterPath = "/movie5.jpg", GenreIds = null  }
                }
            };

            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "SciFi" },
                new Genre { Id = 2, Name = "Action" },
                new Genre { Id = 3, Name = "Horror" },
                new Genre { Id = 5, Name = "Drama" }
            };

            _mockApi.Setup(api => api.GetAsync<MoviesResponse>("movie/upcoming", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(upcomingMovies)));
            _mockApi.Setup(api => api.GetAsync<GenresListResponse>("genre/movie/list", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(new GenresListResponse { Genres = genres })));

            _service = new MovieService(_mockApi.Object);

            var result = await _service.GetUpcomingMovies(1);
            var movies = result.Value;

            result.IsSuccess.Should().BeTrue();
            movies[0].Should().BeEquivalentTo(new Movie { Id = 1, Title = "Movie 1", Overview = "Some overview", PosterPath = "http://image.tmdb.org/t/p/w185/movie1.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie1.jpg", Genres = "SciFi, Action, Horror" });
            movies[1].Should().BeEquivalentTo(new Movie { Id = 2, Title = "Movie 2", Overview = "Great movie", PosterPath = "http://image.tmdb.org/t/p/w185/movie2.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie2.jpg", Genres = "Horror" });
            movies[2].Should().BeEquivalentTo(new Movie { Id = 3, Title = "Movie 3", Overview = "Could be better", PosterPath = "http://image.tmdb.org/t/p/w185/movie3.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie3.jpg", Genres = "Drama" });
            movies[3].Should().BeEquivalentTo(new Movie { Id = 4, Title = "Movie 4", Overview = "No genres", PosterPath = "http://image.tmdb.org/t/p/w185/movie4.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie4.jpg", Genres = "" });
            movies[4].Should().BeEquivalentTo(new Movie { Id = 5, Title = "Movie 5", Overview = "Genres null", PosterPath = "http://image.tmdb.org/t/p/w185/movie5.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie5.jpg", Genres = "" });
        }

        [TestMethod]
        public async Task Should_Search_For_Movies()
        {
            _mockApi = new Mock<ITheMovieDbApi>();

            var upcomingMovies = new MoviesResponse
            {
                Results = new List<MovieData>
                {
                    new MovieData { Id = 1, Title = "Movie 1", Overview = "Some overview", PosterPath = "/movie1.jpg", GenreIds = new List<int> { 1, 2, 3 }  },
                    new MovieData { Id = 2, Title = "Movie 2", Overview = "Great movie", PosterPath = "/movie2.jpg", GenreIds = new List<int> { 3 }  },
                    new MovieData { Id = 3, Title = "Movie 3", Overview = "Could be better", PosterPath = "/movie3.jpg", GenreIds = new List<int> { 5 }  },
                    new MovieData { Id = 4, Title = "Movie 4", Overview = "No genres", PosterPath = "/movie4.jpg", GenreIds = new List<int> { }  },
                    new MovieData { Id = 5, Title = "Movie 5", Overview = "Genres null", PosterPath = "/movie5.jpg", GenreIds = null  }
                }
            };

            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "SciFi" },
                new Genre { Id = 2, Name = "Action" },
                new Genre { Id = 3, Name = "Horror" },
                new Genre { Id = 5, Name = "Drama" }
            };

            _mockApi.Setup(api => api.GetAsync<MoviesResponse>("search/movie", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(upcomingMovies)));
            _mockApi.Setup(api => api.GetAsync<GenresListResponse>("genre/movie/list", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(new GenresListResponse { Genres = genres })));

            _service = new MovieService(_mockApi.Object);

            var result = await _service.SearchMovies("M", 1);
            var movies = result.Value;

            result.IsSuccess.Should().BeTrue();
            movies[0].Should().BeEquivalentTo(new Movie { Id = 1, Title = "Movie 1", Overview = "Some overview", PosterPath = "http://image.tmdb.org/t/p/w185/movie1.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie1.jpg", Genres = "SciFi, Action, Horror" });
            movies[1].Should().BeEquivalentTo(new Movie { Id = 2, Title = "Movie 2", Overview = "Great movie", PosterPath = "http://image.tmdb.org/t/p/w185/movie2.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie2.jpg", Genres = "Horror" });
            movies[2].Should().BeEquivalentTo(new Movie { Id = 3, Title = "Movie 3", Overview = "Could be better", PosterPath = "http://image.tmdb.org/t/p/w185/movie3.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie3.jpg", Genres = "Drama" });
            movies[3].Should().BeEquivalentTo(new Movie { Id = 4, Title = "Movie 4", Overview = "No genres", PosterPath = "http://image.tmdb.org/t/p/w185/movie4.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie4.jpg", Genres = "" });
            movies[4].Should().BeEquivalentTo(new Movie { Id = 5, Title = "Movie 5", Overview = "Genres null", PosterPath = "http://image.tmdb.org/t/p/w185/movie5.jpg", PosterPathBig = "http://image.tmdb.org/t/p/w500/movie5.jpg", Genres = "" });
        }

        [TestMethod]
        public async Task Should_Get_Movies_With_Right_Page()
        {
            _mockApi = new Mock<ITheMovieDbApi>();

            var upcomingMovies = new MoviesResponse { Results = new List<MovieData>() };
            var genres = new List<Genre>();

            _mockApi.Setup(api => api.GetAsync<MoviesResponse>("movie/upcoming", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(upcomingMovies)));
            _mockApi.Setup(api => api.GetAsync<GenresListResponse>("genre/movie/list", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(new GenresListResponse { Genres = genres })));

            _service = new MovieService(_mockApi.Object);

            var result = await _service.GetUpcomingMovies(1);
            var movies = result.Value;

            _mockApi.Verify(api => api.GetAsync<MoviesResponse>("movie/upcoming",
                It.Is<object>(o => (int)o.GetType().GetProperty("page").GetValue(o) == 1)));
        }

        [TestMethod]
        public async Task Should_Search_Movies_With_Right_Page_And_Query()
        {
            _mockApi = new Mock<ITheMovieDbApi>();

            var upcomingMovies = new MoviesResponse { Results = new List<MovieData>() };
            var genres = new List<Genre>();

            _mockApi.Setup(api => api.GetAsync<MoviesResponse>("search/movie", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(upcomingMovies)));
            _mockApi.Setup(api => api.GetAsync<GenresListResponse>("genre/movie/list", It.IsAny<object>())).Returns(Task.FromResult(Result.Ok(new GenresListResponse { Genres = genres })));

            _service = new MovieService(_mockApi.Object);

            var result = await _service.SearchMovies("M", 1);
            var movies = result.Value;

            _mockApi.Verify(api => api.GetAsync<MoviesResponse>("search/movie",
                It.Is<object>(o => (string)o.GetType().GetProperty("query").GetValue(o) == "M" && (int)o.GetType().GetProperty("page").GetValue(o) == 1)));
        }
    }
}
