﻿//using System;
//using Android.App;
//using Android.Gms.Gcm;
//using Android.Util;
//using Android.Content;
//using Android.OS;
//using System.Collections.Generic;
//using Realtime.Messaging.Helpers;


//namespace Realtime.Messaging.Droid
//{
//	[Service (Exported = false), IntentFilter (new [] { "com.google.android.c2dm.intent.RECEIVE" })]
//	public class MyGcmListenerService : GcmListenerService
//	{
//		public override void OnMessageReceived (string from, Bundle data)
//		{
//			var message = data.GetString ("message");

//			Log.Debug ("MyGcmListenerService", "From:    " + from);
//			Log.Debug ("MyGcmListenerService", "Message: " + message);
		
//			if (!PushNotificationAppStarter.AppIsInForeground) {
//				SendNotification (message, data);
//				//foreground
//			} else if(CrossPushNotification.PushNotificationListener!= null){
//				var parameters = new Dictionary<string, object>();

//				foreach (var key in data.KeySet())
//				{
//					parameters.Add (key, data.Get (key));
//				}

//				CrossPushNotification.PushNotificationListener.OnMessage (parameters, DeviceType.Android);
//			}
//		}

//		void SendNotification (string message, Bundle pushData)
//		{
//			var context = Android.App.Application.Context;
//			var intent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
//			intent.AddFlags (ActivityFlags.ClearTop);
//			intent.PutExtra ("pushBundle", pushData);

//			var pendingIntent = PendingIntent.GetActivity (this, 0, intent, PendingIntentFlags.OneShot);

//			var notificationBuilder = new Notification.Builder(this)
//				.SetSmallIcon (context.ApplicationInfo.Icon)
//				.SetContentTitle(context.ApplicationInfo.NonLocalizedLabel)
//				.SetContentText (message)
//				.SetAutoCancel (true)
//				.SetContentIntent (pendingIntent);

//			var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
//			notificationManager.Notify (0, notificationBuilder.Build());
//		}
//	}
//}

