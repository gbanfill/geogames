using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoGames.ViewModel;
using Xamarin.Forms;

namespace GeoGames
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
			BindingContext = ViewModelLocator.MainViewModel;
           
        }

		private async void Tracker_OnClicked(object sender, EventArgs e) {
			if (ViewModelLocator.TrackerViewModel == null)
			{
			    ViewModelLocator.TrackerViewModel = new TrackerViewModel();
			}
			await Navigation.PushAsync(new TrackerSetup());  
        }  

		private async void Fugitive_OnClicked(object sender, EventArgs e) {
			if (ViewModelLocator.FugitiveViewModel == null)
			{
				ViewModelLocator.FugitiveViewModel = new FugitiveViewModel();
			}
			await Navigation.PushAsync(new FugitivePage());  
        }  
    }
}
