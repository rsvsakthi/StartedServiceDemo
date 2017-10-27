using System;
using System.Collections.Generic;

namespace Tools.Core.Network
{
    public class SubmissionObject
    {
		public Guid CorrelationId { get; set; }
		public string DeviceUTCDateTime { get; set; }
		public string UserName { get; set; }
		public string DeviceName { get; set; }

		public string Type { get; set; }
		public Dictionary<string, string> Parameters { get; set; }
		public string InformationObj { get; set; }

		public SubmissionObject()
		{

		}
    }
}