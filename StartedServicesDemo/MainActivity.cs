using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Util;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using System;
using System.Collections.Generic;

namespace StartedServicesDemo
{
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		static readonly string TAG = typeof(MainActivity).FullName;
		static readonly string SERVICE_STARTED_KEY = "has_service_been_started";

		Button stopServiceButton;
		Button startServiceButton;
		Intent serviceToStart;
		bool isStarted = false;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Main);

			if (savedInstanceState != null)
			{
				isStarted = savedInstanceState.GetBoolean(SERVICE_STARTED_KEY, false);
			}

			serviceToStart = new Intent(this, typeof(TimestampService));

			stopServiceButton = FindViewById<Button>(Resource.Id.stop_timestamp_service_button);
			startServiceButton = FindViewById<Button>(Resource.Id.start_timestamp_service_button);

			if (isStarted)
			{
				stopServiceButton.Click += StopServiceButton_Click;
				stopServiceButton.Enabled = true;
				startServiceButton.Enabled = false;
			}
			else 
			{
				startServiceButton.Click += StartServiceButton_Click;
				startServiceButton.Enabled = true;
				stopServiceButton.Enabled = false;
			}

            MobileCenter.Start("1db7a10f-f361-4114-b85c-a16832cae8be", typeof(Analytics), typeof(Crashes));
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			outState.PutBoolean(SERVICE_STARTED_KEY, isStarted);
			base.OnSaveInstanceState(outState);
		}

		protected override void OnDestroy()
		{
			Log.Info(TAG, "Activity is being destroyed; stop the service.");

			//StopService(serviceToStart);
			base.OnDestroy();
		}
		void StopServiceButton_Click(object sender, System.EventArgs e)
		{
			Analytics.TrackEvent("Stop Service clicked", new Dictionary<string, string> {
	{ "Action", "Click" },
	{ "Type", "Stop Service"}
});
            Crashes.GenerateTestCrash();
			//stopServiceButton.Click -= StopServiceButton_Click;
			//stopServiceButton.Enabled = false;

			//Log.Info(TAG, "User requested that the service be stopped.");
			//StopService(serviceToStart);
			//isStarted = false;

			//startServiceButton.Click += StartServiceButton_Click;
			//startServiceButton.Enabled = true;
		}

		void StartServiceButton_Click(object sender, System.EventArgs e)
		{
			Analytics.TrackEvent("Start Service clicked", new Dictionary<string, string> {
	{ "Action", "Click" },
	{ "Type", "Start Service"}
});

			//startServiceButton.Enabled = false;
			//startServiceButton.Click -= StartServiceButton_Click;

			//StartService(serviceToStart);
			//Log.Info(TAG, "User requested that the service be started.");

			//isStarted = true;
			//stopServiceButton.Click += StopServiceButton_Click;

			stopServiceButton.Enabled = true;
		}
	}
}

