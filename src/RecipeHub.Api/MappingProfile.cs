using AutoMapper;
using RecipeHub.Api.Request_Models; 
using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Dto.Auth;
namespace RecipeHub.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User mapping
            CreateMap<RegisterRequestModel, RegisterRequestDto>();
            CreateMap<LoginRequestModel, LoginRequestDto>();
            CreateMap<AuthResponseDto, AuthResponseModel>();

            CreateMap<UserDto, UserRecipesResponseModel>();
            // short user response model
            CreateMap<UserRecipesResponseModel, UserResponseModel>();

            //Image mapping
            CreateMap<ImageRequestModel, ImageDto>();
            CreateMap<ImageDto, ImageResponseModel>();

            //Category mapping
            CreateMap<CategoryRequestModel, CategoryDto>();
            CreateMap<CategoryDto, CategoryRecipesResponseModel>();
            // short category response model
            CreateMap<CategoryRecipesResponseModel, CategoryResponseModel>();

            //Ingredient mapping
            CreateMap<IngredientRequestModel, IngredientDto>();
            CreateMap<IngredientDto, IngredientResponseModel>();

            //RecipeIngredient mapping
            CreateMap<RecipeIngredientRequestModel, RecipeIngredientDto>();
            CreateMap<RecipeIngredientDto, RecipeIngredientResponseModel>();

            //Recipe mapping
            CreateMap<RecipeRequestModel, RecipeDto>();
            CreateMap<RecipeDto, RecipeResponseModel>();

        }
    }
}
