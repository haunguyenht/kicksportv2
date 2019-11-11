using AutoMapper;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices
{
    public class ProductsService : IProductsService
    {
        private readonly IGenericRepository<Product> _productsRepository;
        private readonly IMapper _mapper;

        public ProductsService(
            IGenericRepository<Product> productsRepository,
            IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> All()
        {
            var products = await _productsRepository
                .DbSet
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Ingredients)
                .ThenInclude(pi => pi.Ingredient)
                .Include(p => p.Likes)
                .ThenInclude(ul => ul.ApplicationUser)
                .ToListAsync();
            var productDto = _mapper.Map<IEnumerable<ProductDto>>(products).ToList();
            return productDto;
        }

        public bool Any()
        {
            return _productsRepository.DbSet.Any();
        }

        public async Task CreateAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            await _productsRepository.AddAsync(product);
            await _productsRepository.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<ProductDto> productsDto)
        {
            var products = productsDto
                .Select(pdto => _mapper.Map<Product>(pdto));

            await _productsRepository.AddRangeAsync(products);
            await _productsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string productId)
        {
            var product = await _productsRepository
                .FirstAsync(p => p.Id == productId);

            await _productsRepository.Delete(product);
            await _productsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            await _productsRepository.Update(product);
            await _productsRepository.SaveChangesAsync();
        }

        public async Task<bool> Exists(string productId)
        {
            var existedProduct = await _productsRepository.FindOneAsync(p => p.Id == productId);

            return existedProduct != null;
        }
    }
}
