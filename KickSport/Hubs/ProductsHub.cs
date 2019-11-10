using KickSport.Web.Hubs.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace KickSport.Web.Hubs
{
    public class ProductsHub : Hub<IProductsHubClient>
    {
    }
}
