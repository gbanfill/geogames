using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GeoGames.Messaging;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using GeoGames.ViewModel;
using System.Threading.Tasks;

namespace GeoGames
{
    public partial class TrackerPage : ContentPage
    {
        public TrackerPage()
        {
            InitializeComponent();
			BindingContext = ViewModelLocator.TrackerViewModel;
        }
            
		protected override async void OnAppearing()
		{
			base.OnAppearing();
         
			var position = await CrossGeolocator.Current.GetLastKnownLocationAsync();
			ViewModelLocator.TrackerViewModel.Position = position;
			await StartListeningToLocation();
			MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.1)));
		}

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
			await StopListeningForLocation();
        }
  
		async Task StartListeningToLocation()
        {
            if (CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {         
			ViewModelLocator.TrackerViewModel.Position = e.Position;          
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            //Handle event here for errors
        }

        async Task StopListeningForLocation()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
	}
}
