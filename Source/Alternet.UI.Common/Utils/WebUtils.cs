using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to web.
    /// </summary>
    public static class WebUtils
    {
        /// <summary>
        /// Checks whether specified string a valid json text.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="s"/> is a valid json string;
        /// otherwise returns <c>false</c>.
        /// </returns>
        public static bool IsValidJson(string s)
        {
            try
            {
                var tmpObj = JsonValue.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts string to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="plainText">Plain text string to convert.</param>
        /// <returns>The string representation, in base 64, of the source string.</returns>
        public static string Base64Encode(string? plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Converts string with data that is encoded with base-64 digits to the plain text string.
        /// </summary>
        /// <param name="base64EncodedData">String with data, encoded with base-64 digits.</param>
        /// <returns>Plain text string.</returns>
        public static string Base64Decode(string? base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                return string.Empty;

            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Converts json string to indented string.
        /// </summary>
        /// <param name="unPrettyJson">Json string without indentation.</param>
        /// <returns></returns>
        public static string PrettyJson(string unPrettyJson)
        {
            try
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                };

                var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

                return JsonSerializer.Serialize(jsonElement, options);
            }
            catch
            {
                return unPrettyJson;
            }
        }
    }
}
