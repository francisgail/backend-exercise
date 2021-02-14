using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using backend_exercise.Models;

namespace backend_exercise.Services
{

    /// <summary>
    /// Base class for calling Http SuperDraft micro services.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class SDHttpServiceClientBase
    {
        private readonly HttpClient _httpClient;

        private readonly string _superDraftEndPointApiUrl;

        private readonly string _superDraftEndPointApiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SDHttpServiceClientBase" /> class.
        /// </summary>
        protected SDHttpServiceClientBase()
        {
            var retryHandler = new Microsoft.Rest.RetryDelegatingHandler();
            retryHandler.RetryPolicy.Retrying += RetryPolicy_Retrying;
            retryHandler.InnerHandler = new HttpClientHandler();

            _httpClient = new HttpClient(retryHandler);

            _superDraftEndPointApiUrl = ConfigurationManager.AppSettings["SuperDraftEndPointApiUrl"];
            _superDraftEndPointApiKey = ConfigurationManager.AppSettings["SuperDraftEndPointApiKey"];
        }

        private void RetryPolicy_Retrying(object sender, Microsoft.Rest.TransientFaultHandling.RetryingEventArgs retryingEventArgs)
        {
            //log event here
        }

        /// <summary>
        /// Calls the endpoint.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="verb">The verb.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        protected async Task<TResponse> CallEndpoint<TRequest, TResponse>(HttpMethod verb, string resource, TRequest content)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!_httpClient.DefaultRequestHeaders.Contains("SD-api-key"))
            {
                _httpClient.DefaultRequestHeaders.Add("SD-api-key", _superDraftEndPointApiKey);
            }

            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            var endpoint = $"{_superDraftEndPointApiUrl}/{resource}";

            var request = new HttpRequestMessage(verb, endpoint)
            {
                Content = verb != HttpMethod.Get ? new ObjectContent<TRequest>(content, formatter) : null
            };

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            string json = null;
            if (response.Content != null)
            {
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(json);
            }

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(json);

            if (errorResponse != null)
            {
                throw new Exception($"Error calling {endpoint}: \r\nError: {string.Join(",", errorResponse.Errors)}");
            }

            return default;
        }
    }
}

