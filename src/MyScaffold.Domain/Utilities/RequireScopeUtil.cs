using MyScaffold.Domain.ValueObjects;
using System.Reflection;

namespace MyScaffold.Domain.Utilities
{
    public static class RequireScopeUtil
    {
        public static ScopeDefinition[] Scopes { get; set; } = null!;

        public static void Initialize()
        {
            Scopes = Assembly.Load("MyScaffold." + nameof(Application)).GetTypes()
                .Where(type => type.Namespace == "MyScaffold." + nameof(Application) + ".Services")
                .SelectMany(rqt => rqt.GetMethods().Select(m => m.GetCustomAttribute<ScopeDefinitionAttribute>()).Append(rqt.GetCustomAttribute<ScopeDefinitionAttribute>()))
                .Select(attribute =>
                {
                    return attribute is null ? null :
                    new ScopeDefinition
                    {
                        Name = attribute!.Name,
                        Description = attribute!.Description
                    };
                })
                .Where(s => s is not null).Select(s => s!).ToArray();
        }

        public static bool IsExist(string scopeName) => Scopes.Any(s => s.Name == scopeName);

        public static ScopeDefinition Fill(string scopeName) => Scopes.First(s => s.Name == scopeName);

        public static IEnumerable<ScopeDefinition> FillAll(IEnumerable<string> scopeNames)
        {
            var result = new List<ScopeDefinition>();
            var enumerator = scopeNames.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var fullScope = Fill(enumerator.Current);
                if (fullScope is null)
                {
                    continue;
                }
                else
                {
                    result.Add(fullScope);
                }
            }
            return result;
        }

        public static bool TryFillAll(IEnumerable<string> scopeNames, out List<ScopeDefinition> fullScopes)
        {
            var result = new List<ScopeDefinition>();
            var enumerator = scopeNames.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var fullScope = Fill(enumerator.Current);
                if (fullScope is null)
                {
                    fullScopes = result;
                    return false;
                }
                else
                {
                    result.Add(fullScope);
                }
            }
            fullScopes = result;
            return true;
        }
    }
}