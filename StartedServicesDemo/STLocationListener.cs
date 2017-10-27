using System;
using Android.OS;
using Android.Locations;
using Android.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace StartedServicesDemo
{
    public class STLocationListener : ILocationListener
    {
		Location currentLocation;
		LocationManager locationManager;
		string locationProvider;

        public STLocationListener()
        {
        }

        public IntPtr Handle => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

		//private void InitializeLocationManager()
		//{
		//	locationManager = (LocationManager)GetSystemService(LocationService);
		//	Criteria criteriaForLocationService = new Criteria
		//	{
		//		Accuracy = Accuracy.Fine
		//	};
		//	IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
		//	if (acceptableLocationProviders.Any())
		//	{
		//		locationProvider = acceptableLocationProviders.First();
		//	}
		//	else
		//	{
		//		locationProvider = string.Empty;
		//	}
		//	//Log.Debug(TAG, "Using " + locationProvider + ".");
		//}
    }
}
