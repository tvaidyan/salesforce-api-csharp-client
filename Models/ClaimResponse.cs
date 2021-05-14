using Newtonsoft.Json;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Models
{
    public class ClaimResponse
    {
        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }

        [JsonProperty("Name")]
        public string ClaimId { get; set; }

        [JsonProperty("FavoriteColor__c")]
        public string FavoriteColor { get; set; }

        [JsonProperty("FirstName__c")]
        public string FirstName { get; set; }

        [JsonProperty("LastName__c")]
        public string LastName { get; set; }

        public string Id { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}