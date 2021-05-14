using System.Threading.Tasks;

namespace SalesforceExternalClientAppDemo.ConsoleApp.Application
{
    public interface IHelloWorldConnector
    {
        Task<string> GetMessage();
    }
}