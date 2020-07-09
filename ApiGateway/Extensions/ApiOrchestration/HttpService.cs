using System.Net.Http;

namespace ApiGateway.ApiGateway
{
    public interface IHttpService
    {
        HttpClient Client { get; }
    }

    public class HttpService : IHttpService
    {
        public HttpService(HttpClient client)
        {
            this.Client = client;
        }

        public HttpClient Client { get; }
    }
}
