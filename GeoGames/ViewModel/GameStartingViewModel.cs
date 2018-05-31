using System;
using System.Timers;
using GeoGames.Messaging;
using Xamarin.Forms;

namespace GeoGames.ViewModel
{
	public class GameStartingViewModel : BaseViewModel
    {
        public GameStartingViewModel()
        {
        }
  
		private DateTime _startingDateTime;
		public DateTime StartingDateTime { 
			get { return _startingDateTime ; } 
			set { _startingDateTime = value; OnPropertyChanged("StartingDateTime"); }
		}

		private int _secondsToStart;
		public int SecondsToStart { 
			get { return _secondsToStart;  }
			set { _secondsToStart = value; OnPropertyChanged("SecondsToStart"); } }

		public INavigation Navigation { get; internal set; }

		public void StartCountdownTimer()
		{
			Timer t = new Timer(250);
			t.Elapsed += (object sender, ElapsedEventArgs e) => {
				SecondsToStart = (StartingDateTime - DateTime.Now).Seconds;
				if (SecondsToStart < 0)
				{
					t.Stop();
					Device.BeginInvokeOnMainThread(async () => {
					        await Navigation.PopModalAsync();
					});
				}
			};
			t.Start();
		}

		public GameStartsAtMessage ToGameStartsMessage()
		{
			GameStartsAtMessage result = new GameStartsAtMessage()
			{
				GameStartsAtTime = StartingDateTime
			};
			return result;

		}
	}
}
