using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fcm {
    public interface IFcm {
        void SetApiKey(string apikey);
        void SetProjectId(string projectid);
        Fcm.Response Send(Fcm.Request data);
    }
    public class Fcm : IFcm {
        public string ApiKey { get; set; }
        public string ProjectId { get; set; }
        public class Request {
            [JsonPropertyName("message")]
            public _Message Message { get; set; }
            public class _Message {
                [JsonPropertyName("token")]
                public string Token { get; set; }
                [JsonPropertyName("topic")]
                public string Topic { get; set; }
                [JsonPropertyName("notification")]
                public _Notification Notification { get; set; }
                [JsonPropertyName("data")]
                public _Data Data { get; set; }
                public class _Notification {
                    [JsonPropertyName("title")]
                    public string Title { get; set; }
                    [JsonPropertyName("body")]
                    public string Body { get; set; }
                    [JsonPropertyName("icon")]
                    public string Icon { get; set; }
                    [JsonPropertyName("click_action")]
                    public string ClickAction { get; set; }
                }
                public class _Data {
                    [JsonPropertyName("url")]
                    public string Url { get; set; }
                }
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
        public void SetProjectId(string projectid) {
            ProjectId = projectid;
        }
        public Response Send(Request data) {
            using var http = new HttpClient() { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", ApiKey) } };
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/v1/projects/" + ProjectId + "/messages:send") { Content = new StringContent(JsonString(data), Encoding.UTF8, MediaTypeNames.Application.Json) };
            using var response = http.Send(request);
            var result = JsonSerializer.Deserialize<Response>(response.Content.ReadAsStream());
            return result;
        }
        public static string JsonString<T>(T data) where T : class {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true });
        }
    }
}