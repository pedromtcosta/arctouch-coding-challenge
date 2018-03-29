using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MovieApp.Models;
using MovieApp.ViewModels;

namespace MovieApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MovieDetailPage : ContentPage
	{
        public MovieDetailPage()
        {
            InitializeComponent();
        }
    }
}