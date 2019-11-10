using KickSport.Services.DataServices.Models.Ingredients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface IIngredientsService
    {
        bool Any();

        IEnumerable<IngredientDto> All();

        Task CreateAsync(string ingredientName);

        Task CreateRangeAsync(string[] ingredientsName);

        IngredientDto FindByName(string ingredientName);
    }
}
