using MyScaffold.Domain.Entities;
using System.Reflection;

namespace MyScaffold.Domain.Utilities
{
    public static class RequireScopeUtil
    {
        public static Scope[] Scopes { get; set; } = null!;

        public static void Initialize()
        {
            Scopes = Assembly.Load("MyScaffold." + nameof(Application)).GetTypes()
                .Where(type => type.Namespace == "MyScaffold." + nameof(Application) + ".Services")
                .SelectMany(rqt => rqt.GetMethods().Select(m => m.GetCustomAttribute<ScopeAttribute>()).Append(rqt.GetCustomAttribute<ScopeAttribute>()))
                .Select(attribute =>
                {
                    return attribute is null ? null :
                    new Scope
                    {
                        Name = attribute!.Name,
                        Description = attribute!.Description
                    };
                })
                .Where(s => s is not null).Select(s => s!).ToArray();
        }

        public static bool IsExist(string scopeName) => Scopes.Any(s => s.Name == scopeName);

        public static Scope Fill(string scopeName) => Scopes.First(s => s.Name == scopeName);

        public static IEnumerable<Scope> FillAll(IEnumerable<string> scopeNames)
        {
            var result = new List<Scope>();
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

        public static bool TryFillAll(IEnumerable<string> scopeNames, out List<Scope> fullScopes)
        {
            var result = new List<Scope>();
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