using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MovieApp.Models;
using MovieApp.Views;
using MovieApp.ViewModels;

namespace MovieApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MovieListPage : ContentPage
	{
        public MovieListPage()
        {
            InitializeComponent();
        }
    }
}