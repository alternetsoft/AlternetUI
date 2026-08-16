using System;
using System.ComponentModel;

using Alternet.UI;

namespace Alternet.Drawing;

/// <summary>
/// Specifies formatting information, display manipulations, and font related features for text.
/// </summary>
public partial class StringFormat : DisposableObject, ICloneable
{
    private Record data;

    /// <summary>
    /// Initializes a new instance of the <see cref='StringFormat'/> class.
    /// </summary>
    public StringFormat() : this(0, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='StringFormat'/> class with the specified format flags.
    /// </summary>
    public StringFormat(StringFormatFlags options) : this(options, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='StringFormat'/> class with the specified
    /// format flags and language identifier.
    /// </summary>
    public StringFormat(StringFormatFlags options, int language)
    {
        data.FormatFlags = options;
        data.DigitSubstitutionLanguage = language;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='StringFormat'/> class from the specified
    /// existing <see cref='StringFormat'/>.
    /// </summary>
    public StringFormat(StringFormat? format)
    {
        if (format is null)
            return;
        data.FormatFlags = format.data.FormatFlags;
    }

    /// <summary>
    /// Resets the properties of this <see cref='StringFormat'/> to their default values.
    /// </summary>
    public virtual void Reset()
    {
        if (Immutable)
            return;
        data = new Record();
    }

    /// <summary>
    /// Creates an exact copy of this <see cref='StringFormat'/>.
    /// </summary>
    public virtual object Clone() => new StringFormat(this);

    /// <summary>
    /// Gets or sets a <see cref='StringFormatFlags'/> that contains formatting information.
    /// </summary>
    public virtual StringFormatFlags FormatFlags
    {
        get => data.FormatFlags;
        set
        {
            data.FormatFlags = GetNewFieldValue(data.FormatFlags, value);
        }
    }

    /// <summary>
    /// Sets the measure of characters to the specified range.
    /// </summary>
    public virtual void SetMeasurableCharacterRanges(CharacterRange[] ranges)
    {
        if (Immutable)
            return;
        if (ranges is null)
            ranges = Array.Empty<CharacterRange>();
        data.Ranges = ranges;
    }

    /// <summary>
    /// Specifies text alignment information.
    /// </summary>
    public virtual StringAlignment Alignment
    {
        get => data.Alignment;

        set
        {
            if (value < StringAlignment.Near || value > StringAlignment.Far)
                value = 0;
            data.Alignment = GetNewFieldValue(data.Alignment, value);
        }
    }

    /// <summary>
    /// Gets or sets the line alignment.
    /// </summary>
    public virtual StringAlignment LineAlignment
    {
        get
        {
            return data.LineAlignment;
        }
        set
        {
            if (value < StringAlignment.Near || value > StringAlignment.Far)
                value = 0;
            data.LineAlignment = GetNewFieldValue(data.LineAlignment, value);
        }
    }

    /// <summary>
    /// Gets or sets the <see cref='HotkeyPrefix'/> for this <see cref='StringFormat'/> .
    /// </summary>
    public virtual HotkeyPrefix HotkeyPrefix
    {
        get
        {
            return data.HotkeyPrefix;
        }
        set
        {
            if (value < HotkeyPrefix.None || value > HotkeyPrefix.Hide)
                value = 0;

            data.HotkeyPrefix = GetNewFieldValue(data.HotkeyPrefix, value);
        }
    }

    /// <summary>
    /// Sets tab stops for this <see cref='StringFormat'/> instance.
    /// </summary>
    /// <param name="firstTabOffset">The number of spaces between the beginning of a text line and the first tab stop.</param>
    /// <param name="tabStops">An array of distances (in number of spaces) between tab stops.</param>
    /// <exception cref="ArgumentException"></exception>
    public virtual void SetTabStops(float firstTabOffset, float[] tabStops)
    {
        if (Immutable)
            return;
        if (tabStops is null)
            tabStops = Array.Empty<float>();
        if (firstTabOffset < 0)
        {
            firstTabOffset = 0;
        }

        data.TabStops = tabStops;
        data.FirstTabOffset = firstTabOffset;
    }

    /// <summary>
    /// Gets the tab stops for this <see cref='StringFormat'/> instance.
    /// </summary>
    /// <param name="firstTabOffset">The number of spaces between the beginning of a text line and the first tab stop.</param>
    /// <returns>An array of distances (in number of spaces) between tab stops.</returns>
    public virtual float[] GetTabStops(out float firstTabOffset)
    {
        firstTabOffset = data.FirstTabOffset;
        return data.TabStops;
    }

    /// <summary>
    /// Gets or sets the <see cref='StringTrimming'/> for this <see cref='StringFormat'/>.
    /// </summary>
    public virtual StringTrimming Trimming
    {
        get
        {
            return data.Trimming;
        }
        set
        {
            if (value < StringTrimming.None || value > StringTrimming.EllipsisPath)
                value = StringTrimming.None;
            data.Trimming = GetNewFieldValue(data.Trimming, value);
        }
    }

    /// <summary>
    /// Creates a default <see cref="StringFormat"/> instance.
    /// </summary>
    /// <remarks>
    /// The default <see cref="StringFormat"/> has the following characteristics:
    /// <list type="bullet">
    ///   <item>
    ///     <description>No string format flags are set.</description>
    ///   </item>
    ///   <item>
    ///     <description>Character alignment and line alignment are set to <see cref="StringAlignment.Near"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>Language ID is set to neutral (the current language associated
    ///     with the calling thread is used).</description>
    ///   </item>
    ///   <item>
    ///     <description>String digit substitution is set to <see cref="StringDigitSubstitute.User"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>Hotkey prefix is set to <see cref="HotkeyPrefix.None"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>The number of tab stops is zero.</description>
    ///   </item>
    ///   <item>
    ///     <description>String trimming is set to <see cref="StringTrimming.Character"/>.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public static StringFormat GenericDefault
    {
        get
        {
            return new StringFormat();
        }
    }

    /// <summary>
    /// Gets a generic typographic <see cref="StringFormat"/>.
    /// </summary>
    /// <remarks>
    /// The generic typographic <see cref="StringFormat"/> has the following characteristics:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///     The following format flags are set:  <see cref="StringFormatFlags.LineLimit"/>, 
    ///     <see cref="StringFormatFlags.NoClip"/>, and <see cref="StringFormatFlags.FitBlackBox"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>Char and line alignment are set to <see cref="StringAlignment.Near"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>Language ID is neutral (the current language associated with the calling thread is used).</description>
    ///   </item>
    ///   <item>
    ///     <description>String digit substitution is set to <see cref="StringDigitSubstitute.User"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>Hotkey prefix is set to <see cref="HotkeyPrefix.None"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>The number of tab stops is zero.</description>
    ///   </item>
    ///   <item>
    ///     <description>String trimming is set to <see cref="StringTrimming.None"/>.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public static StringFormat GenericTypographic
    {
        get
        {
            StringFormat format = new ();
            format.data.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            format.data.Trimming = StringTrimming.None;
            return format;
        }
    }

    /// <summary>
    /// Sets the language and digit substitution method for this <see cref='StringFormat'/> instance.
    /// </summary>
    /// <param name="language"> The language for digit substitution. </param>
    /// <param name="substitute"> The digit substitution method. </param>
    public virtual void SetDigitSubstitution(int language, StringDigitSubstitute substitute)
    {
        if (Immutable)
            return;
        data.DigitSubstitutionLanguage = language;
        data.DigitSubstitutionMethod = substitute;
    }

    /// <summary>
    /// Gets the <see cref='StringDigitSubstitute'/> for this <see cref='StringFormat'/> instance.
    /// </summary>
    public virtual StringDigitSubstitute DigitSubstitutionMethod
    {
        get
        {
            return data.DigitSubstitutionMethod;
        }
    }

    /// <summary>
    /// Gets the language of <see cref='StringDigitSubstitute'/> for this <see cref='StringFormat'/> instance.
    /// </summary>
    public virtual int DigitSubstitutionLanguage
    {
        get
        {
            return data.DigitSubstitutionLanguage;
        }
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    public override string ToString() => $"[StringFormat, FormatFlags={FormatFlags}]";

    /// <summary>
    /// Contains all properties of the <see cref="StringFormat"/>.
    /// </summary>
    public struct Record
    {
        /// <summary>
        /// <inheritdoc cref="StringFormat.FormatFlags"/>.
        /// </summary>
        public StringFormatFlags FormatFlags = 0;

        /// <summary>
        /// <inheritdoc cref="StringFormat.Alignment"/>.
        /// </summary>
        public StringAlignment Alignment = StringAlignment.Near;

        /// <summary>
        /// <inheritdoc cref="StringFormat.LineAlignment"/>.
        /// </summary>
        public StringAlignment LineAlignment = StringAlignment.Near;

        /// <summary>
        /// <inheritdoc cref="StringFormat.HotkeyPrefix"/>.
        /// </summary>
        public HotkeyPrefix HotkeyPrefix = HotkeyPrefix.None;

        /// <summary>
        /// <inheritdoc cref="StringFormat.Trimming"/>.
        /// </summary>
        public StringTrimming Trimming = StringTrimming.Character;

        /// <summary>
        /// <inheritdoc cref="StringFormat.DigitSubstitutionLanguage"/>.
        /// </summary>
        public int DigitSubstitutionLanguage = 0;

        /// <summary>
        /// <inheritdoc cref="StringFormat.DigitSubstitutionMethod"/>.
        /// </summary>
        public StringDigitSubstitute DigitSubstitutionMethod = StringDigitSubstitute.User;

        /// <summary>
        /// The number of spaces between the beginning of a text line and the first tab stop.
        /// </summary>
        public float FirstTabOffset;

        /// <summary>
        /// An array of distances (in number of spaces) between tab stops.
        /// </summary>
        public float[] TabStops = Array.Empty<float>();

        /// <summary>
        /// An array of <see cref="CharacterRange"/> structures that specify the ranges of characters to measure.
        /// </summary>
        public CharacterRange[] Ranges = Array.Empty<CharacterRange>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> struct.
        /// </summary>
        public Record()
        {
        }
    }
}
