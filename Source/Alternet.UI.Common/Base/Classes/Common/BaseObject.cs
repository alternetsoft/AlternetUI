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
    /// <remarks>
    /// This delegate is different from <see cref="Action{T1,T2, T3}"/> in the first parameter definition.
    /// Here it is defined as "ref" parameter.
    /// </remarks>
    public delegate void ActionRef<T1, T2, T3>(ref T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// Base class with properties and methods common to all Alternet.UI objects.
    /// </summary>
    public partial class BaseObject : IBaseObject
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeDispose<T>(ref T? disposable) where T: IDisposable
        {
            var t = disposable;
            disposable = default!;
            t?.Dispose();
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
        public static bool InsideTryCatch(Func<bool> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual void Required()
        {
        }

        /// <summary>
        /// Throws <see cref="NotImplementedException"/> exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object NotImplemented() => throw new NotImplementedException();

        /// <summary>
        /// Throws <see cref="NotImplementedException"/> exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// <see cref="Thread.GetApartmentState"/> is not <see cref="ApartmentState.STA"/>.</exception>
        /// <remarks>
        /// This method performs checks only on MSW, on other os it does nothing.
        /// </remarks>
        protected void CheckSTARequirement()
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
