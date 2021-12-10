using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Models;

namespace Services
{
    public class ResourceService : IResourceService
    {

        private readonly HttpClient _client;
        public ResourceService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<ResourceResponseModel> CreateResource(ResourceRequestModel request)
        {
            return await _client.GetJsonAsync<ResourceResponseModel>("api/Resources/Create");
        }

        public async Task CreateUser(string id)
        {
            await _client.GetJsonAsync<object>("api/User/Create");

        }
    }
}