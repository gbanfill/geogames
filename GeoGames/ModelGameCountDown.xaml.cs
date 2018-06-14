using System;
using System.Collections.Generic;
using GeoGames.ViewModel;
using Xamarin.Forms;

namespace GeoGames
{
    public partial class ModelGameCountDown : ContentPage
    {
        public ModelGameCountDown()
        {
            InitializeComponent();
			BindingContext = ViewModelLocator.GameStartingViewModel;
			ViewModelLocator.GameStartingViewModel.Navigation = Navigation;

        }
    }
}
