using System;
using System.Threading.Tasks;
using GeoGames.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GeoGames
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			MainPage = new NavigationPage( new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task StraightToFugitiveForGameId(string gameid)
        {
            if (ViewModelLocator.FugitiveViewModel == null)
            {
                ViewModelLocator.FugitiveViewModel = new FugitiveViewModel();
            }
            ViewModelLocator.FugitiveViewModel.GameId = gameid;
            await MainPage.Navigation.PushAsync(new FugitivePage());  
        }
    }
}
