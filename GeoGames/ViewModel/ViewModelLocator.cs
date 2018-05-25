using System;
namespace GeoGames.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
			MainViewModel = new MainViewModel();
        }

        

		public static MainViewModel MainViewModel { get; set; }

		public static FugitiveViewModel FugitiveViewModel { get; set; }

		public static TrackerViewModel TrackerViewModel { get; set; }
    }
}
