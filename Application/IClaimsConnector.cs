using SalesforceExternalClientAppDemo.ConsoleApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Application
{
    public interface IClaimsConnector
    {
        public Task<ClaimResponse> AddClaim(Claim claim);

        public Task<ClaimResponse> UpdateFavoriteColor(Claim claim);

        public Task<List<ClaimResponse>> GetClaims();

        public Task DeleteClaim(string claimNumber);
    }
}