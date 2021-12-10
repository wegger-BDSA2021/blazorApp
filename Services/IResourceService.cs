using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IResourceService 
    {
        Task<ResourceResponseModel> CreateResource(ResourceRequestModel request);
    }
}