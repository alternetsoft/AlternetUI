using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Static methods related to the value validation.
    /// </summary>
    public static class ValidationUtils
    {
        private static Func<string?, bool> funcIsValidMailAddress = (email) =>
        {
            var method = typeof(MailAddress).GetMethod(
                "TryCreate",
                [typeof(string), typeof(MailAddress).MakeByRefType()]);

            if (method is null)
            {
                funcIsValidMailAddress = (email) =>
                {
                    try
                    {
                        var mail = new MailAddress(email);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                };
            }
            else
            {
                funcIsValidMailAddress = (email) =>
                {
                    return (bool)method.Invoke(null, [email, null]);
                };
            }

            return funcIsValidMailAddress(email);
        };

        /// <summary>
        /// Validates e-mail address.
        /// </summary>
        /// <param name="email">E-mail address.</param>
        /// <returns><c>true</c> if e-mail address is in a valid format;
        /// <c>false</c> otherwise.</returns>
        public static bool IsValidMailAddress(string? email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return funcIsValidMailAddress(email);
        }

        /// <summary>
        /// Validates url address.
        /// </summary>
        /// <param name="url">Url address.</param>
        /// <param name="urlKind">Url kind.</param>
        /// <returns><c>true</c> if url is in a valid format;
        /// <c>false</c> otherwise.</returns>
        public static bool IsValidUrl(string? url, UriKind urlKind = UriKind.Absolute)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            var result = Uri.TryCreate(url, urlKind, out _);
            return result;
        }
    }
}
