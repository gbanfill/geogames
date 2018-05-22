using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Mindscape.Raygun4Net;

namespace GeoGames.Droid
{
    [Activity(Label = "GeoGames", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

			RaygunClient.Attach("hE22yVqWFboJm4JvT0c90Q==");

            global::Xamarin.Forms.Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);

            var uri = this.Intent.Data;
            if (uri != null)
            {
                String path = uri.Path;
                // if we have a path launch properly.
            }
            LoadApplication(new App());

			CrossCurrentActivity.Current.Activity = this;
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }
}

