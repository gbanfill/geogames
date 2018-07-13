using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoGames.Maps
{
    public class ColouredMapPin : Pin
    {
        public ColouredMapPin()
        {
        }

        public string Colour { get; set; }

        public static readonly BindableProperty ColourProperty =
               BindableProperty.Create(
                nameof(Colour),
               typeof(string),
                typeof(CustomBackActionPage),
               "#ff0000");

    }
}
