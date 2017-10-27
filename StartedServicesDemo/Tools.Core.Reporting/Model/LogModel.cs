using System;

namespace Tools.Core.Reporting
{
    public class LogModel
    {
		#region Metadata

		public string DeviceID { get; set; }

		public string DeviceType { get; set; }

		public string DeviceName { get; set; }

		public string User { get; set; }

		public string AppVersion { get; set; }

		public string OSVersion { get; set; }

		public string AppName { get; set; }

		public string OSName { get; set; }

		public DateTime Time { get; set; }

		#endregion

		#region Guid

		public string Guid { get; set; }

		#endregion

		#region Data members

		public string Module { get; set; }

		public string LogLevel { get; set; }

		public string Message { get; set; }

		public string Data { get; set; }

		public string Data2 { get; set; }

		public string Data3 { get; set; }

		public bool IsComplex { get; set; }

		public string SystemType { get; set; }

		#endregion
	}
}