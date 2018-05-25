using System;
using System.Collections.Generic;
using GeoGames.ViewModel;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoGames
{
    public partial class TrackerSetup : ContentPage
    {
        public TrackerSetup()
        {
            InitializeComponent();
			BindingContext = ViewModelLocator.TrackerViewModel;
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			var position = await CrossGeolocator.Current.GetPositionAsync();
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.1)));

		}

		private async void InviteFugitives_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new InviteFugitivesPage());  
		}
    }
}
