using AutoMapper;
using KickSport.Data.Models;
using KickSport.Data.Repository;
using KickSport.Services.DataServices.Contracts;
using KickSport.Services.DataServices.Models.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices
{
    public class IngredientsService : IIngredientsService
    {
        private readonly IGenericRepository<Ingredient> _ingredientsRepository;
        private readonly IMapper _mapper;

        public IngredientsService(
            IGenericRepository<Ingredient> ingredientsRepository,
            IMapper mapper)
        {
            _ingredientsRepository = ingredientsRepository;
            _mapper = mapper;
        }

        public async Task<bool> Any()
        {
            var query = await _ingredientsRepository.GetAllAsync();
            var result = query.ToList().Any();
            return result;
        }

        public async Task<IEnumerable<IngredientDto>> All()
        {
            var ingredient = await _ingredientsRepository.GetAllAsync();
            var ingredientDto = _mapper.Map<IEnumerable<IngredientDto>>(ingredient.ToList()).OrderBy(i => i.Name).ToList();
            return ingredientDto;
        }

        public async Task CreateAsync(string ingredientName)
        {
            var ingredient = new Ingredient
            {
                Name = ingredientName
            };

            await _ingredientsRepository.AddAsync(ingredient);
            await _ingredientsRepository.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(string[] ingredientsName)
        {
            var ingredients = ingredientsName.Select(ingredientName => new Ingredient
            {
                Name = ingredientName
            });

            await _ingredientsRepository.AddRangeAsync(ingredients);
            await _ingredientsRepository.SaveChangesAsync();
        }

        public async Task<IngredientDto> FindByName(string ingredientName)
        {
            var ingredient = await _ingredientsRepository.FindOneAsync(i => i.Name.ToLower() == ingredientName.ToLower());
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
            return ingredientDto;
        }
    }
}
