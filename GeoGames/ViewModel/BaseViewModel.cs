using System;
using System.ComponentModel;

namespace GeoGames.ViewModel
{
	public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
        }

		protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
