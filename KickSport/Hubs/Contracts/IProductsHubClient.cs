using KickSport.Web.Models.Products.ViewModels;
using System.Threading.Tasks;

namespace KickSport.Web.Hubs.Contracts
{
    public interface IProductsHubClient
    {
        Task BroadcastProduct(ProductViewModel product);
    }
}
