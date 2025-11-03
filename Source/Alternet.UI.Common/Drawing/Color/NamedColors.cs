using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with named colors.
    /// </summary>
    public static class NamedColors
    {
        private static readonly Lazy<ColorDictionary> ColorConstants = new(GetColors);

        internal static ColorDictionary Colors => ColorConstants.Value;

        /// <summary>
        /// Registers color name for use in string to color conversions.
        /// </summary>
        /// <param name="name">Color name.</param>
        /// <param name="color">Color value.</param>
        public static void RegisterColor(string name, Color color)
        {
            Colors.Add(name, () => color);
        }

        /// <summary>
        /// Registers color name for use in string to color conversions.
        /// </summary>
        /// <param name="name">Color name.</param>
        /// <param name="colorFunc">Function which returns a color.</param>
        public static void RegisterColor(string name, Func<Color> colorFunc)
        {
            Colors.Add(name, colorFunc);
        }

        /// <summary>
        /// Gets a color by its name or raises an exception if no color is registered
        /// for the specified name.
        /// </summary>
        /// <exception cref="ArgumentException">The color was not found.</exception>
        public static Color GetColorOrException(string name)
        {
            var b = GetColorOrNull(name) ?? throw new ArgumentException(
                    "Cannot find a color with the name: " + name,
                    nameof(name));
            return b;
        }

        /// <summary>
        /// Returns a color by its name or <see cref="Color.Empty"/> if no color is registered
        /// for the specified name.
        /// </summary>
        public static Color GetColorOrEmpty(string name)
        {
            var found = Colors.TryGetValue(name, out var colorFunc);
            if (found && colorFunc is not null)
                return colorFunc.Invoke();
            else
                return Color.Empty;
        }

        /// <summary>
        /// Returns a color by its name or <c>null</c> if no color is registered
        /// for the specified name.
        /// </summary>
        public static Color? GetColorOrNull(string name)
        {
            var found = Colors.TryGetValue(name, out var colorFunc);
            if (found)
                return colorFunc?.Invoke();
            else
                return null;
        }

        /// <summary>
        /// Returns a color by its name or a result of <paramref name="defaultFunc"/> call
        /// if no color is registered for the specified name.
        /// </summary>
        public static Color GetColorOrDefault(string name, Func<Color> defaultFunc)
        {
            var found = Colors.TryGetValue(name, out var colorFunc);
            if (found && colorFunc is not null)
                return colorFunc();
            else
                return defaultFunc();
        }

        /// <summary>
        /// Returns a color by its name or <paramref name="defaultValue"/>
        /// if no color is registered for the specified name.
        /// </summary>
        public static Color GetColorOrDefault(string name, Color defaultValue)
        {
            var found = Colors.TryGetValue(name, out var colorFunc);
            if (found && colorFunc is not null)
                return colorFunc();
            else
                return defaultValue ?? Color.Empty;
        }

        /// <summary>
        /// Gets whether specified color name is registered.
        /// </summary>
        /// <param name="name">Color name.</param>
        /// <returns></returns>
        public static bool IsRegistered(string name) => Colors.TryGetValue(name, out _);

        private static ColorDictionary GetColors()
        {
            var colors = new ColorDictionary();
            FillWithProperties(colors, typeof(Color));
            FillWithProperties(colors, typeof(SystemColors));
            return colors;
        }

        private static void FillWithProperties(ColorDictionary dictionary, Type type)
        {
            foreach (var prop in type.GetFields(
                BindingFlags.Public | BindingFlags.Static))
            {
                if (prop.FieldType == typeof(Color))
                    dictionary[prop.Name] = () => (Color)prop.GetValue(null)!;
            }
        }

        internal class ColorDictionary : Dictionary<string, Func<Color>>
        {
            public ColorDictionary()
                : base(StringComparer.OrdinalIgnoreCase)
            {
            }
        }
    }
}
