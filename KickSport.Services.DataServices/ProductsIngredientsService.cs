using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices
{
    public class ProductsIngredientsService : IProductsIngredientsService
    {
        private readonly IGenericRepository<ProductsIngredients> _productsIngredientsRepository;

        public ProductsIngredientsService(IGenericRepository<ProductsIngredients> productsIngredientsRepository)
        {
            _productsIngredientsRepository = productsIngredientsRepository;
        }

        public async Task DeleteProductIngredientsAsync(string productId)
        {
            var productIngredients = _productsIngredientsRepository
                .DbSet
                .Where(pi => pi.ProductId == productId)
                .ToList();

            if (productIngredients.Any())
            {
                await _productsIngredientsRepository.DeleteRange(productIngredients);
                await _productsIngredientsRepository.SaveChangesAsync();
            }
        }
    }
}
