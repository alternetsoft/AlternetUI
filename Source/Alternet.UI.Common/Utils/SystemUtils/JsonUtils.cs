using System;
using System.Collections.Generic;
using System.Text;

using System.Text.Json;

namespace Alternet.UI;

/// <summary>
/// Provides utility methods for working with JSON data.
/// </summary>
public static class JsonUtils
{
    /// <summary>
    /// Represents a delegate that is invoked for each property of a JSON object during enumeration.
    /// </summary>
    /// <param name="property">The JSON property.</param>
    /// <param name="name">The name of the JSON property.</param>
    /// <param name="value">The value of the JSON property as a .NET object.</param>
    public delegate void JsonPropertyDelegate(
        JsonProperty property,
        string name,
        object? value);

    /// <summary>
    /// Enumerates the properties of a JSON object and invokes the specified action for each property.
    /// </summary>
    /// <param name="json">The JSON string representing an object.</param>
    /// <param name="action">The action to invoke for each property, with the property name and value.</param>
    public static void EnumerateProperties(
        string json,
        JsonPropertyDelegate action)
    {
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.ValueKind != JsonValueKind.Object)
            return;

        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            action(prop, prop.Name, GetValue(prop.Value));
        }
    }

    /// <summary>
    /// Gets the value of a JSON element as a .NET object.
    /// </summary>
    /// <param name="element">The JSON element.</param>
    /// <returns>The value of the JSON element as a .NET object.</returns>
    public static object? GetValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,

            JsonValueKind.Number =>
                element.TryGetInt32(out var i) ? i :
                element.TryGetInt64(out var l) ? l :
                element.TryGetDouble(out var d) ? d :
                element.GetDecimal(),

            JsonValueKind.Null => null,

            _ => element // object/array
        };
    }
}
