using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Validates text controls and values, providing a variety of filtering behaviours.
    /// </summary>
    internal class ValueValidatorText : ValueValidator, IValueValidatorText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueValidatorText"/> class.
        /// </summary>
        /// <param name="style">Specifies behavior of the validator.</param>
        public ValueValidatorText(ValueValidatorTextStyle style = ValueValidatorTextStyle.None)
            : base(Native.ValidatorText.CreateValidatorText((int)style), true)
        {
        }

        internal ValueValidatorText(IntPtr handle = default, bool disposeHandle = true)
            : base(handle, disposeHandle)
        {
        }

        /// <inheritdoc cref="IValueValidatorText.ExcludeNonAscii"/>
        public bool ExcludeNonAscii
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.Ascii);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.Ascii, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.ExcludeNonAlpha"/>
        public bool ExcludeNonAlpha
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.Alpha);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.Alpha, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.ExcludeNonAlphaNumeric"/>
        public bool ExcludeNonAlphaNumeric
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.Numeric);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.Numeric, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.ExcludeNonDigits"/>
        public bool ExcludeNonDigits
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.Digits);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.Digits, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.ExcludeNonNumeric"/>
        public bool ExcludeNonNumeric
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.Numeric);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.Numeric, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.ExcludeNonHexDigits"/>
        public bool ExcludeNonHexDigits
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.XDigits);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.XDigits, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.UseIncludeList"/>
        public bool UseIncludeList
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.IncludeList);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.IncludeList, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.ExcludeEmptyStr"/>
        public bool ExcludeEmptyStr
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.Empty);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.Empty, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.UseIncludeCharList"/>
        public bool UseIncludeCharList
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.IncludeCharList);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.IncludeCharList, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.UseExcludeList"/>
        public bool UseExcludeList
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.ExcludeList);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.ExcludeList, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.UseExcludeCharList"/>
        public bool UseExcludeCharList
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.ExcludeCharList);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.ExcludeCharList, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.AllowSpaceChar"/>
        public virtual bool AllowSpaceChar
        {
            get
            {
                return Style.HasFlag(ValueValidatorTextStyle.Space);
            }

            set
            {
                SetStyleFLag(ValueValidatorTextStyle.Space, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.CharExcludes"/>
        public virtual string CharExcludes
        {
            get
            {
                return Native.ValidatorText.GetCharExcludes(Handle);
            }

            set
            {
                Native.ValidatorText.SetCharExcludes(Handle, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.Style"/>
        public virtual ValueValidatorTextStyle Style
        {
            get
            {
                return (ValueValidatorTextStyle)((int)Native.ValidatorText.GetStyle(Handle));
            }

            set
            {
                Native.ValidatorText.SetStyle(Handle, (int)value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.CharIncludes"/>
        public virtual string CharIncludes
        {
            get
            {
                return Native.ValidatorText.GetCharExcludes(Handle);
            }

            set
            {
                Native.ValidatorText.SetCharExcludes(Handle, value);
            }
        }

        /// <inheritdoc cref="IValueValidatorText.AddCharIncludes"/>
        public virtual void AddCharIncludes(string chars)
        {
            Native.ValidatorText.AddCharIncludes(Handle, chars);
        }

        /// <inheritdoc cref="IValueValidatorText.AddInclude"/>
        public virtual void AddInclude(string include)
        {
            Native.ValidatorText.AddInclude(Handle, include);
        }

        /// <inheritdoc cref="IValueValidatorText.AddExclude"/>
        public virtual void AddExclude(string exclude)
        {
            Native.ValidatorText.AddExclude(Handle, exclude);
        }

        /// <inheritdoc cref="IValueValidatorText.AddCharExcludes"/>
        public virtual void AddCharExcludes(string chars)
        {
            Native.ValidatorText.AddCharExcludes(Handle, chars);
        }

        /// <inheritdoc cref="IValueValidatorText.IsValid"/>
        public string IsValid(string val)
        {
            return Native.ValidatorText.IsValid(Handle, val);
        }

        /// <summary>
        /// Sets <see cref="Style"/> flag on/off.
        /// </summary>
        /// <param name="styleFlag">Style flag to set.</param>
        /// <param name="value"><c>true</c> to turn on, <c>false</c> to turn off.</param>
        public virtual void SetStyleFLag(ValueValidatorTextStyle styleFlag, bool value)
        {
            if (value)
                Style |= styleFlag;
            else
                Style &= ~styleFlag;
        }

        /// <inheritdoc cref="IValueValidatorText.ClearExcludes"/>
        public virtual void ClearExcludes()
        {
            Native.ValidatorText.ClearExcludes(Handle);
        }

        /// <inheritdoc cref="IValueValidatorText.ClearIncludes"/>
        public virtual void ClearIncludes()
        {
            Native.ValidatorText.ClearIncludes(Handle);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            Native.ValidatorText.DeleteValidatorText(Handle);
        }
    }
}
