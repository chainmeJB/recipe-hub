namespace RecipeHub.Application.Dto
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }

        public string Url { get; set; } = string.Empty;
        public int Position { get; set; }
        public bool IsPrimary { get; set; }
    }
}
