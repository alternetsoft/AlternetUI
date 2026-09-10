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
    /// Represents a two-color class that has different values for light and dark themes.
    /// </summary>
    [DebuggerDisplay("{DebugString}")]
    public partial class LightDarkColor : ImmutableObject, IEquatable<LightDarkColor>
    {
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
        /// with the same light and dark colors.
        /// </summary>
        /// <param name="r">Red component of the color.</param>
        /// <param name="g">Green component of the color.</param>
        /// <param name="b">Blue component of the color.</param>
        public LightDarkColor(byte r, byte g, byte b)
            : this(new Color(r, g, b))
        {
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
        public LightDarkColor(ColorStruct light, ColorStruct dark)
        {
            this.light = new Color(light);
            this.dark = new Color(dark);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class
        /// with the specified light and dark color values.
        /// </summary>
        /// <param name="light">Light color.</param>
        /// <param name="dark">Dark color.</param>
        public LightDarkColor(Color light, Color dark)
        {
            this.light = light;
            this.dark = dark;
        }

        /// <summary>
        /// Gets or sets dark color.
        /// </summary>
        public virtual Color Dark
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
        /// Gets or sets light color.
        /// </summary>
        public virtual Color Light
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

        /// <inheritdoc/>
        public virtual string DebugString
        {
            get
            {
                return $"(Light: {Light.DebugString}, Dark: {Dark.DebugString})";
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
        /// Initializes a new instance of the LightDarkColor class with the specified light and dark color values.
        /// </summary>
        /// <param name="light">The color value to use for the light theme variant.</param>
        /// <param name="dark">The color value to use for the dark theme variant.</param>
        public static LightDarkColor FromColorStruct(ColorStruct light, ColorStruct dark)
        {
            return new(new Color(light), new Color(dark));
        }

        /// <summary>
        /// Creates a new LightDarkColor instance with the specified light and dark color values and marks it as immutable.
        /// </summary>
        /// <param name="light">The color value to use for the light theme variant.</param>
        /// <param name="dark">The color value to use for the dark theme variant.</param>
        /// <returns>A new LightDarkColor instance with the specified light and dark color values marked as immutable.</returns>
        public static LightDarkColor CreateImmutable(ColorStruct light, ColorStruct dark)
        {
            var result = new LightDarkColor(new Color(light), new Color(dark));
            result.SetImmutable();
            return result;
        }

        /// <summary>
        /// Creates a new LightDarkColor instance with the specified light and dark color values and marks it as immutable.
        /// </summary>
        /// <param name="light">The color value to use for the light theme variant.</param>
        /// <param name="dark">The color value to use for the dark theme variant.</param>
        /// <returns>A new LightDarkColor instance with the specified light and dark color values marked as immutable.</returns>
        public static LightDarkColor CreateImmutable(Color light, Color dark)
        {
            var result = new LightDarkColor(light, dark);
            result.SetImmutable();
            return result;
        }

        /// <summary>
        /// Sets the light and dark color values of this instance to the specified values.
        /// </summary>
        /// <param name="light">The color value to use for the light theme variant.</param>
        /// <param name="dark">The color value to use for the dark theme variant.</param>
        public virtual void SetColors(Color light, Color dark)
        {
            DoInsideSuspendedPropertyChanged(() =>
            {
                Light = light;
                Dark = dark;
            });
        }

        /// <summary>
        /// Calculates and returns a new color pair with the light and dark colors adjusted
        /// according to the specified operations.
        /// </summary>
        /// <param name="lightOp">The color adjustment operation to apply to the light color.
        /// Determines how the light color is modified.</param>
        /// <param name="darkOp">The color adjustment operation to apply to the dark color.
        /// Determines how the dark color is modified.</param>
        /// <returns>A new LightDarkColor instance containing the adjusted light and dark colors after applying the specified
        /// operations.</returns>
        public LightDarkColor GetAdjustedColorPair(ColorAdjustmentOperation lightOp, ColorAdjustmentOperation darkOp)
        {
            return new LightDarkColor(Light.GetAdjustedColor(lightOp), Dark.GetAdjustedColor(darkOp));
        }

        /// <summary>
        /// Gets a <see cref="LightDarkColor"/> with light and dark colors that are lighter
        /// than the current colors.
        /// </summary>
        /// <returns>A new <see cref="LightDarkColor"/> instance with lighter colors.</returns>
        public LightDarkColor LighterPair()
        {
            return new LightDarkColor(Light.Lighter(), Dark.Lighter());
        }

        /// <summary>
        /// Creates a new LightDarkColor instance with both the light and dark colors set to their 2x lighter variants.
        /// </summary>
        /// <returns>A LightDarkColor object whose Light and Dark properties are the 2x lighter versions of the current Light and
        /// Dark colors.</returns>
        public LightDarkColor LighterLighterPair()
        {
            return new LightDarkColor(Light.LighterLighter(), Dark.LighterLighter());
        }

        /// <summary>
        /// Creates a new LightDarkColor instance with both the light and dark colors set to their 2x darker variants.
        /// </summary>
        /// <returns>A LightDarkColor object whose Light and Dark properties are the 2x darker versions of the current Light and
        /// Dark colors.</returns>
        public LightDarkColor DarkerDarkerPair()
        {
            return new LightDarkColor(Light.DarkerDarker(), Dark.DarkerDarker());
        }

        /// <summary>
        /// Creates a new LightDarkColor instance that represents a darker version of the current color.
        /// </summary>
        /// <returns>A LightDarkColor object whose light and dark components
        /// are each darkened compared to the current instance.</returns>
        public LightDarkColor DarkerPair()
        {
            return new LightDarkColor(Light.Darker(), Dark.Darker());
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

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> color depending on system settings.
        /// </summary>
        /// <returns>The color to be used for the current system theme.</returns>
        public virtual Color LightOrDark()
        {
            if (SystemSettings.AppearanceIsDark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> color depending on system settings and control color mode.
        /// </summary>
        /// <param name="control">The control for which to get the color.</param>
        /// <returns>The color to be used for the specified control.</returns>
        public virtual Color LightOrDark(AbstractControl control)
        {
            var colorMode = control.ColorMode;

            if (colorMode is null)
            {
                return LightOrDark();
            }

            if (colorMode.Value == ControlColorMode.Dark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> or <see cref="Light"/> color depending on
        /// <paramref name="isDark"/> parameter value.
        /// </summary>
        /// <param name="isDark">Whether to get dark or light color.</param>
        /// <returns>The color to be used for the specified theme.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Color LightOrDark(bool isDark)
        {
            if (isDark)
                return Dark;
            else
                return Light;
        }

        /// <summary>
        /// Sets this instance as immutable and returns it. After calling this method, the instance cannot be modified.
        /// </summary>
        /// <returns>The current instance.</returns>
        public new LightDarkColor SetImmutable()
        {
            base.SetImmutable();
            return this;
        }
    }
}
