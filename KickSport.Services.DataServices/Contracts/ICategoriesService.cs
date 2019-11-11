using KickSport.Services.DataServices.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KickSport.Services.DataServices.Contracts
{
    public interface ICategoriesService
    {
        Task<List<CategoryDto>> All();

        Task<bool> Any();

        Task CreateAsync(string categoryName);

        Task CreateRangeAsync(string[] categoriesName);

        Task<CategoryDto> FindByName(string categoryName);
    }
}
