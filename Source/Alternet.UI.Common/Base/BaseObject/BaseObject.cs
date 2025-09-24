using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Encapsulates a method that has a single parameter and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the method that
    /// this delegate encapsulates.</typeparam>
    /// <param name="item">The parameter of the method that this delegate encapsulates.</param>
    /// <remarks>
    /// This delegate is different from <see cref="Action{T}"/> in the parameter definition.
    /// Here it is defined as "ref" parameter.
    /// </remarks>
    public delegate void ActionRef<T>(ref T item);

    /// <summary>
    /// Encapsulates a method that has two parameters and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this
    /// delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that
    /// this delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
    /// <remarks>
    /// This delegate is different from <see cref="Action{T1,T2}"/> in the first parameter definition.
    /// Here it is defined as "ref" parameter.
    /// </remarks>
    public delegate void ActionRef<T1, T2>(ref T1 arg1, T2 arg2);

    /// <summary>
    /// Encapsulates a method that has three parameters and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this
    /// delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that
    /// this delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that
    /// this delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param>
    /// <remarks>
    /// This delegate is different from <see cref="Action{T1,T2, T3}"/> in the
    /// first parameter definition.
    /// Here it is defined as "ref" parameter.
    /// </remarks>
    public delegate void ActionRef<T1, T2, T3>(ref T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Base class with properties and methods common to all Alternet.UI objects.
    /// </summary>
    public partial class BaseObject : IBaseObject
    {
        /// <summary>
        /// Gets or sets static options which are common to all
        /// descendants of <see cref="BaseObject"/>.
        /// </summary>
        public static GenericStaticOptions StaticOptions;

        /// <summary>
        /// Enumerates static options which customize behavior of all <see cref="BaseObject"/>
        /// descendants.
        /// </summary>
        [Flags]
        public enum GenericStaticOptions
        {
            /// <summary>
            /// Specifies that <see cref="object.ToString()"/> should not include property
            /// names in result. This is not supported in all the objects.
            /// </summary>
            NoNamesInToString,
        }

        /// <summary>
        /// Gets a value indicating whether the caller must call an invoke
        /// method when making method
        /// calls to the UI objects because the caller is not on the UI thread.
        /// </summary>
        public static bool InvokeRequired
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var result = !App.IsAppThread;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="object.ToString()"/> should not include property
        /// names in result.
        /// </summary>
        public static bool UseNamesInToString
        {
            get
            {
                return !StaticOptions.HasFlag(GenericStaticOptions.NoNamesInToString);
            }

            set
            {
                if (value)
                    StaticOptions &= ~GenericStaticOptions.NoNamesInToString;
                else
                    StaticOptions |= GenericStaticOptions.NoNamesInToString;
            }
        }

        /// <summary>
        /// Gets whether object is immutable (properties can not be changed).
        /// </summary>
        [Browsable(false)]
        public virtual bool Immutable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether this object is disposing or disposed.
        /// Default implementation returns False. Inherit from <see cref="DisposableObject"/>
        /// or override this property in order to have valid result.
        /// </summary>
        [Browsable(false)]
        public virtual bool DisposingOrDisposed
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Safely disposes specified object which supports
        /// <see cref="IDisposable"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="disposable">Object to dispose.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeDispose<T>(ref T? disposable)
            where T : IDisposable
        {
            if (disposable is null)
                return;
            var t = disposable;
            disposable = default;
            t.Dispose();
        }

        /// <summary>
        /// Safely disposes of the specified disposable object, suppressing any exceptions
        /// that occur during disposal.
        /// </summary>
        /// <remarks>This method ensures that the disposal of the object does not propagate exceptions,
        /// which can be useful in scenarios where disposal failures should not
        /// disrupt the execution flow. Any
        /// exceptions encountered during disposal are logged using
        /// <see cref="Debug.WriteLine(string)"/>.</remarks>
        /// <typeparam name="T">The type of the disposable object, which must
        /// implement <see cref="IDisposable"/>.</typeparam>
        /// <param name="disposable">A reference to the disposable object to be disposed.
        /// The reference is set to <see langword="null"/> after
        /// disposal.</param>
        public static void SafeDisposeSuppressException<T>(ref T? disposable)
            where T : IDisposable
        {
            if (disposable is null)
                return;
            var t = disposable;
            disposable = default;
            try
            {
                t.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Executes a delegate asynchronously on the UI thread.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters
        /// of the same number and type that are contained in the args parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to the
        /// given method. This can be <c>null</c> if no arguments are needed.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result
        /// of the operation.</returns>
        /// <remarks>
        /// You can call this method from another non-ui thread with action
        /// which can perform operation on ui controls.
        /// </remarks>
        /// <example>
        /// private void StartCounterThread1()
        /// {
        ///    var thread1 = new Thread(() =>
        ///    {
        ///      for (int i = 0; ; i++)
        ///      {
        ///          BeginInvoke(() => beginInvokeCounterLabel.Text = i.ToString());
        ///          Thread.Sleep(1000);
        ///       }
        ///    })
        ///    { IsBackground = true };
        ///
        ///    thread1.Start();
        /// }
        /// </example>
        public static IAsyncResult BeginInvoke(Delegate? method, object?[]? args = null)
        {
            var handler = App.Handler ?? throw new InvalidOperationException();
            var invocation = new Invocation(method, args, synchronous: false);
            handler.BeginInvoke(invocation.GetAction());
            return invocation;
        }

        /// <summary>
        /// Retrieves the return value of the asynchronous operation represented
        /// by the <see cref="IAsyncResult"/> passed.
        /// </summary>
        /// <param name="result">The <see cref="IAsyncResult"/> that represents
        /// a specific invoke asynchronous operation, returned when calling
        /// <see cref="BeginInvoke"/>.</param>
        /// <returns>The <see cref="object"/> generated by the
        /// asynchronous operation.</returns>
        public static object? EndInvoke(IAsyncResult result)
        {
            if (result is not Invocation invocation)
                throw new ArgumentException("Invalid IAsyncResult.", nameof(result));

            result.AsyncWaitHandle.WaitOne();

            ExceptionUtils.Rethrow(invocation.Exception);

            return invocation.ReturnValue;
        }

        /// <summary>
        /// Executes the specified delegate, on the UI thread,
        /// with the specified list of arguments.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters of
        /// the same number and type that are contained in the
        /// <c>args</c> parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to
        /// the specified method. This parameter can be <c>null</c> if the
        /// method takes no arguments.</param>
        /// <returns>An <see cref="object"/> that contains the return value
        /// from the delegate being invoked, or <c>null</c> if the delegate has
        /// no return value.</returns>
        public static object? Invoke(Delegate? method, object?[]? args)
        {
            if (method is null)
                return null;
            if (InvokeRequired)
                return EndInvoke(BeginInvoke(method, args));
            else
                return method.DynamicInvoke(args);
        }

        /// <summary>
        /// Executes the specified action while displaying a busy cursor.
        /// </summary>
        /// <remarks>This method posts the action to be executed and ensures
        /// that a busy cursor is displayed during its execution.</remarks>
        /// <param name="action">The action to execute. If <see langword="null"/>,
        /// the method returns immediately without performing any action.</param>
        public static void PostAndBusyCursor(Action? action)
        {
            if (action is null)
                return;
            Post(() =>
            {
                App.DoInsideBusyCursor(action);
            });
        }

        /// <summary>
        /// Adds specified action to the message queue and executes it on the UI thread.
        /// </summary>
        public static void Post(Action? action)
        {
            if (action is null)
                return;

            var handler = App.Handler
                ?? throw new NullReferenceException("Application handler is null");
            handler.BeginInvoke(action);
        }

        /// <summary>
        /// Executes the specified action, on the UI thread.
        /// </summary>
        public static void Invoke(Action? action)
        {
            if (action is null)
                return;

            if (InvokeRequired)
            {
                var handler = App.Handler
                    ?? throw new NullReferenceException("Application handler is null");
                handler.BeginInvoke(action);
            }
            else
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// Gets whether the specified object is disposing or disposed.
        /// </summary>
        /// <param name="obj">Object to test.</param>
        /// <returns></returns>
        public static bool IsDisposingOrDisposed(object? obj)
        {
            if (obj is IDisposableObject disposable)
            {
                return disposable.DisposingOrDisposed;
            }

            return true;
        }

        /// <summary>
        /// Safely disposes specified object which supports
        /// <see cref="IDisposableObject"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="disposable">Object to dispose.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeDisposeObject<T>(ref T? disposable)
            where T : IDisposableObject
        {
            if (disposable is null)
                return;
            var t = disposable;
            disposable = default;
            if(!t.IsDisposed)
                t.Dispose();
        }

        /// <summary>
        /// Calls the specified function inside try catch block.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        /// <remarks>
        /// If exception is raised inside <paramref name="func"/>,
        /// exception is logged and <c>false</c> is returned.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T InsideTryCatch<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return default!;
            }
        }

        /// <summary>
        /// Calls the specified action inside try catch block.
        /// </summary>
        /// <param name="action">Action to call.</param>
        /// <returns></returns>
        /// <remarks>
        /// If exception is raised inside <paramref name="action"/>,
        /// exception is logged.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InsideTryCatch(Action? action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
            }
        }

        /// <summary>
        /// Calls the specified action inside try catch block silently.
        /// </summary>
        /// <param name="action">Action to call.</param>
        public static void TryCatchSilent(Action? action)
        {
            try
            {
                action?.Invoke();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Calls the specified action inside try catch block when DEBUG is specified; otherwise
        /// simply calls the action without try catch block.
        /// </summary>
        public static void InsideTryCatchIfDebug(Action? action)
        {
#if DEBUG
            InsideTryCatch(action);
#else
            action?.Invoke();
#endif
        }

        /// <summary>
        /// Wraps exception for the debug purposes.
        /// </summary>
        /// <param name="e">Exception.</param>
        /// <returns></returns>
        public static Exception WrapException(Exception e)
        {
            return e;
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public static void Nop()
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="exception"></param>
        public static void Nop(Exception? exception)
        {
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public virtual void Required()
        {
        }

        /// <summary>
        /// Throws <see cref="NotImplementedException"/> exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Browsable(false)]
        public object NotImplemented() => throw new NotImplementedException();

        /// <summary>
        /// Throws <see cref="NotImplementedException"/> exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Browsable(false)]
        public T NotImplemented<T>() => throw new NotImplementedException();

        /// <summary>
        /// Same as <see cref="App.Log"/>.
        /// </summary>
        /// <param name="s">Object to log.</param>
        public virtual void Log(object? s) => App.Log(s);

        /// <summary>
        /// Checks current thread on <see cref="ApartmentState.STA"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Raised if
        /// <see cref="Thread.GetApartmentState"/> is
        /// not <see cref="ApartmentState.STA"/>.</exception>
        /// <remarks>
        /// This method performs checks only on MSW, on other os it does nothing.
        /// </remarks>
        [Browsable(false)]
        public virtual void CheckSTARequirement()
        {
            if (!App.IsWindowsOS)
                return;

            // STA Requirement
            // Alternet UI doesn't necessarily require STA, but many components do.  Examples
            // include Cicero, OLE, COM, etc.  So we throw an exception here if the
            // thread is not STA.
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new InvalidOperationException(SR.Get(SRID.RequiresSTA));
            }
        }
    }
}
