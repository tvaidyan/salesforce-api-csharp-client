using ForceDotNetJwtCompanion;
using SalesforceExternalClientAppDemo.ConsoleApp.Exceptions;
using SalesforceExternalClientAppDemo.ConsoleApp.Helpers;
using SalesforceExternalClientAppDemo.ConsoleApp.Settings;
using System;
using System.Text;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Authentication
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly SalesforceSettings salesforceSettings;

        public AuthenticationHelper(SalesforceSettings salesforceSettings)
        {
            this.salesforceSettings = salesforceSettings;
        }

        public string DoAuthentication()
        {
            var privateKey = salesforceSettings.GetPrivateKey();

            try
            {
                var authClient = new JwtAuthenticationClient(salesforceSettings.ApiVersion, salesforceSettings.IsProduction);
                authClient.JwtPrivateKeyAsync(
                            salesforceSettings.ClientId,
                            privateKey,
                            salesforceSettings.Passphrase,
                            salesforceSettings.Username,
                            salesforceSettings.TokenEndpoint
                            ).Wait();

                return authClient.AccessToken;
            }
            catch (Exception ex)
            {
                var exceptions = ExceptionsHelper.GetExceptionDetailsAsString(ex);
                var errorMessageBuilder = new StringBuilder();
                errorMessageBuilder.AppendLine("Authentication to Salesforce failed.");
                // TODO
                // Do more parsing and handling of exceptions here.
                errorMessageBuilder.Append(exceptions);

                throw new AuthenticationFailedException(errorMessageBuilder.ToString());
            }
        }
    }
}