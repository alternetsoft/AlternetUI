// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alternet.Drawing
{
    internal static class ColorTable
    {
        private static readonly Lazy<Dictionary<string, Color>> ColorConstants = new(GetColors);

        internal static Dictionary<string, Color> Colors => ColorConstants.Value;

        private static Dictionary<string, Color> GetColors()
        {
            var colors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
            FillWithProperties(colors, typeof(Color));
            FillWithProperties(colors, typeof(SystemColors));
            return colors;
        }

        private static void FillWithProperties(
            Dictionary<string, Color> dictionary,
            Type typeWithColors)
        {
            foreach (PropertyInfo prop in typeWithColors.GetProperties(
                BindingFlags.Public | BindingFlags.Static))
            {
                if (prop.PropertyType == typeof(Color))
                    dictionary[prop.Name] = (Color)prop.GetValue(null, null)!;
            }
        }

        internal static bool TryGetNamedColor(string name, out Color result) =>
            Colors.TryGetValue(name, out result);

        internal static bool IsKnownNamedColor(string name) => Colors.TryGetValue(name, out _);
    }
}
