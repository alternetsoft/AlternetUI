using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    internal class CharValidator : ImmutableObject, ICharValidator
    {
        private BitArray[]? charInfo;
        private BitArray64 catInfo = new(true);

        internal BitArray[] CharInfo
        {
            get
            {
                return charInfo ??= new BitArray[ushort.MaxValue + 1];
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator AllCategoriesBad()
        {
            return AllCategoriesValid(false);
        }

        /// <inheritdoc/>
        public ICharValidator AllCategoriesValid(bool valid = true)
        {
            Reset();
            catInfo.SetAllBits(valid);
            return this;
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
        public ICharValidator BadChars(char[] ch, bool isValid)
        {
            return ValidChars(ch, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadChars(string s)
        {
            return ValidChars(s, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidCategory(char ch)
        {
            return IsValidCategory(CharUnicodeInfo.GetUnicodeCategory(ch));
        }

        /// <inheritdoc/>
        public ICharValidator Reset()
        {
            charInfo = null;
            catInfo.SetAllBits();
            return this;
        }

        /// <inheritdoc/>
        public ICharValidator ValidCategories(UnicodeCategory[] cat, bool isValid)
        {
            for (int i = 0; i < cat.Length; i++)
                ValidCategory(cat[i], isValid);
            return this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator ValidCategories(params UnicodeCategory[] cat)
        {
            return ValidCategories(cat, true);
        }

        /// <inheritdoc/>
        public ICharValidator ValidCategory(UnicodeCategory cat, bool isValid = true)
        {
            catInfo[(int)cat] = isValid;
            return this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator ValidChars(params char[] ch)
        {
            return ValidChars(ch, true);
        }

        /// <inheritdoc/>
        public ICharValidator ValidChars(char[] ch, bool isValid)
        {
            for (int i = 0; i < ch.Length; i++)
                ValidChar(ch[i], isValid);
            return this;
        }

        /// <inheritdoc/>
        public ICharValidator ValidChars(string s, bool isValid = true)
        {
            for (int i = 0; i < s.Length; i++)
                ValidChar(s[i], isValid);
            return this;
        }

        /// <inheritdoc/>
        public ICharValidator ValidChars(char minCh, char maxCh, bool isValid = true)
        {
            for(char i = minCh; i <= maxCh; i++)
            {
                ValidChar(i, isValid);
            }

            return this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ICharValidator BadChars(char minCh, char maxCh, bool isValid = true)
        {
            return ValidChars(minCh, maxCh, false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidCategory(UnicodeCategory cat)
        {
            var result = catInfo[(int)cat];
            return result;
        }

        /// <inheritdoc/>
        public bool IsValidChar(char ch)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ICharValidator ValidChar(char ch, bool isValid = true)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ICharValidator AllCharsValid(bool valid = true)
        {
            throw new NotImplementedException();
        }
    }
}
