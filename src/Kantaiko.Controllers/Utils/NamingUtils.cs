﻿namespace Kantaiko.Controllers.Utils
{
    public static class NamingUtils
    {
        public static string ToCamelCase(string name)
        {
            return char.ToLowerInvariant(name[0]) + name[1..];
        }
    }
}
