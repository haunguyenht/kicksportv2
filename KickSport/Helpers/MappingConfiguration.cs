using AutoMapper;
using KickSport.Data.Models;
using KickSport.Services.DataServices.Models.Categories;
using KickSport.Services.DataServices.Models.Ingredients;
using KickSport.Services.DataServices.Models.Orders;
using KickSport.Services.DataServices.Models.Products;
using KickSport.Services.DataServices.Models.Reviews;
using KickSport.Web.Models.Account.FacebookModels;
using KickSport.Web.Models.Account.InputModels;
using KickSport.Web.Models.Categories.ViewModels;
using KickSport.Web.Models.Ingredients.ViewModels;
using KickSport.Web.Models.Orders.InputModels;
using KickSport.Web.Models.Orders.ViewModels;
using KickSport.Web.Models.Products.ViewModels;
using KickSport.Web.Models.Reviews.ViewModels;
using System.Linq;

namespace KickSport.Helpers
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<RegisterInputModel, ApplicationUser>();
            CreateMap<FacebookUserData, ApplicationUser>();

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.CreatorUsername, opt => opt.MapFrom(src => src.Creator.UserName));
            CreateMap<ReviewDto, ReviewViewModel>();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Select(l => l.ApplicationUser)))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(i => i.Ingredient)));
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(idto => new ProductsIngredients
                {
                    IngredientId = idto.Id
                })))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore());
            CreateMap<ProductDto, ProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Ingredients,
                    opt => opt.MapFrom(src => src.Ingredients.Select(i => i.Name)))
                .ForMember(dest => dest.Likes,
                    opt => opt.MapFrom(src => src.Likes.Select(u => u.UserName)));

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, CategoryViewModel>();

            CreateMap<Ingredient, IngredientDto>();
            CreateMap<IngredientDto, IngredientViewModel>();

            CreateMap<OrderProductInputModel, OrderProductDto>();
            CreateMap<OrderProductDto, OrderProductViewModel>();
            CreateMap<OrderProductDto, OrderProduct>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<OrderProduct, OrderProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CreatorEmail, opt => opt.MapFrom(src => src.Creator.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.Products));
            CreateMap<OrderDto, OrderViewModel>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.CreationDate));
        }
    }
}
