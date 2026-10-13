using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.Json;

namespace Alternet.UI;

/// <summary>
/// Provides utility methods for working with JSON data.
/// </summary>
public static class JsonUtils
{
    /// <summary>
    /// Defines flags for controlling the behavior of the <see cref="ToJsonString"/> method.
    /// </summary>
    public enum ToJsonStringFlags
    {
        /// <summary>
        /// Indicates that no special formatting should be applied to the JSON string.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that a comma should be added after the JSON string representation of the property bag.
        /// </summary>
        AddComma = 1,
    }

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
    /// Escapes a string for safe inclusion in JSON by encoding special characters.
    /// </summary>
    /// <param name="value">The string to escape.</param>
    /// <returns>The escaped string.</returns>
    public static string EscapeString(string value)
    {
        var result = JsonEncodedText.Encode(value).ToString();
        return result;
    }

    /// <summary>
    /// Determines whether the specified value requires quotes when represented in JSON.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the value requires quotes; otherwise, <c>false</c>.</returns>
    public static bool ValueNeedQuotes(object? value)
    {
        return value switch
        {
            null => false,

            string => true,
            char => true,

            bool => false,

            sbyte or byte or
            short or ushort or
            int or uint or
            long or ulong or
            float or double or decimal => false,

            _ => true
        };
    }

    /// <summary>
    /// Converts a collection of name-value pairs into a JSON string representation
    /// and writes it to the specified <see cref="ILogWriter"/>.
    /// </summary>
    /// <param name="nameValuePairs">The collection of name-value pairs to convert to JSON.</param>
    /// <param name="writer">The <see cref="ILogWriter"/> to write the JSON string to.</param>
    /// <param name="flags">Flags that control the behavior of the JSON string conversion.</param>
    public static void ToJsonString(
        IEnumerable<KeyValuePair<string, object?>> nameValuePairs,
        ILogWriter? writer = null,
        ToJsonStringFlags flags = ToJsonStringFlags.None)
    {
        if (writer is null)
            return;

        writer.WriteLine("{");
        writer.Indent();

        var pairs = nameValuePairs.ToArray();

        for (int i = 0; i < pairs.Length; i++)
        {
            var propertyName = pairs[i].Key;
            var escapedPropertyName = JsonUtils.EscapeString(propertyName);
            var isLastProperty = (i == pairs.Length - 1);

            void EmitQuotedValue(string? value)
            {
                var escapedValue = JsonUtils.EscapeString(value ?? string.Empty);
                writer.WriteLine($"\"{escapedPropertyName}\" : \"{escapedValue}\"{(isLastProperty ? "" : ",")}");
            }

            void EmitValue(string? value)
            {
                writer.WriteLine($"\"{escapedPropertyName}\" : {value}{(isLastProperty ? "" : ",")}");
            }

            var value = pairs[i].Value;
            var needQuotes = ValueNeedQuotes(value);

            if (needQuotes)
            {
                EmitQuotedValue(value?.ToString());
            }
            else
            {
                EmitValue(value?.ToString());
            }
        }

        writer.Unindent();
        writer.WriteLine(flags.HasFlag(ToJsonStringFlags.AddComma) ? "}," : "}");
    }

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
