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
    [Activity(Label = "Fugitive", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            Android.Support.V7.Widget.Toolbar toolbar
              = this.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // check if the current item id 
            // is equals to the back button id
            if (item.ItemId == 16908332)
            {
                // retrieve the current xamarin forms page instance
                var currentpage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() as CustomBackActionPage;

                // check if the page has subscribed to 
                // the custom back button event
                if (currentpage?.CustomBackButtonAction != null)
                {
                    // invoke the Custom back button action
                    currentpage?.CustomBackButtonAction.Invoke();
                    // and disable the default back button action
                    return false;
                }

                // if its not subscribed then go ahead 
                // with the default back button action
                return base.OnOptionsItemSelected(item);
            }
            else
            {
                // since its not the back button 
                //click, pass the event to the base
                return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            // this is not necessary, but in Android user 
            // has both Nav bar back button and
            // physical back button its safe 
            // to cover the both events

            // retrieve the current xamarin forms page instance
            var currentpage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() as CustomBackActionPage;


            // check if the page has subscribed to 
            // the custom back button event
            if (currentpage?.CustomBackButtonAction != null)
            {
                currentpage?.CustomBackButtonAction.Invoke();
            }
            else
            {
                base.OnBackPressed();
            }
        }



    }
}

