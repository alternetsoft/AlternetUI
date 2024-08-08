using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the operating system and processor.
    /// </summary>
    public static class OSUtils
    {
        /// <summary>
        /// Adds an extension to the specified native module name. The extension depends on the
        /// operating system.
        /// </summary>
        /// <param name="nativeModuleNameNoExt">Native module name without an extension.</param>
        /// <returns></returns>
        public static string GetNativeModuleName(string nativeModuleNameNoExt)
        {
            var result = nativeModuleNameNoExt;

            if (App.IsWindowsOS)
            {
                result = $"{result}.dll";
            }
            else
            if (App.IsLinuxOS || App.IsAndroidOS)
            {
                result = $"{result}.so";
            }
            else
            if (App.IsMacOS || App.IsIOS)
            {
                result = $"{result}.dylib";
            }

            return result;
        }

        /// <summary>
        /// Suspends the execution of the current thread for a specified interval.
        /// </summary>
        /// <param name="milliSeconds">Specifies time, in milliseconds, for which
        /// to suspend execution.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sleep(int milliSeconds)
        {
            Thread.Sleep(milliSeconds);
        }

        /// <summary>
        /// Retrieves the low-order word from the specified value.
        /// </summary>
        /// <param name="value">Specifies the value to be converted.</param>
        /// <returns>The return value is the low-order word of the specified value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short LoWord(IntPtr value)
        {
            return (short)value.ToInt32();
        }

        /// <summary>
        /// Retrieves the high-order word from the given value.
        /// </summary>
        /// <param name="value">Specifies the value to be converted.</param>
        /// <returns>The return value is the high-order word of the specified value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short HiWord(IntPtr value)
        {
            return (short)(value.ToInt32() >> 16);
        }

        /// <summary>
        /// Converts two bytes to <see cref="short"/> value.
        /// </summary>
        /// <param name="ch0">First byte.</param>
        /// <param name="ch1">Second byte.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short BytesToInt16(byte ch0, byte ch1)
        {
            int num = ch1;
            num |= ch0 << 8;
            return (short)num;
        }

        /// <summary>
        /// Converts four bytes to <see cref="int"/> value.
        /// </summary>
        /// <param name="ch0">First byte.</param>
        /// <param name="ch1">Second byte.</param>
        /// <param name="ch2">Third byte.</param>
        /// <param name="ch3">Fourth byte.</param>
        /// <returns></returns>
        public static int BytesToInt(byte ch0, byte ch1, byte ch2, byte ch3)
        {
            int num = ch0;
            num |= (int)((uint)ch1 << 8);
            num |= (int)((uint)ch2 << 16);
            return num | (int)((uint)ch3 << 24);
        }

        /// <summary>
        /// Converts four chars to <see cref="int"/> value.
        /// Only low byte of the <see cref="char"/> is used in the conversion.
        /// </summary>
        /// <param name="ch0">First character.</param>
        /// <param name="ch1">Second character.</param>
        /// <param name="ch2">Third character.</param>
        /// <param name="ch3">Fourth character.</param>
        /// <returns></returns>
        public static int CharsToInt(char ch0, char ch1, char ch2, char ch3)
        {
            int num = ch0;
            num |= (int)((uint)ch1 << 8);
            num |= (int)((uint)ch2 << 16);
            return num | (int)((uint)ch3 << 24);
        }
    }
}
