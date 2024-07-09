using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    [DefaultMember("Item")]
    internal ref struct ValueStringBuilder
    {
        private char[]? farrayToReturnToPool;
        private Span<char> fchars;
        private int fpos;

        public ValueStringBuilder(Span<char> initialBuffer)
        {
            farrayToReturnToPool = null;
            fchars = initialBuffer;
            fpos = 0;
        }

        public int Length
        {
            readonly get
            {
                return fpos;
            }

            set
            {
                fpos = value;
            }
        }

        public override string ToString()
        {
            string result = fchars.Slice(0, fpos).ToString();
            Dispose();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            int pos = fpos;
            Span<char> chars = fchars;
            if ((uint)pos < (uint)chars.Length)
            {
                chars[pos] = c;
                fpos = pos + 1;
            }
            else
            {
                GrowAndAppend(c);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string s)
        {
            if (s != null)
            {
                int pos = fpos;
                if (s.Length == 1 && (uint)pos < (uint)fchars.Length)
                {
                    fchars[pos] = s[0];
                    fpos = pos + 1;
                }
                else
                {
                    AppendSlow(s);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            char[]? arrayToReturnToPool = farrayToReturnToPool;
            this = default;
            if (arrayToReturnToPool != null)
            {
                ArrayPool<char>.Shared.Return(arrayToReturnToPool);
            }
        }

        public void Append(ReadOnlySpan<char> value)
        {
            int pos = fpos;
            if (pos > fchars.Length - value.Length)
            {
                Grow(value.Length);
            }

            value.CopyTo(fchars.Slice(fpos));
            fpos += value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<char> AppendSpan(int length)
        {
            int pos = fpos;
            if (pos > fchars.Length - length)
            {
                Grow(length);
            }

            fpos = pos + length;
            return fchars.Slice(pos, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowAndAppend(char c)
        {
            Grow(1);
            Append(c);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int additionalCapacityBeyondPos)
        {
            int minimumLength = (int)Math.Max((uint)(fpos + additionalCapacityBeyondPos), Math.Min((uint)(fchars.Length * 2), 2147483591u));
            char[] array = ArrayPool<char>.Shared.Rent(minimumLength);
            fchars.Slice(0, fpos).CopyTo(array);
            char[]? arrayToReturnToPool = farrayToReturnToPool;
            fchars = farrayToReturnToPool = array;
            if (arrayToReturnToPool != null)
            {
                ArrayPool<char>.Shared.Return(arrayToReturnToPool);
            }
        }

        private void AppendSlow(string s)
        {
            int pos = fpos;
            if (pos > fchars.Length - s.Length)
            {
                Grow(s.Length);
            }

            s.CopyToSpan(fchars.Slice(pos));
            fpos += s.Length;
        }
    }
}
