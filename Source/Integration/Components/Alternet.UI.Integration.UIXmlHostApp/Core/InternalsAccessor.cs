using ExposedObject;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    internal static class InternalsAccessor
    {
        public static dynamic CreateObject(Assembly assembly, string typeName, params object[] parameters)
        {
            Logger.Instance.Information("========");
            Logger.Instance.Information("InternalsAccessor.CreateObject");
            Logger.Instance.Information($"Assembly: {assembly.CodeBase}");
            Logger.Instance.Information($"typeName: {typeName}");

            Logger.Instance.Information("========");
            var type = assembly.GetType(typeName);
            var instance = Activator.CreateInstance(type, BindingFlags.Public | BindingFlags.Instance, null, parameters, null);
            return Exposed.From(instance);
        }
    }
}