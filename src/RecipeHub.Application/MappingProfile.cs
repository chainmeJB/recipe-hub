using RecipeHub.Application.Dto;
using RecipeHub.Infrastructure.Entities;
using AutoMapper;

namespace RecipeHub.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Ingredient, IngredientDto>().ReverseMap();
            CreateMap<Image, ImageDto>().ReverseMap();
            CreateMap<RecipeIngredient, RecipeIngredientDto>().ReverseMap();
            CreateMap<Recipe, RecipeDto>().ReverseMap();
        }
    }
}
