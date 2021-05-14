using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Settings
{
    public class SalesforceSettings
    {
        [Required]
        [Url]
        public string ApiBaseUrl { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string PrivateKeyFilename { get; set; }

        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [Url]
        public string TokenEndpoint { get; set; }

        [Required]
        public string Passphrase { get; set; }

        [Required]
        public string ApiVersion { get; set; }

        [Required]
        public bool IsProduction { get; set; }

        internal string GetPrivateKey()
        {
            try
            {
                return File.ReadAllText(PrivateKeyFilename);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string AccessToken { get; set; }
    }
}