using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IProductsIngredientsService
    {
        Task DeleteProductIngredientsAsync(string productId);
    }
}
