using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using System.Threading;
using Tools.Core.Reporting;
using Tools.Core;
using Tools.Core.Network;
using Android.Locations;
using Android.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace StartedServicesDemo
{
	/// <summary>
	/// This is a sample started service. When the service is started, it will log a string that details how long 
	/// the service has been running (using Android.Util.Log). This service displays a notification in the notification
	/// tray while the service is active.
	/// </summary>
	[Service]
	public class TimestampService : Service, ILocationListener
	{
		static readonly string TAG = typeof(TimestampService).FullName;
		static readonly int DELAY_BETWEEN_LOG_MESSAGES = 10000; // milliseconds
		static readonly int NOTIFICATION_ID = 10000;
        private const string SourceLM = "LogManager";

		UtcTimestamper timestamper;
		bool isStarted;
		Handler handler;
		Action runnable;

		Location currentLocation;
		LocationManager locationManager;
		string locationProvider;

		private NetworkManager _networkService;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "OnCreate: the service is initializing.");

            timestamper = new UtcTimestamper();
            handler = new Handler();

            // This Action is only for demonstration purposes.
            runnable = new Action(() =>
                            {
                                if (timestamper != null)
                                {
                                    Log.Debug(TAG, timestamper.GetFormattedTimestamp());
                                    CancelNotification();
                                    DispatchNotificationThatServiceIsRunning();
                                    //locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
                                    LogLocation();
                                    handler.PostDelayed(runnable, DELAY_BETWEEN_LOG_MESSAGES);

                                }
                            });
        }

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			if (isStarted)
			{
				Log.Info(TAG, "OnStartCommand: This service has already been started.");
			}
			else
			{
				Log.Info(TAG, "OnStartCommand: The service is starting.");
                InitializeLocationManager();
				DispatchNotificationThatServiceIsRunning();
				handler.PostDelayed(runnable, DELAY_BETWEEN_LOG_MESSAGES);
				isStarted = true;
			}

			// This tells Android not to restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
		}


		public override IBinder OnBind(Intent intent)
		{
			// Return null because this is a pure started service. A hybrid service would return a binder that would
			// allow access to the GetFormattedStamp() method.
			return null;
		}


		public override void OnDestroy()
        {
            // We need to shut things down.
            Log.Debug(TAG, GetFormattedTimestamp());
            Log.Info(TAG, "OnDestroy: The started service is shutting down.");

            // Stop the handler.
            handler.RemoveCallbacks(runnable);

            CancelNotification();

            timestamper = null;
            isStarted = false;
            base.OnDestroy();
        }

        private void CancelNotification()
        {
            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(NOTIFICATION_ID);
        }

        /// <summary>
        /// This method will return a formatted timestamp to the client.
        /// </summary>
        /// <returns>A string that details what time the service started and how long it has been running.</returns>
        string GetFormattedTimestamp()
		{
			return timestamper?.GetFormattedTimestamp();
		}

		void DispatchNotificationThatServiceIsRunning()
		{
			Notification.Builder notificationBuilder = new Notification.Builder(this)
				.SetSmallIcon(Resource.Drawable.ic_stat_name)
				.SetContentTitle(Resources.GetString(Resource.String.app_name))
                .SetContentText(GetFormattedTimestamp());

			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());
		}

        void LogLocation()
        {
            LogModel logModel = CreateLog("MainActivity", "Information");
            logModel.Data = "Bangalore, India";

			if (_networkService == null)
			{
				_networkService = new NetworkManager();

			}
            _networkService.SubmitResultRequest(new System.Collections.Generic.Dictionary<string, string>(), logModel);
        }

		/// <summary>
		//  Creates new log with current time
		/// </summary>
		/// <returns>The log.</returns>
		private LogModel CreateLog(string module, string level)
		{
			LogModel data = new LogModel();

			data.Guid = Guid.NewGuid().ToString();
			data.Time = System.DateTime.Now;

			data.User = "";
            data.DeviceID = GetDeviceId();
            data.DeviceType = Android.OS.Build.Model;
            data.DeviceName = GetDevicename();
			data.AppVersion = "1.0";
			data.OSVersion = "1.0";
			data.OSName = "abc";
			data.AppName = "SimplerTools Service";
            data.Module = "Location Service";
            data.LogLevel = level;
            data.Message = "Location";

			data.Data2 = SourceLM;
			data.Data3 = "NA";

			return data;
		}

		private string GetDeviceId()
		{
			string deviceId = string.Empty;

			try
			{
				deviceId = Build.Id + "-" + Build.Serial;
			}
			catch (Exception)
			{
				deviceId = string.Empty;
			}

			return deviceId;
		}

		private string GetDevicename()
		{
			string deviceName = string.Empty;

			try
			{
				Java.Lang.Class commClass = Java.Lang.Class.FromType(typeof(Android.OS.Build));
				Java.Lang.Class[] param = new Java.Lang.Class[1];
				param[0] = Java.Lang.Class.FromType(typeof(Java.Lang.String));
				Java.Lang.Reflect.Method getStringMethod = commClass.GetDeclaredMethod("getString", param);
				getStringMethod.Accessible = true;
				deviceName = getStringMethod.Invoke(null, "net.hostname").ToString();
			}
			catch (Exception)
			{
				deviceName = string.Empty;
			}

			return deviceName;

		}

        public void OnLocationChanged(Location location)
        {
			currentLocation = location;
			if (currentLocation == null)
			{
				//Error Message  
			}
			else
			{
				var latitude = currentLocation.Latitude.ToString();
				var longitude = currentLocation.Longitude.ToString();
			}
		}

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

		private void InitializeLocationManager()
		{
			locationManager = (LocationManager)GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
                Accuracy = Accuracy.Coarse
			};
			IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
			if (acceptableLocationProviders.Any())
			{
				locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				locationProvider = string.Empty;
			}
			Log.Debug(TAG, "Using " + locationProvider + ".");
		}
	}
}
