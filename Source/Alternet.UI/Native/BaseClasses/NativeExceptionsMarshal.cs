using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Alternet.UI.Native
{
    class NativeExceptionsMarshal
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public delegate void NativeExceptionCallbackType(
            ExceptionType exceptionType,
            [MarshalAs(UnmanagedType.LPWStr)]
            string message,
            int errorCode);

        public enum ExceptionType
        {
            ExternalException,
            InvalidOperationException,
            FormatException,
            ArgumentNullException,
            ThreadStateException,
            ArgumentException,
        }

        public static void OnUnhandledNativeException(ExceptionType exceptionType, string message, int errorCode) =>
            throw GetException(exceptionType, message, errorCode);

        public static void OnCaughtNativeException(ExceptionType exceptionType, string message, int errorCode) =>
            UI.Application.Current.OnThreadException(GetException(exceptionType, message, errorCode));

        public static ExceptionType GetExceptionType(Exception exception) =>
            exception switch
            {
                InvalidOperationException _ => ExceptionType.InvalidOperationException,
                FormatException _ => ExceptionType.FormatException,
                ArgumentNullException _ => ExceptionType.ArgumentNullException,
                ThreadStateException _ => ExceptionType.ThreadStateException,
                ArgumentException _ => ExceptionType.ArgumentException,
                _ => ExceptionType.ExternalException
            };

        static Exception GetException(ExceptionType exceptionType, string message, int errorCode) =>
            exceptionType switch
            {
                ExceptionType.ExternalException => new ExternalException(message, errorCode),
                ExceptionType.InvalidOperationException => new InvalidOperationException(message),
                ExceptionType.FormatException => new FormatException(message),
                ExceptionType.ArgumentNullException => new ArgumentNullException(message),
                ExceptionType.ThreadStateException => new ThreadStateException(message),
                ExceptionType.ArgumentException => new ArgumentException(message),
                _ => new ExternalException(message, errorCode),
            };
    }
}