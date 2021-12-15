using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GoogleBooks.Service.Http
{
    sealed class HttpService
    {
        private static readonly Lazy<HttpService> _Lazy = new Lazy<HttpService>(() => new HttpService());
        public static HttpService Current { get => _Lazy.Value; }

        private HttpService()
        {
            Client = new HttpClient();
            _JsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                IgnoreReadOnlyProperties = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public HttpClient Client { get; }
        private readonly JsonSerializerOptions _JsonSerializerOptions;

        public async Task<T> Get<T>(
            string url,
            string accessToken = null)
        {
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: url);

            if (!string.IsNullOrWhiteSpace(accessToken))
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: accessToken);

            using HttpResponseMessage httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
                await ExceptionFromHttpStatusCode(httpResponseMessage);

            if (string.IsNullOrWhiteSpace(responseContent))
                return default;

            return JsonSerializer.Deserialize<T>(responseContent, _JsonSerializerOptions);
        }

        public async Task PostJson(
            string url,
            object args,
            string accessToken = null)
        {
            var content = JsonSerializer.Serialize(args, _JsonSerializerOptions);
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                method: HttpMethod.Post,
                requestUri: url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrWhiteSpace(accessToken))
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: accessToken);

            using HttpResponseMessage httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                await ExceptionFromHttpStatusCode(httpResponseMessage);
        }

        public Task<T> PostJson<T>(string url)
            => PostJson<T>(url: url, args: null, accessToken: null);

        public Task<T> PostJson<T>(string url, object args = null)
            => PostJson<T>(url: url, args: args, accessToken: null);

        public async Task<T> PostJson<T>(
            string url,
            object args = null,
            string accessToken = null)
        {
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                method: HttpMethod.Post,
                requestUri: url);

            if (!(args is null))
            {
                var content = JsonSerializer.Serialize(args, _JsonSerializerOptions);
                httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: accessToken);

            using HttpResponseMessage httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                await ExceptionFromHttpStatusCode(httpResponseMessage);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseContent))
                return default;

            return JsonSerializer.Deserialize<T>(responseContent, _JsonSerializerOptions);
        }

        public async Task<T> PostJson<T>(
            string url,
            object args = null,
            Action<HttpRequestMessage> beforeSend = null)
        {
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                method: HttpMethod.Post,
                requestUri: url);

            if (!(args is null))
            {
                var content = JsonSerializer.Serialize(args, _JsonSerializerOptions);
                httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            beforeSend?.Invoke(httpRequestMessage);

            using HttpResponseMessage httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                await ExceptionFromHttpStatusCode(httpResponseMessage);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseContent))
                return default;

            return JsonSerializer.Deserialize<T>(responseContent, _JsonSerializerOptions);
        }

        public async Task PutJson(
            string url,
            object args,
            string accessToken = null)
        {
            var content = JsonSerializer.Serialize(args, _JsonSerializerOptions);
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                method: HttpMethod.Put,
                requestUri: url)
            {
                Content = new StringContent(
                    content: content,
                    encoding: Encoding.UTF8,
                    mediaType: "application/json")
            };

            if (!string.IsNullOrWhiteSpace(accessToken))
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: accessToken);

            using HttpResponseMessage httpResponseMessage = await Client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                await ExceptionFromHttpStatusCode(httpResponseMessage);
        }

        private async Task ExceptionFromHttpStatusCode(HttpResponseMessage httpResponseMessage)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            throw httpResponseMessage.StatusCode switch
            {
                HttpStatusCode.BadRequest => new InvalidOperationException(message: GetMessageFromError(content)),
                HttpStatusCode.Unauthorized => new UnauthorizedAccessException(),
                _ => new InvalidOperationException("Erro desconhecido ao realizar essa operação"),
            };
        }

        class ErrorByPass
        {
            [JsonProperty("authenticated")]
            public bool Ok { get; set; }

            public string message { get; set; }
        }

        private string GetMessageFromError(string content)
        {
            var payload = JsonConvert.DeserializeObject<ErrorByPass>(content);

            if (payload is null)
                return "Erro desconhecido ao realizar essa requisição";

            return payload.message;
        }
    }
}
