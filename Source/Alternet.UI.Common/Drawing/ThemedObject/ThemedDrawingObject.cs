using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines an interface for a two-value class that has different values for light and dark themes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IThemedDrawingObject<T>
    {
        /// <summary>
        /// Gets dark value.
        /// </summary>
        T Dark { get; }

        /// <summary>
        /// Gets light value.
        /// </summary>
        T Light { get; }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> value depending on system settings.
        /// </summary>
        /// <returns>The value to be used for the current system theme.</returns>
        public T GetValue()
        {
            if (SystemSettings.AppearanceIsDark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> value depending on system settings and control color mode.
        /// </summary>
        /// <param name="control">The control for which to get the value.</param>
        /// <returns>The value to be used for the specified control.</returns>
        public T GetValue(AbstractControl control)
        {
            var colorMode = control.ColorMode;

            if (colorMode is null)
            {
                return GetValue();
            }

            if (colorMode.Value == ControlColorMode.Dark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> value depending on
        /// <paramref name="isDark"/> parameter value.
        /// </summary>
        /// <param name="isDark">Whether to get dark or light value.</param>
        /// <returns>The value to be used for the specified theme.</returns>
        public T GetValue(bool isDark)
        {
            if (isDark)
                return Dark;
            else
                return Light;
        }
    }

    /// <summary>
    /// Represents a two-value class that has different values for light and dark themes.
    /// </summary>
    public partial class ThemedDrawingObject<T> : ImmutableObject, IThemedDrawingObject<T>
    {
        private T light;
        private T dark;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingObject{T}"/> class
        /// with the same light and dark values.
        /// </summary>
        /// <param name="value">Light and dark value.</param>
        public ThemedDrawingObject(T value)
        {
            this.light = value;
            this.dark = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingObject{T}"/> class
        /// with the specified light and dark values.
        /// </summary>
        /// <param name="light">Light value.</param>
        /// <param name="dark">Dark value.</param>
        public ThemedDrawingObject(T light, T dark)
        {
            this.light = light;
            this.dark = dark;
        }

        /// <summary>
        /// Gets or sets dark value.
        /// </summary>
        public virtual T Dark
        {
            get
            {
                return dark;
            }

            set
            {
                SetNotNullProperty(ref dark, value);
            }
        }

        /// <summary>
        /// Gets or sets light value.
        /// </summary>
        public virtual T Light
        {
            get
            {
                return light;
            }

            set
            {
                SetNotNullProperty(ref light, value);
            }
        }

        /// <summary>
        /// Sets the light and dark values of this instance to the specified values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        public virtual void SetValues(T light, T dark)
        {
            DoInsideSuspendedPropertyChanged(() =>
            {
                Light = light;
                Dark = dark;
            });
        }

        /// <summary>
        /// Sets the light and dark values of this instance to the specified value, making both values the same.
        /// </summary>
        /// <param name="value">The value to use for both the light and dark theme variants.</param>
        public virtual void SetValues(T value)
        {
            DoInsideSuspendedPropertyChanged(() =>
            {
                Light = value;
                Dark = value;
            });
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the
        /// current object; otherwise, <c>false</c>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is ThemedDrawingObject<T> other && Equals(other);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return (Dark, Light).GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of
        /// the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ThemedDrawingObject<T>? other)
        {
            if (other is null)
                return false;
            return this == other;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> value depending on system settings.
        /// </summary>
        /// <returns>The value to be used for the current system theme.</returns>
        public virtual T GetValue()
        {
            if (SystemSettings.AppearanceIsDark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> value depending on system settings and control color mode.
        /// </summary>
        /// <param name="control">The control for which to get the value.</param>
        /// <returns>The value to be used for the specified control.</returns>
        public virtual T GetValue(AbstractControl control)
        {
            var colorMode = control.ColorMode;

            if (colorMode is null)
            {
                return GetValue();
            }

            if (colorMode.Value == ControlColorMode.Dark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> value depending on
        /// <paramref name="isDark"/> parameter value.
        /// </summary>
        /// <param name="isDark">Whether to get dark or light value.</param>
        /// <returns>The value to be used for the specified theme.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual T GetValue(bool isDark)
        {
            if (isDark)
                return Dark;
            else
                return Light;
        }
    }
}
