using System;
namespace GeoGames.Extensions
{
    public static class PositionExtensions
    {
		public static Plugin.Geolocator.Abstractions.Position ToGeolocatorPosition(this Xamarin.Forms.Maps.Position xamarinFormsPosition)
		{
			return new Plugin.Geolocator.Abstractions.Position(xamarinFormsPosition.Latitude, xamarinFormsPosition.Longitude);
		}
    }
}
