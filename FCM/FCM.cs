using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FCM {
    public interface IFCM {
        void SetApiKey(string apikey);
        FCM.Response Send(object payload);
    }
    public class FCM : IFCM {
        private string Endpoint { get; set; }
        public string ApiKey { get; set; }
        public FCM() {
            Endpoint = "https://fcm.googleapis.com/fcm/send";
        }
        public class Request {
            [JsonPropertyName("to")]
            public string To { get; set; }
            [JsonPropertyName("notification")]
            public PayloadNotification Notification { get; set; }
            [JsonPropertyName("data")]
            public PayloadData Data { get; set; }
            public class PayloadNotification {
                [JsonPropertyName("title")]
                public string Title { get; set; }
                [JsonPropertyName("body")]
                public string Body { get; set; }
                [JsonPropertyName("icon")]
                public string Icon { get; set; }
                [JsonPropertyName("click_action")]
                public string ClickAction { get; set; }
            }
            public class PayloadData {
                [JsonPropertyName("url")]
                public string Url { get; set; }
            }
        }
        public class Response {
            [JsonPropertyName("multicast_id")]
            public long MulticastId { get; set; }
            [JsonPropertyName("canonical_ids")]
            public long CanonicalIds { get; set; }
            public int Success { get; set; }
            public int Failure { get; set; }
            public List<Result> Results { get; set; }
            public class Result {
                [JsonPropertyName("message_id")]
                public long MessageId { get; set; }
                [JsonPropertyName("registration_id")]
                public string RegistrationId { get; set; }
                public string Error { get; set; }
            }
        }
        public void SetApiKey(string apikey) {
            ApiKey = apikey;
        }
        public Response Send(object data) {
            var http = new HttpClient() { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("key", ApiKey) } };
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) { Content = new StringContent(JsonString(data), Encoding.UTF8, MediaTypeNames.Application.Json) };
            var response = http.Send(request);
            var stream = response.Content.ReadAsStream();
            if (response.IsSuccessStatusCode) {
                return JsonSerializer.Deserialize<Response>(stream);
            } else {
                using (var reader = new StreamReader(stream, Encoding.UTF8)) {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            return null;
        }
        public static string JsonString<T>(T data) where T : class {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true });
        }
    }
}