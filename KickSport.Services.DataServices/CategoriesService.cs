using AutoMapper;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IGenericRepository<Category> _categoriesRepository;
        private readonly IMapper _mapper;

        public CategoriesService(
            IGenericRepository<Category> categoriesRepository,
            IMapper mapper)
        {
            _categoriesRepository = categoriesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> All()
        {
            //return _categoriesRepository
            //    .All()
            //    .Select(c => )
            //    .OrderBy(c => c.Name);

            var categories = await _categoriesRepository.GetAllAsync();
            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categories.ToList()).OrderBy(c => c.Name).ToList();

            return categoryDto;
        }

        public async Task<bool> Any()
        {
            var query = await _categoriesRepository.GetAllAsync();
            var result = query.ToList().Any();
            return result;
            //return _categoriesRepository.All().Any();
        }

        public async Task CreateAsync(string categoryName)
        {
            var category = new Category
            {
                Name = categoryName
            };

            await _categoriesRepository.AddAsync(category);
            await _categoriesRepository.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(string[] categoriesName)
        {
            var categories = categoriesName
                .Select(categoryName => new Category
                {
                    Name = categoryName
                });

            await _categoriesRepository.AddRangeAsync(categories);
            await _categoriesRepository.SaveChangesAsync();
        }

        public CategoryDto FindByName(string categoryName)
        {
            var query = _categoriesRepository.FindOneAsync(x => x.Name == categoryName);
            var categoryDto = _mapper.Map<CategoryDto>(query);

            return categoryDto;
        }
    }
}
