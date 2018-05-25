using System;
using System.ComponentModel;

namespace GeoGames.ViewModel
{
	public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			Version = assembly.GetName().Version.ToString();

        }

		public string Version { get; set; }


	}
}
