using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoGames.Maps
{
    public class BindableMap : Map
    {
        // class taken from https://xamarinhelp.com/xamarin-forms-maps/

        public static readonly BindableProperty MapPinsProperty = BindableProperty.Create(
                 nameof(Pins),
            typeof(ObservableCollection<ColouredMapPin>),
                 typeof(BindableMap),
            new ObservableCollection<ColouredMapPin>(),
                 propertyChanged: (b, o, n) =>
                 {
                     var bindable = (BindableMap)b;
                     bindable.Pins.Clear();

            var collection = (ObservableCollection<ColouredMapPin>)n;
                     foreach (var item in collection)
                         bindable.Pins.Add(item);
                     collection.CollectionChanged += (sender, e) =>
                     {
                         Device.BeginInvokeOnMainThread(() =>
                         {
                             switch (e.Action)
                             {
                                 case NotifyCollectionChangedAction.Add:
                                 case NotifyCollectionChangedAction.Replace:
                                 case NotifyCollectionChangedAction.Remove:
                                     if (e.OldItems != null)
                                         foreach (var item in e.OldItems)
                                             bindable.Pins.Remove((Pin)item);
                                     if (e.NewItems != null)
                                         foreach (var item in e.NewItems)
                                             bindable.Pins.Add((Pin)item);
                                     break;
                                 case NotifyCollectionChangedAction.Reset:
                                     bindable.Pins.Clear();
                                     break;
                             }
                         });
                     };
                 });
        public IList<ColouredMapPin> MapPins { 
            get { return (IList<ColouredMapPin>)GetValue(MapPinsProperty); } 
            set { SetValue(MapPinsProperty, value); }
        }

        public static readonly BindableProperty MapPositionProperty = BindableProperty.Create(
                 nameof(MapPosition),
                 typeof(Position),
                 typeof(BindableMap),
                 new Position(0, 0),
                 propertyChanged: (b, o, n) =>
                 {
                     ((BindableMap)b).MoveToRegion(MapSpan.FromCenterAndRadius(
                          (Position)n,
                          Distance.FromMiles(1)));
                 });

        public Position MapPosition { get; set; }
    }
}
