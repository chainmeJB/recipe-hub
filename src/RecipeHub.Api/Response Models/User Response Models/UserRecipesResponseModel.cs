namespace RecipeHub.Api.Response_Models
{
    public class UserRecipesResponseModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;

        public ICollection<RecipeResponseModel> Recipes { get; set; } = new List<RecipeResponseModel>();
    }
}
