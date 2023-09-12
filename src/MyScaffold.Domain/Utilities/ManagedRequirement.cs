namespace MyScaffold.Application.Auth
{
    public static class ManagedAction
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string Read = "Read";
    }

    public static class ManagedResource
    {
        public const string User = "User";
        public const string Blog = "Blog";
        public const string Role = "Role";
    }

    public static class ManagedItem
    {
        public const string Id = "Id";
        public const string Limit = "Limit";
        public const string All = "All";
        public const string Dto = "Dto";
    }
}