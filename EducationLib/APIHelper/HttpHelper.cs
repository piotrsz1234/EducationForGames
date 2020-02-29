using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace EducationLib.APIHelper {
	public class HttpHelper {

		protected readonly string APIServerUrl;

		public HttpHelper (string serverUrl) {
			APIServerUrl = serverUrl;
			if (APIServerUrl[APIServerUrl.Length - 1] != '/') APIServerUrl += '/';
		}

		private async Task<string> GetResponseAsync (string url) {
			using (WebClient client = new WebClient ()) {
				return await client.DownloadStringTaskAsync (url);
			}
		}

		public async Task<T> Get<T> (string url) {
			return JsonConvert.DeserializeObject<T> (await GetResponseAsync (CombineUrls (url)));
		}

		public T Post<T> (string url, Dictionary<string, string> values) {
			using (var wb = new WebClient ()) {
				var data = new NameValueCollection ();
				foreach (var item in values) {
					data.Add (item.Key, item.Value);
				}
				var response = wb.UploadValues (CombineUrls(url), "POST", data);
				string responseInString = Encoding.UTF8.GetString (response);
				return JsonConvert.DeserializeObject<T>(responseInString);
			}
		}

		public T Post<T> (string url, object toSend) {
			var temp = JsonConvert.DeserializeObject<Dictionary<string, string>> (JsonConvert.SerializeObject (toSend));
			return Post<T> (url, temp);
		}

		private string CombineUrls (string url) {
			if (url[0] == '/') url = url.Substring (1);
			return (APIServerUrl + url);
		}

	}
}
