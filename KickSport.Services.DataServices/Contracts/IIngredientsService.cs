using KickSport.Services.DataServices.Models.Ingredients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IIngredientsService
    {
        Task<bool> Any();

        Task<IEnumerable<IngredientDto>> All();

        Task CreateAsync(string ingredientName);

        Task CreateRangeAsync(string[] ingredientsName);

        Task<IngredientDto> FindByName(string ingredientName);
    }
}
