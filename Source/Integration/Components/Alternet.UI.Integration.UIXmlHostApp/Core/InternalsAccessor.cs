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
            var type = assembly.GetType(typeName);
            var instance = Activator.CreateInstance(type, BindingFlags.Public | BindingFlags.Instance, null, parameters, null);
            return Exposed.From(instance);
        }
    }
}