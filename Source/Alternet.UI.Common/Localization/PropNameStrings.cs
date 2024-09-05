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
        public string X { get; set; } = "X";

        /// <inheritdoc cref="Left"/>
        public string Y { get; set; } = "Y";

        /// <summary>
        /// Registers property name localizations in the <see cref="PropertyGrid"/> infrastructure.
        /// </summary>
        /// <remarks>
        /// Before this call all static classes similar to <see cref="ControlProperties"/> with
        /// property name localizations should be
        /// assigned with appropriate localization.
        /// </remarks>
        public static void RegisterPropNameLocalizations()
        {
            var rootType = typeof(PropNameStrings);
            var asm = rootType.Assembly;
            var nestedTypes = rootType.GetNestedTypes();

            foreach(var nestedType in nestedTypes)
            {
                var splitted = nestedType.FullName.Split('+');
                if (splitted.Length < 2)
                    continue;
                var suffix = splitted[1];
                if (!suffix.HasSuffix("Properties"))
                    continue;
                suffix = suffix.Remove(suffix.Length - "Properties".Length);
                var registerForTypeName = $"Alternet.UI.{suffix}";

                var registerForType = asm.GetType(registerForTypeName);
                if (registerForType is null)
                    continue;
                RegisterPropNameLocalizations(registerForType, nestedType);
            }
        }

        /// <summary>
        /// Registers property name localizations in the <see cref="PropertyGrid"/> infrastructure
        /// for the specified type using localization container specified in the
        /// <paramref name="localizations"/> parameter.
        /// </summary>
        /// <param name="type">Type for which property name localizations are registered.</param>
        /// <param name="localizations">Type similar to <see cref="ControlProperties"/> which contains
        /// static fields with property name localizations.</param>
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
                var prm = propRegistry.NewItemParams;
                prm.Label = str;
            }
        }

        /// <summary>
        /// Contains localizations for <see cref="Control"/> property names.
        /// Used by <see cref="RegisterPropNameLocalizations()"/>.
        /// </summary>
        public static class ControlProperties
        {
            /// <summary>
            /// Get or sets default localization for the corresponding property
            /// of the <see cref="Control"/>.
            /// </summary>
            public static string? Layout;

            /// <see cref="Layout"/>
            public static string? Title;

            /// <see cref="Layout"/>
            public static string? Dock;

            /// <see cref="Layout"/>
            public static string? Text;

            /// <see cref="Layout"/>
            public static string? ToolTip;

            /// <see cref="Layout"/>
            public static string? Left;

            /// <see cref="Layout"/>
            public static string? Top;

            /// <see cref="Layout"/>
            public static string? Visible;

            /// <see cref="Layout"/>
            public static string? Enabled;

            /// <see cref="Layout"/>
            public static string? Width;

            /// <see cref="Layout"/>
            public static string? Height;

            /// <see cref="Layout"/>
            public static string? SuggestedWidth;

            /// <see cref="Layout"/>
            public static string? SuggestedHeight;

            /// <see cref="Layout"/>
            public static string? MinChildMargin;

            /// <see cref="Layout"/>
            public static string? Margin;

            /// <see cref="Layout"/>
            public static string? Padding;

            /// <see cref="Layout"/>
            public static string? MinWidth;

            /// <see cref="Layout"/>
            public static string? MinHeight;

            /// <see cref="Layout"/>
            public static string? MaxWidth;

            /// <see cref="Layout"/>
            public static string? MaxHeight;

            /// <see cref="Layout"/>
            public static string? BackgroundColor;

            /// <see cref="Layout"/>
            public static string? ParentBackColor;

            /// <see cref="Layout"/>
            public static string? ParentForeColor;

            /// <see cref="Layout"/>
            public static string? ParentFont;

            /// <see cref="Layout"/>
            public static string? ForegroundColor;

            /// <see cref="Layout"/>
            public static string? Font;

            /// <see cref="Layout"/>
            public static string? IsBold;

            /// <see cref="Layout"/>
            public static string? VerticalAlignment;

            /// <see cref="Layout"/>
            public static string? HorizontalAlignment;
        }
    }
}