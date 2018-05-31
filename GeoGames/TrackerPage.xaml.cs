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
         
			var position = await CrossGeolocator.Current.GetPositionAsync();
          
			MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.1)));
		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
  
	}
}
