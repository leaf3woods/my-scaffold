namespace MyScaffold.Domain.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
    public class ScopeAttribute : Attribute
    {
        public ScopeAttribute(string description, params string[] names)
        {
            Name = string.Join('.', names);
            Description = description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}