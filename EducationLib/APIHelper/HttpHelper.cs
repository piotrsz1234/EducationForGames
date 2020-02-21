using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EducationLib.APIHelper {
	public class HttpHelper {

		protected readonly string APIServerUrl;
		protected readonly HttpClient client;

		public HttpHelper(string serverUrl) {
			APIServerUrl = serverUrl;
			client = new HttpClient ();
			if (APIServerUrl[APIServerUrl.Length - 1] != '/') APIServerUrl += '/';
		}

		private async Task<string> GetResponseAsync (string url) {
			using (WebClient client = new WebClient()) {
				return await client.DownloadStringTaskAsync (url);
			}
		}

		public async Task<T> Get<T> (string url) {
			return JsonConvert.DeserializeObject<T> (await GetResponseAsync (CombineUrls (url)));
		}

		public async Task<T> Post<T> (string url, Dictionary<string, string> values) {
			var content = new FormUrlEncodedContent (values);

			var response = await client.PostAsync (CombineUrls(url), content);

			var responseString = await response.Content.ReadAsStringAsync ();
			return JsonConvert.DeserializeObject<T> (responseString);
		}

		public async Task<T> Post<T> (string url, object toSend) {
			var temp = JsonConvert.DeserializeObject<Dictionary<string, string>> (JsonConvert.SerializeObject (toSend));
			return await Post<T> (url, temp);
		}

		private string CombineUrls(string url) {
			if (url[0] == '/') url = url.Substring (1);
			return APIServerUrl + url;
		}

	}
}
