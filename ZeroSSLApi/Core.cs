using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZeroSSLApi.Objets.Error;

namespace ZeroSSLApi
{
    public class Core
    {
        private const string ApiServer = "https://api.zerossl.com";

        public static async Task<string> SendGetRequest(string url)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("GET"), $"{ApiServer}{url}"))
                {
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            // Get Error
            JObject result = JObject.Parse(json);
            Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();
            if (string.IsNullOrWhiteSpace(error.Type) == false)
            {
                // certificate_not_found
                throw new Exception($"{error.Code} - {error.Type}");
            }

            // Free
            return json;
        }

        public static async Task<string> SendPostRequest(string url, string content)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod("POST"), $"{ApiServer}{url}"))
                {
                    httpRequestMessage.Content = new StringContent(content);
                    httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            // Get Error
            JObject result = JObject.Parse(json);
            Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();
            if (string.IsNullOrWhiteSpace(error.Type) == false)
            {
                throw new Exception($"{error.Code} - {error.Type}");
            }

            // Free
            return json;
        }

        public static async Task SendDeleteRequest(string token, string url)
        {
            HttpResponseMessage httpResponseMessage;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"{ApiServer}{url}"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
                    httpResponseMessage = await httpClient.SendAsync(request);
                }
            }

            // Response
            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;

                default:
                    JObject result = JObject.Parse(json);
                    //Error error = JsonConvert.DeserializeObject<Error>($"{result["error"]}") ?? new Error();
                    throw new Exception("{error.Code} - {error.Message}");
            }
        }
    }
}