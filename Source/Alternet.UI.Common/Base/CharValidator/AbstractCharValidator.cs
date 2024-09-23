using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Abstract implementation of the <see cref="ICharValidator"/> interface
    /// </summary>
    public abstract class AbstractCharValidator : ImmutableObject, ICharValidator
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllowNegativeSign()
        {
            return ValidChar('-').ValidChars(CharValidator.NegativeSign);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllowPositiveSign()
        {
            return ValidChars("+").ValidChars(CharValidator.PositiveSign);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllowDigits()
        {
            return ValidChars("0123456789");
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllowNativeDigits()
        {
            return ValidChars(CharValidator.NativeDigits);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllowHexDigits()
        {
            return AllowDigits().ValidChars('a', 'f').ValidChars('A', 'F');
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllCategoriesBad()
        {
            return AllCategoriesValid(false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllCharsBad()
        {
            return AllCharsValid(false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadCategories(params UnicodeCategory[] cat)
        {
            return ValidCategories(cat, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadCategory(UnicodeCategory cat)
        {
            return ValidCategory(cat, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadChar(char ch)
        {
            return ValidChar(ch, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadChars(params char[] ch)
        {
            return ValidChars(ch, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator ValidChars(params char[] ch)
        {
            return ValidChars(ch, true);
        }

        /// <inheritdoc/>
        public ICharValidator ValidChars(char[] ch, bool valid)
        {
            for (int i = 0; i < ch.Length; i++)
                ValidChar(ch[i], valid);
            return this;
        }

        /// <inheritdoc/>
        public ICharValidator ValidChars(string[] s, bool valid = true)
        {
            if (s is null)
                return this;
            for (int i = 0; i < s.Length; i++)
                ValidChars(s[i], valid);
            return this;
        }

        /// <inheritdoc/>
        public ICharValidator ValidChars(string? s, bool valid = true)
        {
            if (s is null)
                return this;
            for (int i = 0; i < s.Length; i++)
                ValidChar(s[i], valid);
            return this;
        }

        /// <inheritdoc/>
        public ICharValidator ValidRange(char minCh, char maxCh, bool valid = true)
        {
            for (char i = minCh; i <= maxCh; i++)
            {
                ValidChar(i, valid);
            }

            return this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadRange(char minCh, char maxCh)
        {
            return ValidRange(minCh, maxCh, false);
        }

        /// <inheritdoc/>
        public ICharValidator ValidCategories(UnicodeCategory[] cat, bool valid)
        {
            for (int i = 0; i < cat.Length; i++)
                ValidCategory(cat[i], valid);
            return this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator ValidCategories(params UnicodeCategory[] cat)
        {
            return ValidCategories(cat, true);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadChars(string? s)
        {
            return ValidChars(s, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValid(char ch)
        {
            return IsValidCategory(ch) && IsValidChar(ch);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidCategory(char ch)
        {
            return IsValidCategory(CharUnicodeInfo.GetUnicodeCategory(ch));
        }

        /// <inheritdoc/>
        public abstract ICharValidator AllCategoriesValid(bool valid = true);

        /// <inheritdoc/>
        public abstract ICharValidator Reset();

        /// <inheritdoc/>
        public abstract ICharValidator ValidCategory(UnicodeCategory cat, bool valid = true);

        /// <inheritdoc/>
        public abstract bool IsValidCategory(UnicodeCategory cat);

        /// <inheritdoc/>
        public abstract bool IsValidChar(char ch);

        /// <inheritdoc/>
        public abstract ICharValidator ValidChar(char ch, bool valid = true);

        /// <inheritdoc/>
        public abstract ICharValidator AllCharsValid(bool valid = true);
    }
}