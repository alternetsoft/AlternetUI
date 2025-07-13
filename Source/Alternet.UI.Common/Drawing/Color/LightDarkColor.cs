using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Extends <see cref="Color"/> with additional features.
    /// Implements light and dark color pair, one of them
    /// is used when argb of the <see cref="LightDarkColor"/> is requested. Returned argb
    /// depends on the value specified in <see cref="IsDarkOverride"/> and a result
    /// of <see cref="SystemSettings.IsUsingDarkBackground"/>.
    /// </summary>
    public class LightDarkColor : Color, IEquatable<LightDarkColor>
    {
        /// <summary>
        /// Gets or sets whether dark or light color is returned
        /// when argb of the color is requested. When this is null (default),
        /// <see cref="SystemSettings.AppearanceIsDark"/> is used in order to determine
        /// which argb to return.
        /// </summary>
        public static bool? IsDarkOverride;

        private Color light;
        private Color dark;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class
        /// with the same light and dark colors.
        /// </summary>
        /// <param name="value">Light and dark color value.</param>
        public LightDarkColor(Color value)
        {
            this.light = value;
            this.dark = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class
        /// with the light and dark colors assigned from <see cref="KnownSvgColor"/>.
        /// </summary>
        /// <param name="knownSvgColor">Known svg color identifier.</param>
        public LightDarkColor(KnownSvgColor knownSvgColor)
        {
            this.light = SvgColors.GetSvgColor(knownSvgColor, false);
            this.dark = SvgColors.GetSvgColor(knownSvgColor, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class
        /// with the specified light and dark color values.
        /// </summary>
        /// <param name="light">Light color.</param>
        /// <param name="dark">Dark color.</param>
        public LightDarkColor(Color light, Color dark)
            : base(0, StateFlags.ValueValid)
        {
            this.light = light;
            this.dark = dark;
        }

        /// <summary>
        /// Gets whether dark or light color is returned.
        /// </summary>
        public static bool IsUsingDarkColor
        {
            get
            {
                return IsDarkOverride ?? SystemSettings.IsUsingDarkBackground;
            }
        }

        /// <summary>
        /// Gets dark color.
        /// </summary>
        public Color Dark
        {
            get
            {
                return dark;
            }

            set
            {
                dark = value;
            }
        }

        /// <summary>
        /// Gets light color.
        /// </summary>
        public Color Light
        {
            get
            {
                return light;
            }

            set
            {
                light = value;
            }
        }

        /// <inheritdoc/>
        public override string DebugString
        {
            get
            {
                return $"(IsUsingDarkColor: {IsUsingDarkColor}," +
                    $" Light: {Light.ARGBWeb}, Dark: {Dark.ARGBWeb})";
            }
        }

        /// <summary>
        /// Tests whether two specified <see cref="LightDarkColor"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="LightDarkColor"/> that is to the left
        /// of the inequality operator.</param>
        /// <param name="right">The <see cref="LightDarkColor"/> that is to the right
        /// of the inequality operator.</param>
        /// <returns><c>true</c> if the two <see cref="LightDarkColor"/> structures
        /// are different; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(LightDarkColor? left, LightDarkColor? right) => !(left == right);

        /// <summary>
        /// Tests whether two specified <see cref="LightDarkColor"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="LightDarkColor"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="LightDarkColor"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="LightDarkColor"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LightDarkColor? left, LightDarkColor? right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Dark == right.Dark
                && left.Light == right.Light;
        }

        /// <summary>
        /// Calls the specified action inside the block which temporary changes
        /// value of the <see cref="IsDarkOverride"/> property.
        /// </summary>
        /// <param name="tempIsDarkOverride">Temporary value for the
        /// <see cref="IsDarkOverride"/> property.</param>
        /// <param name="action">Action to call.</param>
        public static void DoInsideTempIsDarkOverride(bool? tempIsDarkOverride, Action? action)
        {
            var savedOverride = IsDarkOverride;
            try
            {
                IsDarkOverride = tempIsDarkOverride;
                action?.Invoke();
            }
            finally
            {
                IsDarkOverride = savedOverride;
            }
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> color depending on
        /// <paramref name="isDark"/> parameter value.
        /// </summary>
        /// <param name="isDark">Whether to get dark or light color.</param>
        /// <returns></returns>
        public Color LightOrDark(bool isDark)
        {
            if (isDark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the
        /// current object; otherwise, <c>false</c>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is LightDarkColor other && Equals(other);

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
        public bool Equals(LightDarkColor? other)
        {
            if (other is null)
                return false;
            return this == other;
        }

        /// <inheritdoc/>
        protected override void RequireArgb(ref ColorStruct val)
        {
            if (IsUsingDarkColor)
                val = dark.AsStruct;
            else
                val = light.AsStruct;
        }
    }
}
