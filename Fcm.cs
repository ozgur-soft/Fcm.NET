using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fcm {
    public interface IFcm {
        void SetApiKey(string apikey);
        Fcm.Response Send(Fcm.Request data);
    }
    public class Fcm : IFcm {
        private string Endpoint { get; set; }
        public string ApiKey { get; set; }
        public Fcm() {
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
        public Response Send(Request data) {
            using var http = new HttpClient() { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("key", ApiKey) } };
            using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) { Content = new StringContent(JsonString(data), Encoding.UTF8, MediaTypeNames.Application.Json) };
            using var response = http.Send(request);
            var result = JsonSerializer.Deserialize<Response>(response.Content.ReadAsStream());
            return result;
        }
        public static string JsonString<T>(T data) where T : class {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true });
        }
    }
}