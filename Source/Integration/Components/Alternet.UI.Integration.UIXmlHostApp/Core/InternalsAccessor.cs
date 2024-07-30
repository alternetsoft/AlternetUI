#nullable enable
using ExposedObject;
using System;
using System.IO;
using System.Reflection;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    internal static class InternalsAccessor
    {
        public static dynamic CreateObject(Assembly assembly, string typeName, params object?[] parameters)
        {
            return CreateObjectWithParams(assembly, typeName, parameters);
        }

        public static dynamic CreateObjectWithParams(Assembly assembly, string typeName, object?[]? parameters = null)
        {
            Log.Information("========");
            Log.Information("InternalsAccessor.CreateObject");
            Log.Information($"Assembly: {assembly.Location}");
            Log.Information($"typeName: {typeName}");

            Log.Information("========");
            var type = assembly.GetType(typeName);

            object? instance = null;

            if (type is not null)
                instance = Activator.CreateInstance(
                    type,
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    parameters,
                    null);

            return Exposed.From(instance ?? new object());
        }
    }
}