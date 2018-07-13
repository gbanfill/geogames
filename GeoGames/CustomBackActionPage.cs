using System;
using Xamarin.Forms;

namespace GeoGames
{
    public class CustomBackActionPage : ContentPage
    {
        public CustomBackActionPage()
        {
        }


        //  https://theconfuzedsourcecode.wordpress.com/2017/03/12/lets-override-navigation-bar-back-button-click-in-xamarin-forms/

        /// <summary>
        /// Gets or Sets the Back button click overriden custom action
        /// </summary>
        public Action CustomBackButtonAction { get; set; }

        public static readonly BindableProperty EnableBackButtonOverrideProperty =
               BindableProperty.Create(
               nameof(EnableBackButtonOverride),
               typeof(bool),
                typeof(CustomBackActionPage),
               false);

        /// <summary>
        /// Gets or Sets Custom Back button overriding state
        /// </summary>
        public bool EnableBackButtonOverride
        {
            get
            {
                return (bool)GetValue(EnableBackButtonOverrideProperty);
            }
            set
            {
                SetValue(EnableBackButtonOverrideProperty, value);
            }
        }
    }
}
