using System;
using System.Collections.Generic;
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
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			var position = await CrossGeolocator.Current.GetPositionAsync();
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.1)));

		}

		private void fugitivefrequencySlider_ValueChanged(object sender, EventArgs e)
		{
			fugitiveFrequencyLabel.Text = fugitiveFrequencySlider.Value.ToString("##");

		}

		private void trackerfrequencySlider_ValueChanged(object sender, EventArgs e)
		{
			trackerFrequencyLabel.Text = trackerFrequencySlider.Value.ToString("##");
		}
        
		private async void InviteFugitives_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new InviteFugitivesPage());  
		}
    }
}
