using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;

namespace StartedServicesDemo
{

	public class UtcTimestamper
	{
		DateTime startTime;

		public UtcTimestamper()
		{
			startTime = DateTime.UtcNow;
		}

		public string GetFormattedTimestamp()
		{
			TimeSpan duration = DateTime.UtcNow.Subtract(startTime);
            return $"Started at {startTime.Hour}:{startTime.Minute}:{startTime.Second} ({duration.Minutes}Mins:{duration.Seconds}Ses ago).";
		}

	}
	
}
