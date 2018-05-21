using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GeoGames
{
    public partial class InviteFugitivesPage : ContentPage
    {
        public InviteFugitivesPage()
        {
            InitializeComponent();
        }

		async void StartGame_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new TrackerPage());  
		}
    }
}
