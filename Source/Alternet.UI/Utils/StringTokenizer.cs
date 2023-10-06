using System;
using System.Globalization;

namespace Alternet.UI
{
    internal struct StringTokenizer : IDisposable
    {
        private const char DefaultSeparatorChar = ',';

        private readonly string mStr;
        private readonly int mLength;
        private readonly char mSeparator;
        private readonly string? mExceptionMessage;
        private readonly IFormatProvider mFormatProvider;
        private int mIndex;
        private int mtokenIndex;
        private int mTokenLength;

        public StringTokenizer(string s, IFormatProvider formatProvider, string? exceptionMessage = null)
            : this(s, GetSeparatorFromFormatProvider(formatProvider), exceptionMessage)
        {
            mFormatProvider = formatProvider;
        }

        public StringTokenizer(string s, char separator = DefaultSeparatorChar, string? exceptionMessage = null)
        {
            mStr = s ?? throw new ArgumentNullException(nameof(s));
            mLength = s?.Length ?? 0;
            mSeparator = separator;
            mExceptionMessage = exceptionMessage;
            mFormatProvider = CultureInfo.InvariantCulture;
            mIndex = 0;
            mtokenIndex = -1;
            mTokenLength = 0;

            while (mIndex < mLength && char.IsWhiteSpace(mStr, mIndex))
            {
                mIndex++;
            }
        }

        public readonly string? CurrentToken =>
            mtokenIndex < 0 ? null : mStr.Substring(mtokenIndex, mTokenLength);

        public readonly void Dispose()
        {
            if (mIndex != mLength)
            {
                throw GetFormatException();
            }
        }

        public bool TryReadInt32(out int result, char? separator = null)
        {
            if (TryReadString(out var stringResult, separator) &&
                int.TryParse(stringResult, NumberStyles.Integer, mFormatProvider, out result))
            {
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public int ReadInt32(char? separator = null)
        {
            if (!TryReadInt32(out var result, separator))
            {
                throw GetFormatException();
            }

            return result;
        }

        public bool TryReadDouble(out double result, char? separator = null)
        {
            if (TryReadString(out var stringResult, separator) &&
                double.TryParse(stringResult, NumberStyles.Float, mFormatProvider, out result))
            {
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public double ReadDouble(char? separator = null)
        {
            if (!TryReadDouble(out var result, separator))
            {
                throw GetFormatException();
            }

            return result;
        }

        public bool TryReadSingle(out double result, char? separator = null)
        {
            if (TryReadString(out var stringResult, separator) &&
                double.TryParse(stringResult, NumberStyles.Float, mFormatProvider, out result))
            {
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public double ReadSingle(char? separator = null)
        {
            if (!TryReadSingle(out var result, separator))
            {
                throw GetFormatException();
            }

            return result;
        }

        public bool TryReadString(out string? result, char? separator = null)
        {
            var success = TryReadToken(separator ?? mSeparator);
            result = CurrentToken;
            return success;
        }

        public string ReadString(char? separator = null)
        {
            if (!TryReadString(out var result, separator))
            {
                throw GetFormatException();
            }

            if (result == null)
                throw new InvalidOperationException();

            return result;
        }

        private static char GetSeparatorFromFormatProvider(IFormatProvider provider)
        {
            var c = DefaultSeparatorChar;

            var formatInfo = NumberFormatInfo.GetInstance(provider);
            if (formatInfo.NumberDecimalSeparator.Length > 0
                && c == formatInfo.NumberDecimalSeparator[0])
            {
                c = ';';
            }

            return c;
        }

        private bool TryReadToken(char separator)
        {
            mtokenIndex = -1;

            if (mIndex >= mLength)
            {
                return false;
            }

#pragma warning disable
            var c = mStr[mIndex];
#pragma warning restore

            var index = mIndex;
            var length = 0;

            while (mIndex < mLength)
            {
                c = mStr[mIndex];

                if (char.IsWhiteSpace(c) || c == separator)
                {
                    break;
                }

                mIndex++;
                length++;
            }

            SkipToNextToken(separator);

            mtokenIndex = index;
            mTokenLength = length;

            if (mTokenLength < 1)
            {
                throw GetFormatException();
            }

            return true;
        }

        private void SkipToNextToken(char separator)
        {
            if (mIndex < mLength)
            {
                var c = mStr[mIndex];

                if (c != separator && !char.IsWhiteSpace(c))
                {
                    throw GetFormatException();
                }

                var length = 0;

                while (mIndex < mLength)
                {
                    c = mStr[mIndex];

                    if (c == separator)
                    {
                        length++;
                        mIndex++;

                        if (length > 1)
                        {
                            throw GetFormatException();
                        }
                    }
                    else
                    {
                        if (!char.IsWhiteSpace(c))
                        {
                            break;
                        }

                        mIndex++;
                    }
                }

                if (length > 0 && mIndex >= mLength)
                {
                    throw GetFormatException();
                }
            }
        }

        private readonly FormatException GetFormatException() =>
            mExceptionMessage != null ? new FormatException(mExceptionMessage) : new FormatException();
     }
}