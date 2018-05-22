﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeoGames
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            version.Text = assembly.GetName().Version.ToString();
        }

		private async void Tracker_OnClicked(object sender, EventArgs e) {  
			await Navigation.PushAsync(new TrackerSetup());  
        }  

		private async void Fugitive_OnClicked(object sender, EventArgs e) {  
			await Navigation.PushAsync(new FugitivePage());  
        }  
    }
}
