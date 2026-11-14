namespace RecipeHub.Api.Response_Models
{
    public class CategoryRecipesResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RecipeResponseModel> Recipes { get; set; } = new List<RecipeResponseModel>();
    }
}
