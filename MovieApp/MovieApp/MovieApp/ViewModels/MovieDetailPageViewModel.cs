using System;

using MovieApp.Models;
using Prism.Navigation;

namespace MovieApp.ViewModels
{
    public class MovieDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private Movie _movie;
        public Movie Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Movie = (Movie)parameters["movie"];
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
