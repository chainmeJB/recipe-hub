namespace RecipeHub.Api.Authorization
{
    public static class AuthPolicies
    {
        public const string User = "UserOrAdmin";
        public const string Admin = "AdminOnly";
    }
}
