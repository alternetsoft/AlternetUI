using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="ICustomCharValidator"/>.
    /// Contains methods and properties which allow to specify valid and invalid
    /// chars and chars categories for the input.
    /// </summary>
    public interface ICharValidator : ICustomCharValidator
    {
        /// <summary>
        /// Allows native digits returned by <see cref="CharValidator.NativeDigits"/> for the input.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllowNativeDigits();

        /// <summary>
        /// Allows negative sign chars for the input (minus).
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllowNegativeSign();

        /// <summary>
        /// Allows positive sign chars for the input (plus).
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllowPositiveSign();

        /// <summary>
        /// Allows digits [0..9] for the input.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllowDigits();

        /// <summary>
        /// Allows hex digits for the input. Hex digits are [0..9], [a..f], [A..F].
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllowHexDigits();

        /// <summary>
        /// Resets object to the initial state.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator Reset();

        /// <summary>
        /// Sets the specified character as invalid for the input.
        /// </summary>
        /// <param name="ch">Character to change state for.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator BadChar(char ch);

        /// <summary>
        /// Sets the specified character as valid or invalid for the input.
        /// </summary>
        /// <param name="ch">Character to change state for.</param>
        /// <param name="valid">Whether char is valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidChar(char ch, bool valid = true);

        /// <summary>
        /// Sets all the characters in the range between
        /// <paramref name="minCh"/> and <paramref name="maxCh"/>
        /// as valid or invalid for the input.
        /// </summary>
        /// <param name="minCh">Minimal character to change state for.</param>
        /// <param name="maxCh">Maximal character to change state for.</param>
        /// <param name="valid">Whether char is valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidRange(char minCh, char maxCh, bool valid = true);

        /// <summary>
        /// Sets all the characters in the range between
        /// <paramref name="minCh"/> and <paramref name="maxCh"/>
        /// as invalid for the input.
        /// </summary>
        /// <param name="minCh">Minimal character to change state for.</param>
        /// <param name="maxCh">Maximal character to change state for.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator BadRange(char minCh, char maxCh);

        /// <summary>
        /// Sets all the characters from the specified array
        /// as invalid for the input.
        /// </summary>
        /// <param name="ch">Array of characters to change state for.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator BadChars(params char[] ch);

        /// <summary>
        /// Sets the specifieds characters as valid for the input.
        /// </summary>
        /// <param name="ch">Characters to change state for.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidChars(params char[] ch);

        /// <summary>
        /// Sets the specifieds characters as valid or invalid for the input.
        /// </summary>
        /// <param name="ch">Characters to change state for.</param>
        /// <param name="valid">Whether chars are valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidChars(char[] ch, bool valid);

        /// <summary>
        /// Sets all string characters as valid or invalid for the input.
        /// </summary>
        /// <param name="s">Lists characters to change state for.</param>
        /// <param name="valid">Whether chars are valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidChars(string? s, bool valid = true);

        /// <summary>
        /// Sets all string characters as valid or invalid for the input.
        /// </summary>
        /// <param name="s">Lists characters to change state for.</param>
        /// <param name="valid">Whether chars are valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidChars(string[] s, bool valid = true);

        /// <summary>
        /// Sets all string characters as invalid for the input.
        /// </summary>
        /// <param name="s">Lists characters to change state for.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator BadChars(string? s);

        /// <summary>
        /// Sets all possible characters as valid or invalid for the input.
        /// </summary>
        /// <param name="valid">Whether chars are valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllCharsValid(bool valid = true);

        /// <summary>
        /// Sets all possible characters as invalid for the input.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllCharsBad();

        /// <summary>
        /// Sets all possible character categories (<see cref="UnicodeCategory"/>)
        /// as invalid for the input.
        /// </summary>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllCategoriesBad();

        /// <summary>
        /// Sets all possible character categories (<see cref="UnicodeCategory"/>)
        /// as valid or invalid for the input.
        /// </summary>
        /// <param name="valid">Whether char categories are valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator AllCategoriesValid(bool valid = true);

        /// <summary>
        /// Sets the specified character category
        /// as valid or invalid for the input.
        /// </summary>
        /// <param name="cat">Char category.</param>
        /// <param name="valid">Whether char category is valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidCategory(UnicodeCategory cat, bool valid = true);

        /// <summary>
        /// Sets the specified character category
        /// as invalid for the input.
        /// </summary>
        /// <param name="cat">Char category.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator BadCategory(UnicodeCategory cat);

        /// <summary>
        /// Sets the specified character categories
        /// as valid for the input.
        /// </summary>
        /// <param name="cat">Array of char categories.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidCategories(params UnicodeCategory[] cat);

        /// <summary>
        /// Sets the specified character categories
        /// as valid or invalid for the input.
        /// </summary>
        /// <param name="cat">Array of char categories.</param>
        /// <param name="valid">Whether char categories are valid or invalid for the input.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator ValidCategories(UnicodeCategory[] cat, bool valid);

        /// <summary>
        /// Sets the specified character categories
        /// as invalid for the input.
        /// </summary>
        /// <param name="cat">Array of char categories.</param>
        /// <returns>Returns this object instance for use in the call sequences.</returns>
        ICharValidator BadCategories(params UnicodeCategory[] cat);
    }
}