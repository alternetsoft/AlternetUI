using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to <see cref="Action"/>, <see cref="Func{TResult}"/>
    /// and their versions with additional parameters.
    /// </summary>
    public static class ActionUtils
    {
        /// <summary>
        /// Converts <see cref="Func{T, TResult}"/> with <see cref="object"/> parameter and
        /// <see cref="int"/> result type to <see cref="System.Action{T}"/>.
        /// </summary>
        /// <param name="func">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T> ToAction<T>(Func<object, int> func)
        {
            void Fn(T s)
            {
                func(s!);
            }

            return Fn;
        }
    }
}
