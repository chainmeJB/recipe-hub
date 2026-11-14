using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto;

namespace RecipeHub.Api.Request_Models
{
    public class CategoryRequestModel
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<RecipeRequestModel> Recipes { get; set; } = new List<RecipeRequestModel>();
    }
}
