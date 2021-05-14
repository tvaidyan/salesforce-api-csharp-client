using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Helpers
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }), Encoding.UTF8, "application/json")
        { }
    }
}