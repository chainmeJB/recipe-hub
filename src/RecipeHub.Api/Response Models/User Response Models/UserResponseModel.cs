using RecipeHub.Application.Dto;

namespace RecipeHub.Api.Response_Models
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}
