using Bogus;
using Microsoft.Extensions.Logging;
using SalesforceExternalClientAppDemo.ConsoleApp.Application;
using SalesforceExternalClientAppDemo.ConsoleApp.Models;
using System.Threading.Tasks;

namespace SalesforceExternalClientAppDemo.ConsoleApp
{
    public class Startup : IStartup
    {
        private readonly ILogger<Startup> logger;
        private readonly IClaimsConnector claimsConnector;
        private readonly IHelloWorldConnector helloWorldConnector;

        public Startup(ILogger<Startup> logger, IClaimsConnector claimsConnector, IHelloWorldConnector helloWorldConnector)
        {
            this.logger = logger;
            this.claimsConnector = claimsConnector;
            this.helloWorldConnector = helloWorldConnector;
        }

        public async Task Run()
        {
            await DemoHelloWorld();
            // await DemoAddingRecords();
            // await DemoReadingRecords();
            // await DemoUpdatingRecords();
            // await DemoDeletingRecords();

            logger.LogInformation("Demo completed.");
        }

        private async Task DemoHelloWorld()
        {
            var message = await helloWorldConnector.GetMessage();
            logger.LogInformation($"Message: {message}");
        }

        private async Task DemoDeletingRecords()
        {
            var claims = await claimsConnector.GetClaims();
            foreach (var claim in claims)
            {
                await claimsConnector.DeleteClaim(claim.ClaimId);
                logger.LogInformation($"Deleted {claim.FirstName} {claim.LastName}.");
                System.Threading.Thread.Sleep(500);
            }
        }

        private async Task DemoReadingRecords()
        {
            var claims = await claimsConnector.GetClaims();
            foreach (var claim in claims)
            {
                logger.LogInformation($"{claim.ClaimId} - {claim.FirstName} {claim.LastName}");
            }
        }

        private async Task DemoUpdatingRecords()
        {
            var claims = await claimsConnector.GetClaims();
            foreach (var claim in claims)
            {
                // update color
                var faker = new Faker();
                var favoriteColor = faker.Commerce.Color();
                await claimsConnector.UpdateFavoriteColor(new Claim()
                {
                    ClaimNumber = claim.ClaimId,
                    FavoriteColor = favoriteColor
                });
                logger.LogInformation($"{claim.FirstName}'s favorite color is now {favoriteColor}.");
                System.Threading.Thread.Sleep(500);
            }
        }

        private async Task DemoAddingRecords()
        {
            for (int i = 0; i < 10; i++)
            {
                var claimFaker = new Faker<Claim>();
                claimFaker.RuleFor(x => x.FirstName, x => x.Person.FirstName);
                claimFaker.RuleFor(x => x.LastName, x => x.Person.LastName);

                var claimResponse = await claimsConnector.AddClaim(claimFaker.Generate());
                logger.LogInformation($"Added {claimResponse.FirstName} {claimResponse.LastName}: {claimResponse.Id}");
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}