using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to web.
    /// </summary>
    public static class WebUtils
    {
        private static readonly Regex DataUrlRegex = new (
                @"^data:(?<mime>[\w\-\+\.]+\/[\w\-\+\.]+)?(?<encoding>;base64)?,(?<data>.*)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Returns a value indicating whether the specified string is a data URL.
        /// </summary>
        /// <param name="url">The string to check.</param>
        /// <param name="strict">If set to <c>true</c>, performs a strict check
        /// using a regular expression. Default is <c>false</c>.</param>
        /// <returns><c>true</c> if the string is a data URL; otherwise, <c>false</c>.</returns>
        public static bool IsDataUrl(string? url, bool strict = false)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            if (strict)
            {
                return DataUrlRegex.IsMatch(url.Trim());
            }
            else
            {
                return url.TrimStart().StartsWith("data:", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Converts a data URL back into raw bytes.
        /// </summary>
        /// <param name="dataUrl">The data URL string.</param>
        /// <param name="mimeType">Outputs the MIME type (if present).</param>
        /// <returns>Decoded byte array.</returns>
        public static byte[]? DataUrlToBytes(string? dataUrl, out string mimeType)
        {
            if (string.IsNullOrWhiteSpace(dataUrl))
            {
                mimeType = string.Empty;
                return null;
            }

            var match = DataUrlRegex.Match(dataUrl);
            if (!match.Success)
            {
                mimeType = string.Empty;
                return null;
            }

            mimeType = match.Groups["mime"].Value;
            string dataPart = match.Groups["data"].Value;

            if (match.Groups["encoding"].Success)
            {
                return Convert.FromBase64String(dataPart);
            }
            else
            {
                return Encoding.UTF8.GetBytes(Uri.UnescapeDataString(dataPart));
            }
        }

        /// <summary>
        /// Converts a data URL back into a string (for text content).
        /// </summary>
        public static string? DataUrlToText(string? dataUrl, out string mimeType)
        {
            var bytes = DataUrlToBytes(dataUrl, out mimeType);
            if (bytes == null)
                return null;
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Creates a data URL from a byte array.
        /// </summary>
        /// <param name="data">The raw data (e.g., image bytes).</param>
        /// <param name="mimeType">The MIME type (e.g., "image/png").</param>
        /// <returns>A valid data URL string.</returns>
        public static string DataUrlFromBytes(byte[] data, string mimeType)
        {
            string base64 = Convert.ToBase64String(data);
            return $"data:{mimeType};base64,{base64}";
        }

        /// <summary>
        /// Creates a data URL from plain text.
        /// </summary>
        /// <param name="text">The text content.</param>
        /// <param name="mimeType">The MIME type (defaults to "text/plain").</param>
        /// <returns>A valid data URL string.</returns>
        public static string DataUrlFromText(string text, string mimeType = "text/plain")
        {
            string encoded = Uri.EscapeDataString(text);
            return $"data:{mimeType},{encoded}";
        }

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
