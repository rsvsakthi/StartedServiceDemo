using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Tools.Core.Reporting;

namespace Tools.Core.Network
{
	public class NetworkManager
	{
		private HttpClient _httpClient;

		public NetworkManager()
		{

			{
				_httpClient = new HttpClient();

			}

			_httpClient.Timeout = new System.TimeSpan(0, 5, 0);


		}

		public void SubmitResultRequest(Dictionary<string, string> parameters, LogModel info)
		{

			string jsonData = string.Empty;

			SubmissionObject submission = new SubmissionObject();
            submission.Type = typeof(LogModel).AssemblyQualifiedName;
			submission.Parameters = parameters;

			submission.CorrelationId = Guid.NewGuid();
			submission.DeviceUTCDateTime = DateTime.UtcNow.ToString();
			submission.UserName = "SampleUser";
			submission.DeviceName = "ServiceTest";
			submission.InformationObj = string.Empty;

			try
			{
				submission.InformationObj = JsonConvert.SerializeObject(info);

				string serialized = JsonConvert.SerializeObject(submission);
				var inputMessage = new HttpRequestMessage
				{
					Content = new StringContent(serialized, Encoding.UTF8, "application/json")
				};

				inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage message = _httpClient.PostAsync("http://abc.123.com/SubmitInformation", 
                                                                    inputMessage.Content).Result;

				if (message.IsSuccessStatusCode == true)
				{
					jsonData = message.Content.ReadAsStringAsync().Result;
				}
				else
				{
					throw new Exception("Not a success call");
				}
			}
			catch (Exception)
			{
				//ignore as of now
			}
		}
	}
}

