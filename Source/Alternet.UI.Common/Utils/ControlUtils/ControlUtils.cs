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
        /// Retrieves the parent platform-specific user control
        /// of the specified <see cref="AbstractControl"/>.
        /// </summary>
        /// <remarks>A platform-specific user control is identified
        /// as an <see cref="AbstractControl"/> that has
        /// <see cref="AbstractControl.IsPlatformControl"/> set to
        /// <see langword="true"/> and is of type <see cref="UserControl"/>.
        /// The method traverses the parent hierarchy of the
        /// specified <paramref name="control"/> to locate such a control.</remarks>
        /// <param name="control">The <see cref="AbstractControl"/> for which
        /// to find the parent platform-specific <see cref="UserControl"/>.
        /// Can be <see langword="null"/>.</param>
        /// <returns>The parent platform-specific <see cref="UserControl"/> if found;
        /// otherwise, <see langword="null"/>.</returns>
        public static UserControl? GetParentPlatformUserControl(AbstractControl? control)
        {
            if (control is null)
                return null;

            AbstractControl? overlayParent = control.Parent;
            while (true)
            {
                if (overlayParent is null)
                    break;
                if (overlayParent.IsPlatformControl && overlayParent is UserControl userControl)
                    return userControl;

                overlayParent = overlayParent.Parent;
            }

            return null;
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
                    },
                    true);

                if (result is not null)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets control for measure purposes in a safe way. Do not change
        /// any properties of the returned control.
        /// </summary>
        /// <param name="obj">The object which is returned if it is a control.</param>
        /// <returns></returns>
        public static AbstractControl SafeControl(object? obj)
        {
            if (obj is AbstractControl control)
                return control;
            return App.SafeWindow ?? Empty;
        }

        /// <summary>
        /// Increases width or height specified in <paramref name="currentValue"/>
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
        /// Increases size specified in <paramref name="currentSize"/>
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
