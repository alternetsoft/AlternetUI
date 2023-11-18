using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for text editors.
    /// </summary>
    public abstract class CustomTextEdit : Control, IReadOnlyStrings
    {
        private StringSearch? search;

        /// <summary>
        /// Gets or sets a text string that can be used as a default validator error message.
        /// </summary>
        /// <remarks>
        /// This property can be used when <see cref="ValidatorErrorText"/> is <c>null</c>.
        /// </remarks>
        public static string? DefaultValidatorErrorText { get; set; }

        /// <summary>
        /// Gets or sets default <see cref="Color"/> that can be used
        /// as a background color for the <see cref="TextBox"/> in cases when
        /// application needs to report user an error in <see cref="Text"/> property.
        /// </summary>
        public static Color DefaultErrorBackgroundColor { get; set; } = Color.Red;

        /// <summary>
        /// Gets or sets default <see cref="Color"/> that can be used
        /// as a foreground color for the <see cref="TextBox"/> in cases when
        /// application needs to report user an error in <see cref="Text"/> property.
        /// </summary>
        public static Color DefaultErrorForegroundColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultErrorForegroundColor"/>
        /// when application needs to report user an error.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c> on Windows and <c>false</c> on other platforms.
        /// Do not set <c>true</c> on MacOs as it is not supported.
        /// </remarks>
        public static bool DefaultErrorUseForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultErrorBackgroundColor"/>
        /// when application needs to report user an error.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c> on Windows and <c>false</c> on other platforms.
        /// Do not set <c>true</c> on MacOs as it is not supported.
        /// </remarks>
        public static bool DefaultErrorUseBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets default value for the <see cref="ResetErrorBackgroundMethod"/>
        /// property.
        /// </summary>
        public static ResetColorType DefaultResetErrorBackgroundMethod { get; set; } = ResetColorType.Auto;

        /// <summary>
        /// Gets or sets default value for the <see cref="ResetErrorForegroundMethod"/>
        /// property.
        /// </summary>
        public static ResetColorType DefaultResetErrorForegroundMethod { get; set; } = ResetColorType.Auto;

        /// <summary>
        /// Gets or sets a bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <see cref="Text"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it if <see cref="TextBox"/> edits a number value or
        /// for any other purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual NumberStyles? NumberStyles { get; set; }

        /// <summary>
        /// Gets or sets an object that supplies culture-specific formatting information
        /// about <see cref="Text"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets default format used in value to string convertion.
        /// </summary>
        public virtual string? DefaultFormat { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IObjectToString"/> provider which is used in
        /// value to string convertion.
        /// </summary>
        [Browsable(false)]
        public virtual IObjectToString? Converter { get; set; }

        /// <summary>
        /// Gets or sets default value for the <see cref="Text"/> property.
        /// </summary>
        public virtual string? DefaultText { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Type"/> of the <see cref="Text"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual Type? DataType { get; set; }

        /// <summary>
        /// Gets or sets validator reporter object or control.
        /// </summary>
        /// <remarks>
        /// This propety can be used to store reference to control that
        /// reports validation or other errors to the end users. Usually
        /// this is a <see cref="PictureBox"/> with error image.
        /// </remarks>
        [Browsable(false)]
        public virtual object? ValidatorReporter { get; set; }

        /// <summary>
        /// Gets or sets a text string that can be used as validator error message.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        public virtual string? ValidatorErrorText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether empty string is allowed in <see cref="Text"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual bool AllowEmptyText { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Text"/> is required to be not empty.
        /// This is an opposite of <see cref="AllowEmptyText"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public bool IsRequired
        {
            get
            {
                return !AllowEmptyText;
            }

            set
            {
                AllowEmptyText = !value;
            }
        }

        /// <summary>
        /// Gets or sets data value in cases when <see cref="Text"/> property is empty.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual object? EmptyTextValue { get; set; }

        /// <summary>
        /// Gets or sets string search provider.
        /// </summary>
        public virtual StringSearch Search
        {
            get => search ??= new(this);

            set
            {
                if (value is null)
                    return;
                search = value;
            }
        }

        /// <summary>
        /// Gets or sets method which is used to clear error state
        /// if error backround color is used for reporting it.
        /// </summary>
        public ResetColorType? ResetErrorBackgroundMethod { get; set; }

        /// <summary>
        /// Gets or sets method which is used to clear error state
        /// if error foreground color is used for reporting it.
        /// </summary>
        public ResetColorType? ResetErrorForegroundMethod { get; set; }

        /// <summary>
        /// Gets whether <see cref="Text"/> is null or empty.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrEmpty => string.IsNullOrEmpty(Text);

        /// <summary>
        /// Gets whether <see cref="Text"/> is null or white space.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(Text);

        /// <summary>
        /// Gets or sets the text contents of the text box.
        /// </summary>
        /// <value>A string containing the text contents of the text box. The
        /// default is an empty string ("").</value>
        /// <remarks>
        /// Getting this property returns a string copy of the contents of the
        /// text box. Setting this property replaces the contents of the text box
        /// with the specified string.
        /// </remarks>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public abstract string Text { get; set; }

        int IReadOnlyStrings.Count => GetNumberOfLines();

        string? IReadOnlyStrings.this[int index]
        {
            get
            {
                try
                {
                    return GetLineText(index);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Returns the number of lines in the text control buffer.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The returned number is the number of logical lines, i.e. just the count of
        /// the number of newline characters in the control + 1, for GTK
        /// and OSX/Cocoa ports while it is the number of physical lines,
        /// i.e. the count of
        /// lines actually shown in the control, in MSW and OSX/Carbon. Because of
        /// this discrepancy, it is not recommended to use this function.
        /// </remarks>
        /// <remarks>
        /// Note that even empty text controls have one line (where the
        /// insertion point is), so this function never returns 0.
        /// </remarks>
        public abstract int GetNumberOfLines();

        /// <summary>
        /// Returns the contents of a given line in the text control, not
        /// including any trailing newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The contents of the line.</returns>
        public abstract string GetLineText(long lineNo);
    }
}
