using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using Models;

namespace Services
{
    public class ResourceService : IResourceService
    {

        private readonly HttpClient _client;
        private readonly ITokenAcquisition _tokenAcquisition;
        public ResourceService(HttpClient httpClient, ITokenAcquisition tokenAcquisition)
        {
            _client = httpClient;
            _tokenAcquisition = tokenAcquisition;
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "api://23d9a97d-3387-499c-83bc-0ae84d198403/ReadAccess" });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ResourceResponseModel> CreateResource(ResourceRequestModel request)
        {
            await PrepareAuthenticatedClient();
            // var response = await _client.PostJsonAsync<ResourceResponseModel>("api/Resource/Create", request);
            // System.Console.WriteLine(response);
            // return response;
            var response = await _client.GetJsonAsync<ResourceResponseModel>("api/Resource/Create");
            return response;
        }

        public async Task CreateUser(string id)
        {
            await _client.GetJsonAsync<object>("api/User/Create");
        }
    }
}