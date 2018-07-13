using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Widget;
using GeoGames.Droid;
using GeoGames.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(BindableMap), typeof(BindableMapRenderer))]
namespace GeoGames.Droid
{
    public class BindableMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        IList<ColouredMapPin> customPins;

        public BindableMapRenderer(Android.Content.Context context) : base(context)
        {
        
        }

        BindableMap formsMap;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                formsMap = (BindableMap)e.NewElement;
                customPins = formsMap.MapPins;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

       protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            ColouredMapPin customPin = (ColouredMapPin) pin;
            var colour = HexColourtoAndroidColour(customPin.Colour);
            var bmp1 = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.pin);
            var icon = bmp1.Copy(bmp1.GetConfig(),true); 
            Paint paint = new Paint();
            ColorFilter filter = new PorterDuffColorFilter(colour, PorterDuff.Mode.SrcIn);
            paint.SetColorFilter(filter);

            Canvas canvas = new Canvas(icon);
            canvas.DrawBitmap(icon, 0, 0, paint);

            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(icon));
            return marker;
        }

        private static global::Android.Graphics.Color HexColourtoAndroidColour(string p)
        {
            try
            {
                // special case - ParseColor cant handle #000 (it expects #000000)
                if (string.Equals(p, "#000"))
                {
                    p = "#000000";
                }
                return global::Android.Graphics.Color.ParseColor(p);
            }
            catch (Exception ex)
            {
                return global::Android.Graphics.Color.Red;
            }
        }
        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

           
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Android.Content.Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
               
                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
               
                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        ColouredMapPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);           
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

        ColouredMapPin GetCustomPin(LatLng latLng)
        {
            var position = new Position(latLng.Latitude, latLng.Longitude);    
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
