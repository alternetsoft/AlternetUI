// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing;

public static partial class ControlPaint
{
    /// <summary>
    ///  Logic copied from Windows sources to copy the lightening and darkening of colors.
    /// </summary>
    private readonly struct HLSColor : IEquatable<HLSColor>
    {
        private const int ShadowAdjustment = -333;
        private const int HighlightAdjustment = 500;
        private const int Range = 240;
        private const int HLSMax = Range;
        private const int RGBMax = 255;
        private const int Undefined = HLSMax * 2 / 3;

        private readonly int hue;
        private readonly int saturation;
        private readonly bool isSystemColorsControl;
        private readonly byte alpha;

        public HLSColor(Color color)
            : this(color.AsStruct, color.ToKnownColor() == SystemColors.Control.ToKnownColor())
        {
        }

        public HLSColor(ColorStruct color, bool isSystemColorsControl = false)
        {
            this.isSystemColorsControl = isSystemColorsControl;

            ColorStruct argb = (ColorStruct)color;
            int r = argb.R;
            int g = argb.G;
            int b = argb.B;
            alpha = argb.A;
            int rDelta, gDelta, bDelta;  // intermediate value: % of spread from max

            // calculate lightness
            int max = Math.Max(Math.Max(r, g), b);
            int min = Math.Min(Math.Min(r, g), b);
            int sum = max + min;

            Luminosity = ((sum * HLSMax) + RGBMax) / (2 * RGBMax);

            int dif = max - min;
            if (dif == 0)
            {
                // r=g=b --> achromatic case
                saturation = 0;
                hue = Undefined;
            }
            else
            {
                // chromatic case
                saturation = Luminosity <= (HLSMax / 2)
                    ? ((dif * HLSMax) + (sum / 2)) / sum
                    : ((dif * HLSMax) + (((2 * RGBMax) - sum) / 2)) / ((2 * RGBMax) - sum);

                rDelta = (((max - r) * (HLSMax / 6)) + (dif / 2)) / dif;
                gDelta = (((max - g) * (HLSMax / 6)) + (dif / 2)) / dif;
                bDelta = (((max - b) * (HLSMax / 6)) + (dif / 2)) / dif;

                if (r == max)
                {
                    hue = bDelta - gDelta;
                }
                else if (g == max)
                {
                    hue = (HLSMax / 3) + rDelta - bDelta;
                }
                else
                {
                    // B == cMax
                    hue = (2 * HLSMax / 3) + gDelta - rDelta;
                }

                if (hue < 0)
                {
                    hue += HLSMax;
                }

                if (hue > HLSMax)
                {
                    hue -= HLSMax;
                }
            }
        }

        public int Luminosity { get; }

        public byte Alpha => alpha;

        public static bool operator ==(HLSColor a, HLSColor b) => a.Equals(b);

        public static bool operator !=(HLSColor a, HLSColor b) => !a.Equals(b);

        public ColorStruct Darker(float percDarker)
        {
            if (!isSystemColorsControl)
            {
                int zeroLum = NewLuma(ShadowAdjustment, true);
                var result = ColorFromHLS(hue, zeroLum - (int)(zeroLum * percDarker), saturation);
                return result.WithAlpha(alpha);
            }
            else
            {
                // With the usual color scheme, ControlDark/DarkDark is not exactly
                // what we would otherwise calculate
                if (percDarker == 0.0f)
                {
                    return SystemColors.ControlDark.AsStruct.WithAlpha(alpha);
                }
                else if (percDarker == 1.0f)
                {
                    return SystemColors.ControlDarkDark.AsStruct.WithAlpha(alpha);
                }
                else
                {
                    ColorStruct dark = (ColorStruct)SystemColors.ControlDark;
                    ColorStruct darkDark = (ColorStruct)SystemColors.ControlDarkDark;

                    return new ColorStruct(
                        (byte)(dark.R - (byte)((dark.R - darkDark.R) * percDarker)),
                        (byte)(dark.G - (byte)((dark.G - darkDark.G) * percDarker)),
                        (byte)(dark.B - (byte)((dark.B - darkDark.B) * percDarker))).WithAlpha(alpha);
                }
            }
        }

        public override bool Equals(object? o)
        {
            if (o is not HLSColor hlsColor)
            {
                return false;
            }

            return Equals(hlsColor);
        }

        public bool Equals(HLSColor other)
        {
            return hue == other.hue
                && alpha == other.alpha
                && saturation == other.saturation
                && Luminosity == other.Luminosity
                && isSystemColorsControl == other.isSystemColorsControl;
        }

        public override int GetHashCode() => (hue, saturation, Luminosity, alpha).GetHashCode();

        public ColorStruct Lighter(float percentLighter)
        {
            if (isSystemColorsControl)
            {
                // With the usual color scheme, ControlLight/LightLight is not exactly
                // what we would otherwise calculate
                if (percentLighter == 0.0f)
                {
                    return SystemColors.ControlLight.AsStruct.WithAlpha(alpha);
                }
                else if (percentLighter == 1.0f)
                {
                    return SystemColors.ControlLightLight.AsStruct.WithAlpha(alpha);
                }
                else
                {
                    ColorStruct light = (ColorStruct)SystemColors.ControlLight;
                    ColorStruct lightLight = (ColorStruct)SystemColors.ControlLightLight;

                    return new ColorStruct(
                        (byte)(light.R - (byte)((light.R - lightLight.R) * percentLighter)),
                        (byte)(light.G - (byte)((light.G - lightLight.G) * percentLighter)),
                        (byte)(light.B - (byte)((light.B - lightLight.B) * percentLighter)))
                        .WithAlpha(alpha);
                }
            }
            else
            {
                int zeroLum = Luminosity;
                int oneLum = NewLuma(HighlightAdjustment, true);
                return ColorFromHLS(hue, zeroLum + (int)((oneLum - zeroLum) * percentLighter), saturation).WithAlpha(alpha);
            }
        }

        private static int NewLuma(int luminosity, int n, bool scale)
        {
            if (n == 0)
            {
                return luminosity;
            }

            if (scale)
            {
                return n > 0
                    ? (int)(((luminosity * (1000 - n)) + ((Range + 1L) * n)) / 1000)
                    : luminosity * (n + 1000) / 1000;
            }

            luminosity += (int)((long)n * Range / 1000);

            if (luminosity < 0)
            {
                return 0;
            }
            else if (luminosity > HLSMax)
            {
                return HLSMax;
            }

            return luminosity;
        }

        private static ColorStruct ColorFromHLS(int hue, int luminosity, int saturation)
        {
            byte r, g, b;
            int magic1, magic2;

            if (saturation == 0)
            {
                // achromatic case
                r = g = b = (byte)(luminosity * RGBMax / HLSMax);
                if (hue != Undefined)
                {
                    /* ERROR */
                }
            }
            else
            {
                // chromatic case
                if (luminosity <= (HLSMax / 2))
                {
                    magic2 = ((luminosity * (HLSMax + saturation)) + (HLSMax / 2)) / HLSMax;
                }
                else
                {
                    magic2 = luminosity + saturation - (((luminosity * saturation) + (HLSMax / 2)) / HLSMax);
                }

                magic1 = (2 * luminosity) - magic2;

                // get RGB, change units from HLSMax to RGBMax
                r = (byte)(((HueToRGB(magic1, magic2, hue + (HLSMax / 3)) * RGBMax) + (HLSMax / 2)) / HLSMax);
                g = (byte)(((HueToRGB(magic1, magic2, hue) * RGBMax) + (HLSMax / 2)) / HLSMax);
                b = (byte)(((HueToRGB(magic1, magic2, hue - (HLSMax / 3)) * RGBMax) + (HLSMax / 2)) / HLSMax);
            }

            return new ColorStruct(r, g, b);
        }

        private static int HueToRGB(int n1, int n2, int hue)
        {
            // range check: note values passed add/subtract thirds of range

            /* The following is redundant for WORD (unsigned int) */
            if (hue < 0)
            {
                hue += HLSMax;
            }

            if (hue > HLSMax)
            {
                hue -= HLSMax;
            }

            // return r, g, or b value from this sector
            if (hue < (HLSMax / 6))
            {
                return n1 + ((((n2 - n1) * hue) + (HLSMax / 12)) / (HLSMax / 6));
            }

            if (hue < (HLSMax / 2))
            {
                return n2;
            }

            if (hue < (HLSMax * 2 / 3))
            {
                return n1 + ((((n2 - n1) * ((HLSMax * 2 / 3) - hue)) + (HLSMax / 12)) / (HLSMax / 6));
            }
            else
            {
                return n1;
            }
        }

        private int NewLuma(int n, bool scale)
        {
            return NewLuma(Luminosity, n, scale);
        }
    }
}
