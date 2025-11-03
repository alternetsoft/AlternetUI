using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for property names.
    /// </summary>
    public class PropNameStrings
    {
        /// <summary>
        /// Current localizations for property names.
        /// </summary>
        public static PropNameStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets localized property name.
        /// </summary>
        public string Left { get; set; } = "Left";

        /// <inheritdoc cref="Left"/>
        public string Top { get; set; } = "Top";

        /// <inheritdoc cref="Left"/>
        public string Right { get; set; } = "Right";

        /// <inheritdoc cref="Left"/>
        public string Bottom { get; set; } = "Bottom";

        /// <inheritdoc cref="Left"/>
        public string Width { get; set; } = "Width";

        /// <inheritdoc cref="Left"/>
        public string Height { get; set; } = "Height";

        /// <inheritdoc cref="Left"/>
        public string Center { get; set; } = "Center";

        /// <inheritdoc cref="Left"/>
        public string Column { get; set; } = "Column";

        /// <inheritdoc cref="Left"/>
        public string Row { get; set; } = "Row";

        /// <inheritdoc cref="Left"/>
        public string X { get; set; } = "X";

        /// <inheritdoc cref="Left"/>
        public string Y { get; set; } = "Y";

        /// <summary>
        /// Registers property name localizations in the <see cref="PropertyGrid"/> infrastructure.
        /// Uses nested types of the <paramref name="localizationsContainer"/> type as
        /// a source of the localized property names.
        /// </summary>
        /// <remarks>
        /// Before this call all nested static classes of the
        /// <paramref name="localizationsContainer"/> type which contain
        /// property name localizations should be
        /// assigned with appropriate localization.
        /// </remarks>
        /// <param name="localizationsContainer">Container of the nested static classes with property name
        /// localizations. Each such nested class should have 'Properties' suffix in the class name.</param>
        /// <param name="namespacePrefix">Prefix of the namespace. Optional.
        /// By default equals 'Alternet.UI.'.</param>
        /// <param name="asm">Assembly used to find types by name.</param>
        public static void RegisterPropNameLocalizations(
            Type localizationsContainer,
            Assembly? asm = null,
            string namespacePrefix = "Alternet.UI.")
        {
            var nestedTypes = localizationsContainer.GetNestedTypes();

            foreach(var nestedType in nestedTypes)
            {
                var splitted = nestedType.FullName?.Split('+') ?? Array.Empty<string>();
                if (splitted.Length < 2)
                    continue;
                var suffix = splitted[1];
                if (!suffix.HasSuffix("Properties"))
                    continue;
                suffix = suffix.Remove(suffix.Length - "Properties".Length);
                var registerForTypeName = $"{namespacePrefix}{suffix}";

                var registerForType
                    = Resolve(asm, registerForTypeName)
                    ?? Resolve(KnownAssemblies.LibraryCommon, registerForTypeName);

                if (registerForType is null)
                    continue;

                var realType = AssemblyUtils.GetRealType(registerForType);
                if (realType.IsEnum)
                    RegisterEnumValueLocalizations(realType, nestedType);
                else
                    RegisterPropNameLocalizations(registerForType, nestedType);
            }

            Type? Resolve(Assembly? asmToResolve, string typeName)
            {
                return asmToResolve?.GetType(typeName);
            }
        }

        /// <summary>
        /// Registers enum values localizations in the <see cref="PropertyGrid"/> infrastructure
        /// for the specified type using localization container specified in the
        /// <paramref name="localizations"/> parameter.
        /// </summary>
        /// <param name="type">Type for which enum values localizations are registered.</param>
        /// <param name="localizations">Type of the class which contains
        /// static fields with enum values localizations.</param>
        public static void RegisterEnumValueLocalizations(Type type, Type localizations)
        {
            if (!type.IsEnum)
                return;

            var fields = localizations.GetFields(BindingFlags.Static | BindingFlags.Public);
            var choices = PropertyGrid.CreateChoicesOnce(type);

            foreach (var field in fields)
            {
                var name = field.Name;
                var value = field.GetValue(null);

                if (value is not string str)
                    continue;
                if (string.IsNullOrEmpty(str))
                    continue;

                var parsedName = Enum.Parse(type, name);

                if (parsedName is null)
                    continue;

                choices.SetLabelForValue(Convert.ToInt32(parsedName), str);
            }
        }

        /// <summary>
        /// Registers property name localizations in the <see cref="PropertyGrid"/> infrastructure
        /// for the specified type using localization container specified in the
        /// <paramref name="localizations"/> parameter.
        /// </summary>
        /// <param name="type">Type for which property name localizations are registered.</param>
        /// <param name="localizations">Type of the class which contains
        /// static string fields with property name localizations.</param>
        public static void RegisterPropNameLocalizations(Type type, Type localizations)
        {
            var fields = localizations.GetFields(BindingFlags.Static | BindingFlags.Public);

            var registry = PropertyGrid.GetTypeRegistry(type);

            foreach (var field in fields)
            {
                var name = field.Name;
                var value = field.GetValue(null);

                if (value is not string str)
                    continue;
                if (string.IsNullOrEmpty(str))
                    continue;
                var propRegistry = registry.GetPropRegistry(name);
                if (propRegistry is null)
                    continue;
                var prm = propRegistry.NewItemParams;
                prm.Label = str;
            }
        }
    }
}