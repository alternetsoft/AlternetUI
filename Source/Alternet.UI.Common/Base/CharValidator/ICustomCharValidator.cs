using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allow to check whether the character (or it's category)
    /// is valid for the input.
    /// </summary>
    public interface ICustomCharValidator : IImmutableObject
    {
        /// <summary>
        /// Checks whether the specified char and it's category are valid for the input.
        /// </summary>
        /// <param name="ch">Character to check.</param>
        /// <returns><c>true</c> if char and it's category are valid for the input;
        /// <c>false</c> otherwise.</returns>
        bool IsValid(char ch);

        /// <summary>
        /// Checks whether the specified char category is valid for the input.
        /// </summary>
        /// <param name="ch">Character which is used to get the category.</param>
        /// <returns><c>true</c> if category is valid for the input;
        /// <c>false</c> otherwise.</returns>
        bool IsValidCategory(char ch);

        /// <summary>
        /// Checks whether the specified char is valid for the input.
        /// This method doesn't perform checks on char category, use <see cref="IsValid"/>
        /// to check both char and it's category.
        /// </summary>
        /// <param name="ch">Character to check.</param>
        /// <returns><c>true</c> if char is valid for the input;
        /// <c>false</c> otherwise.</returns>
        bool IsValidChar(char ch);

        /// <summary>
        /// Checks whether the specified char category is valid for the input.
        /// </summary>
        /// <param name="cat">Category to check.</param>
        /// <returns><c>true</c> if category is valid for the input;
        /// <c>false</c> otherwise.</returns>
        bool IsValidCategory(UnicodeCategory cat);
    }
}
