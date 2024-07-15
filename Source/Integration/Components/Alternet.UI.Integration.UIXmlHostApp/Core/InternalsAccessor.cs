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
            Logger.Instance.Information("========");
            Logger.Instance.Information("InternalsAccessor.CreateObject");
            Logger.Instance.Information($"Assembly: {assembly.Location}");
            Logger.Instance.Information($"typeName: {typeName}");

            Logger.Instance.Information("========");
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