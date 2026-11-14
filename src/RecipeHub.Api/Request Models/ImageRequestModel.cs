namespace RecipeHub.Api.Request_Models
{
    public class ImageRequestModel
    {
        public int RecipeId { get; set; }

        public string Url { get; set; } = string.Empty;
        public int Position { get; set; }
        public bool IsPrimary { get; set; }
    }
}
