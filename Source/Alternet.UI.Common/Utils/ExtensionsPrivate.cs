using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Extensions
{
    public static class ExtensionsPrivate
    {
        /// <summary>
        /// Gets whether <see cref="DockStyle"/> equals <see cref="DockStyle.Top"/> or
        /// <see cref="DockStyle.Bottom"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTopOrBottom(this DockStyle dock)
        {
            return dock == DockStyle.Bottom || dock == DockStyle.Top;
        }

        /// <summary>
        /// Calls <see cref="Stack{T}.Push"/> for the each item in <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Type of the item</typeparam>
        /// <param name="stack"><see cref="Stack{T}"/> instance.</param>
        /// <param name="items">Items to push.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
                stack.Push(item);
        }

        /// <summary>
        /// Removes underscore characters ('_') from string.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveUnderscore(this string s)
        {
            return s.Replace("_", string.Empty);
        }

        /// <summary>
        /// Trims end of line characters from the string.
        /// </summary>
        /// <param name="s">String.</param>
        /// <returns>String without all end of line characters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimEndEol(this string s)
        {
            return s.TrimEnd('\r', '\n');
        }

        /// <summary>
        /// Gets whether <see cref="DockStyle"/> equals <see cref="DockStyle.Left"/> or
        /// <see cref="DockStyle.Right"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLeftOrRight(this DockStyle dock)
        {
            return dock == DockStyle.Left || dock == DockStyle.Right;
        }
    }
}
