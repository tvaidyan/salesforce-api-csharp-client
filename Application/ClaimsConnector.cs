using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesforceExternalClientAppDemo.ConsoleApp.Exceptions;
using SalesforceExternalClientAppDemo.ConsoleApp.Helpers;
using SalesforceExternalClientAppDemo.ConsoleApp.Models;
using SalesforceExternalClientAppDemo.ConsoleApp.Settings;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Application
{
    public class ClaimsConnector : IClaimsConnector
    {
        private readonly ILogger<ClaimsConnector> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly SalesforceSettings salesforceSettings;
        private readonly string requestUri;
        private readonly HttpClient client;

        public ClaimsConnector(ILogger<ClaimsConnector> logger, IHttpClientFactory httpClientFactory,
            SalesforceSettings salesforceSettings)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.salesforceSettings = salesforceSettings;

            requestUri = $"{salesforceSettings.ApiBaseUrl}v1/claim/";
            client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", salesforceSettings.AccessToken);
        }

        public async Task<ClaimResponse> AddClaim(Claim claim)
        {
            var payload = new JsonContent(claim);
            var response = await client.PostAsync(requestUri, payload);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error from Claim API: {@response}", response);
                var content = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                var reasonPhrase = response.ReasonPhrase;
                throw new ApiException($"Status Code: {statusCode}, Reason: {reasonPhrase}, Content: {content}");
            }

            logger.LogDebug("Done sending {@claim} to Claim API", claim);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClaimResponse>(jsonString);
        }

        public async Task DeleteClaim(string claimNumber)
        {
            await client.DeleteAsync(requestUri + "?claimNumber=" + claimNumber);
        }

        public async Task<List<ClaimResponse>> GetClaims()
        {
            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error from Claim API: {@response}", response);
                var content = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                var reasonPhrase = response.ReasonPhrase;
                throw new ApiException($"Status Code: {statusCode}, Reason: {reasonPhrase}, Content: {content}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ClaimResponse>>(jsonString);
        }

        public async Task<ClaimResponse> UpdateFavoriteColor(Claim claim)
        {
            var payload = new JsonContent(claim);
            var response = await client.PutAsync(requestUri, payload);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error from Claim API: {@response}", response);
                var content = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                var reasonPhrase = response.ReasonPhrase;
                throw new ApiException($"Status Code: {statusCode}, Reason: {reasonPhrase}, Content: {content}");
            }

            logger.LogDebug("Done sending {@claim} to Claim API", claim);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClaimResponse>(jsonString);
        }
    }
}