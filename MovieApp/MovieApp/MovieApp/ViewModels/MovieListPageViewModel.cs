using CSharpFunctionalExtensions;
using MovieApp.Models;
using MovieApp.Services;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MovieApp.ViewModels
{
    public class MovieListPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMovieService _movieService;
        private int _page = 1;

        public bool IsLoading { get; set; }

        private ObservableCollection<Movie> _movies;
        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set { SetProperty(ref _movies, value); }
        }

        private string _currentSearch;

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public ICommand NavigateToDetailsCommand => new Command<Movie>(async (Movie movie) => await NavigateToDetailsCommandExecute(movie));
        public ICommand LoadMoreMoviesCommand => new Command(async () => await ExecuteLoadMoreMoviesCommand());
        public ICommand ReloadMoviesCommand => new Command(async () => await ExecuteReloadMoviesCommand());
        public ICommand SearchMoviesCommand => new Command(async () => await ExecuteSearchMoviesCommand());
        public ICommand ReloadCommand => new Command(async () => await ReloadCommandExecute());

        public MovieListPageViewModel(IMovieService movieService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _movieService = movieService;
            Title = "Browse Movies";

            LoadInitialData();
        }

        public async void LoadInitialData()
        {
            IsBusy = true;
            var result = await _movieService.GetUpcomingMovies(_page);
            if (result.IsSuccess)
            {
                Movies = new ObservableCollection<Movie>(result.Value);
            }
            IsBusy = false;
        }

        #region Command Implementations

        private async Task ExecuteLoadMoreMoviesCommand()
        {
            if (IsBusy)
                return;

            _page++;

            IsBusy = true;
            var result = await GetMovieResults();
            if (result.IsSuccess)
            {
                var movies = result.Value;
                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }
                IsBusy = false;
            }
        }

        private async Task ExecuteReloadMoviesCommand()
        {
            if (IsBusy)
                return;

            _page = 1;

            IsBusy = true;
            var result = await GetMovieResults();
            if (result.IsSuccess)
            {
                Movies = new ObservableCollection<Movie>(result.Value);
            }
            IsBusy = false;
        }

        private async Task ExecuteSearchMoviesCommand()
        {
            _page = 1;
            _currentSearch = SearchText;

            IsBusy = true;
            var result = await GetMovieResults();
            if (result.IsSuccess)
            {
                Movies = new ObservableCollection<Movie>(result.Value);
            }
            IsBusy = false;
        }

        private async Task ReloadCommandExecute()
        {
            if (IsBusy)
                return;

            _page = 1;
            SearchText = string.Empty;
            _currentSearch = string.Empty;

            IsBusy = true;
            var result = await GetMovieResults();
            if (result.IsSuccess)
            {
                Movies = new ObservableCollection<Movie>(result.Value);
            }
            IsBusy = false;
        }

        #endregion

        #region Private Utils

        private async Task<Result<Movie[]>> GetMovieResults()
        {
            return string.IsNullOrEmpty(_currentSearch)
                ? await _movieService.GetUpcomingMovies(_page)
                : await _movieService.SearchMovies(SearchText, _page);
        }

        private async Task NavigateToDetailsCommandExecute(Movie movie)
        {
            var parameters = new NavigationParameters();
            parameters.Add("movie", movie);
            await _navigationService.NavigateAsync("MovieDetailPage", parameters);
        }

        #endregion
    }
}