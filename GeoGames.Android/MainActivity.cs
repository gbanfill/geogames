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
using System.Linq;

namespace GeoGames.Droid
{
    [Activity(Label = "GeoGames", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                  Categories = new[] { Android.Content.Intent.CategoryBrowsable, Android.Content.Intent.CategoryDefault },
                  DataScheme = DeepLinkingConstants.DataScheme,
                  DataHost = DeepLinkingConstants.DataHost,
                  DataPathPrefix = DeepLinkingConstants.DataPathPrefix,
              AutoVerify = true)]
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

            var app = new App();
            LoadApplication(app);

			CrossCurrentActivity.Current.Activity = this;

            var uri = this.Intent.Data;
            if (uri != null)
            {
                String path = uri.Path;
                var gameId = path.Split('/').Last();
                app.StraightToFugitiveForGameId(gameId);
            }
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }
}

