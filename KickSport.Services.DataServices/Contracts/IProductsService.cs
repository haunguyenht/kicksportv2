using KickSport.Services.DataServices.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IProductsService
    {
        IEnumerable<ProductDto> All();

        bool Any();

        Task CreateAsync(ProductDto product);

        Task CreateRangeAsync(IEnumerable<ProductDto> products);

        Task DeleteAsync(string productId);

        Task EditAsync(ProductDto product);

        bool Exists(string productId);
    }
}
