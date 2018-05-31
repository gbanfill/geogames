using System;
using System.Globalization;
using GeoGames.ViewModel;
using Xamarin.Forms;

namespace GeoGames.Convertors
{
	public class FugitiveToMapPinConvertor : IValueConverter
    {
        public FugitiveToMapPinConvertor()
        {
        }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var fugitive = value as Fugitive;
			if (fugitive != null)
			{
				return fugitive.Position;
			}else{
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
