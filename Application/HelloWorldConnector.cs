using Microsoft.Extensions.Logging;
using SalesforceExternalClientAppDemo.ConsoleApp.Exceptions;
using SalesforceExternalClientAppDemo.ConsoleApp.Settings;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Application
{
    public class HelloWorldConnector : IHelloWorldConnector
    {
        private readonly ILogger<HelloWorldConnector> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly SalesforceSettings salesforceSettings;

        public HelloWorldConnector(ILogger<HelloWorldConnector> logger, IHttpClientFactory httpClientFactory,
            SalesforceSettings salesforceSettings)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.salesforceSettings = salesforceSettings;
        }

        public async Task<string> GetMessage()
        {
            var requestUri = $"{salesforceSettings.ApiBaseUrl}v1/hello-world/";
            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", salesforceSettings.AccessToken);

            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error from HelloWorld API: {@response}", response);
                var content = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                var reasonPhrase = response.ReasonPhrase;
                throw new ApiException($"Status Code: {statusCode}, Reason: {reasonPhrase}, Content: {content}");
            }

            logger.LogInformation("Got Message from HelloWorld API", response.Content.ReadAsStringAsync().Result);
            return await response.Content.ReadAsStringAsync();
        }
    }
}