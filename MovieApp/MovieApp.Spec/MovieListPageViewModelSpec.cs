using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieApp.Models;
using MovieApp.Services;
using MovieApp.ViewModels;
using Prism.Services;
using System.Threading.Tasks;

namespace MovieApp.Spec
{
    [TestClass]
    public class MovieListPageViewModelSpec
    {
        private Mock<IMovieService> _mockMovieService;

        [TestInitialize]
        public void Setup()
        {
            _mockMovieService = new Mock<IMovieService>();
        }

        [TestMethod]
        public void LoadInitialData_Should_Populate_Movies()
        {
            _mockMovieService.Setup(s => s.GetUpcomingMovies(It.IsAny<int>())).Returns(Task.FromResult(Result.Ok(new Movie[]
                   {
                    new Movie { }, new Movie { }, new Movie { }, new Movie { }
                   })));

            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, null);

            viewModel.Movies.Count.Should().Be(4);
        }

        [TestMethod]
        public void Should_Get_Movies_With_Right_Page_On_Second_Request()
        {
            _mockMovieService.Setup(s => s.GetUpcomingMovies(It.IsAny<int>())).Returns(Task.FromResult(Result.Ok(new Movie[0])));

            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, null);

            viewModel.LoadMoreMoviesCommand.Execute(null);
            _mockMovieService.Verify(s => s.GetUpcomingMovies(2));
        }

        [TestMethod]
        public void Should_Get_Movies_With_Right_Page_On_Subsequent_Requests()
        {
            _mockMovieService.Setup(s => s.GetUpcomingMovies(It.IsAny<int>())).Returns(Task.FromResult(Result.Ok(new Movie[0])));

            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, null);

            viewModel.LoadMoreMoviesCommand.Execute(null);
            _mockMovieService.Verify(s => s.GetUpcomingMovies(2));

            viewModel.LoadMoreMoviesCommand.Execute(null);
            _mockMovieService.Verify(s => s.GetUpcomingMovies(3));
        }

        [TestMethod]
        public void Should_Get_Reload_Movies_After_Several_Requests()
        {
            _mockMovieService.Setup(s => s.GetUpcomingMovies(It.IsAny<int>())).Returns(Task.FromResult(Result.Ok(new Movie[0])));

            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, null);

            viewModel.LoadMoreMoviesCommand.Execute(null);
            viewModel.LoadMoreMoviesCommand.Execute(null);
            viewModel.ReloadMoviesCommand.Execute(null);

            _mockMovieService.Verify(s => s.GetUpcomingMovies(1), Times.Exactly(2));
        }

        [TestMethod]
        public void Should_Search_For_Movies_On_Page_1()
        {
            _mockMovieService.Setup(s => s.SearchMovies(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(Result.Ok(new Movie[0])));

            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, null);
            viewModel.SearchText = "Some query";
            viewModel.LoadMoreMoviesCommand.Execute(null);
            viewModel.LoadMoreMoviesCommand.Execute(null);
            viewModel.SearchMoviesCommand.Execute(null);

            _mockMovieService.Verify(s => s.SearchMovies(It.IsAny<string>(), 1), Times.Exactly(1));
        }

        [TestMethod]
        public void LoadMoreMovies_Should_Search_Instead_Of_Get_Upcoming_If_Search_Fired()
        {
            _mockMovieService.Setup(s => s.SearchMovies(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(Result.Ok(new Movie[0])));

            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, null);
            viewModel.SearchText = "Some query";
            viewModel.SearchMoviesCommand.Execute(null);
            viewModel.LoadMoreMoviesCommand.Execute(null);
            viewModel.LoadMoreMoviesCommand.Execute(null);

            _mockMovieService.Verify(s => s.SearchMovies(It.IsAny<string>(), 1), Times.Exactly(1));
            _mockMovieService.Verify(s => s.SearchMovies(It.IsAny<string>(), 2), Times.Exactly(1));
            _mockMovieService.Verify(s => s.SearchMovies(It.IsAny<string>(), 3), Times.Exactly(1));
        }

        [TestMethod]
        public void ReloadMovies_Should_Search_Instead_Of_Get_Upcoming_If_Search_Fired()
        {
            _mockMovieService.Setup(s => s.SearchMovies(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(Result.Ok(new Movie[0])));

            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, null);
            viewModel.SearchText = "Some query";
            viewModel.SearchMoviesCommand.Execute(null);
            viewModel.ReloadMoviesCommand.Execute(null);

            _mockMovieService.Verify(s => s.SearchMovies(It.IsAny<string>(), 1), Times.Exactly(2));
        }

        [TestMethod]
        public void Should_Successfully_Make_Second_Request_If_First_One_Fails()
        {
            _mockMovieService.SetupSequence(s => s.GetUpcomingMovies(It.IsAny<int>()))
                .Returns(Task.FromResult(Result.Fail<Movie[]>("Error")))
                .Returns(Task.FromResult(Result.Ok(new Movie[]
                               {
                    new Movie { }, new Movie { }, new Movie { }, new Movie { }
                               })));

            var mockPageDialog = new Mock<IPageDialogService>();
            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, mockPageDialog.Object);

            viewModel.SearchMoviesCommand.Execute(null);
            viewModel.Movies.Count.Should().Be(4);
        }

        [TestMethod]
        public void Should_Successfully_Search_For_Movies_If_First_Even_When_First_Load_Fails()
        {
            _mockMovieService.Setup(s => s.GetUpcomingMovies(It.IsAny<int>())).Returns(Task.FromResult(Result.Fail<Movie[]>("Error")));
            _mockMovieService.Setup(s => s.SearchMovies(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(Result.Ok(new Movie[]
                               {
                    new Movie { }, new Movie { }, new Movie { }, new Movie { }
                               })));

            var mockPageDialog = new Mock<IPageDialogService>();
            var viewModel = new MovieListPageViewModel(_mockMovieService.Object, null, mockPageDialog.Object);

            viewModel.SearchText = "TEST";
            viewModel.SearchMoviesCommand.Execute(null);
            viewModel.Movies.Count.Should().Be(4);
        }
    }
}
