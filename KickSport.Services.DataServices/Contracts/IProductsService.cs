using KickSport.Services.DataServices.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IProductsService
    {
        Task<List<ProductDto>> All();

        bool Any();

        Task CreateAsync(ProductDto productDto);

        Task CreateRangeAsync(IEnumerable<ProductDto> productsDto);

        Task DeleteAsync(string productId);

        Task EditAsync(ProductDto productDto);

        Task<bool> Exists(string productId);
    }
}
