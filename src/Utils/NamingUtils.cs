namespace Kantaiko.Controllers.Utils;

internal static class NamingUtils
{
    public static string ToCamelCase(string name)
    {
        return char.ToLowerInvariant(name[0]) + name[1..];
    }
}
