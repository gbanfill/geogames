using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

            this.Appearing += Handle_Appearing;
            this.Disappearing += Handle_Disappearing;
            cancellation = new CancellationToken();
        }

        CancellationToken cancellation;

        async void Handle_Appearing(object sender, EventArgs e)
        {
            await RotateElement(loadingImage, cancellation);
        }

        void Handle_Disappearing(object sender, EventArgs e)
        {
            
        }


        private async Task RotateElement(VisualElement element, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await element.RotateTo(360, 800, Easing.Linear);
                await element.RotateTo(0, 0); // reset to initial position
            }
        }
    }
}
