using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which are <see cref="AbstractControl"/> related.
    /// </summary>
    public static class ControlUtils
    {
        private static Control? empty;

        /// <summary>
        /// Gets an empty control for the debug purposes.
        /// </summary>
        public static Control Empty
        {
            get
            {
                return empty ??= new Control();
            }
        }

        /// <summary>
        /// Finds visible control of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the control to find.</typeparam>
        /// <returns></returns>
        public static T? FindVisibleControl<T>()
            where T : AbstractControl
        {
            var windows = App.Current.LastActivatedWindows;

            if (AssemblyUtils.TypeEqualsOrDescendant(typeof(T), typeof(Window)))
            {
                return windows.FirstOrDefault() as T;
            }

            T? result = null;

            foreach(var window in windows)
            {
                window.ForEachVisibleChild(
                    (control) =>
                    {
                        if (control is T tt)
                            result = tt;
                    }, true);

                if (result is not null)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets control for measure purposes in a safe way. Do not change
        /// any properties of the returned control.
        /// </summary>
        /// <param name="obj">The object which is returrned if it is a control.</param>
        /// <returns></returns>
        public static AbstractControl SafeControl(object? obj)
        {
            if (obj is AbstractControl control)
                return control;
            return App.SafeWindow ?? Empty;
        }

        /// <summary>
        /// Increazes width or height specified in <paramref name="currentValue"/>
        /// if it is less than value specified in <paramref name="minValueAtLeast"/>.
        /// </summary>
        /// <param name="currentValue">Current width or height.</param>
        /// <param name="minValueAtLeast">Minimal value of the result will be at least.</param>
        /// <returns></returns>
        public static Coord GrowCoord(Coord? currentValue, Coord minValueAtLeast)
        {
            minValueAtLeast = Math.Max(0, minValueAtLeast);

            if (currentValue is null || currentValue <= 0)
                return minValueAtLeast;
            var result = Math.Max(currentValue.Value, minValueAtLeast);
            return result;
        }

        /// <summary>
        /// Increazes size specified in <paramref name="currentSize"/>
        /// if it is less than size specified in <paramref name="minSizeAtLeast"/>.
        /// Width and height are increased individually.
        /// </summary>
        /// <param name="currentSize">Current size value.</param>
        /// <param name="minSizeAtLeast">Minimal size of the result will be at least.</param>
        /// <returns></returns>
        public static SizeD GrowSize(SizeD? currentSize, SizeD minSizeAtLeast)
        {
            var newWidth = GrowCoord(currentSize?.Width, minSizeAtLeast.Width);
            var newHeight = GrowCoord(currentSize?.Height, minSizeAtLeast.Height);
            return (newWidth, newHeight);
        }
    }
}
