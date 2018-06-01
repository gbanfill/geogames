using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GeoGames
{
    public partial class CaughtPage : ContentPage
    {
        public CaughtPage()
        {
            InitializeComponent();
        }

		public async void Dismiss_Clicked(object sender, EventArgs eventArgs)
		{
			await Navigation.PopModalAsync();
		}
    }
}
