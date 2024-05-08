using System.Text;
using System.Text.Json;

namespace Sales.WEB.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<T>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<T>(default, true, responseHttp);

        }

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContext = new StringContent(messageJSON, Encoding.UTF8, "aplication/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContext);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContext = new StringContent(messageJSON, Encoding.UTF8, "aplication/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContext);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);

            }
            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T?> UnserializeAnswer<T>(HttpResponseMessage httpRespnse, JsonSerializerOptions jsonSerializerOptions)
        {
            var respuestaString = await httpRespnse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions);
        }

    }
}
