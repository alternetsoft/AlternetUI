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
    public class ValueValidatorText : ValueValidator, IValueValidatorText
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

        /// <inheritdoc cref="IValueValidatorText.CharExcludes"/>
        public string CharExcludes
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
        public ValueValidatorTextStyle Style
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
        public string CharIncludes
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
        public void AddCharIncludes(string chars)
        {
            Native.ValidatorText.AddCharIncludes(Handle, chars);
        }

        /// <inheritdoc cref="IValueValidatorText.AddInclude"/>
        public void AddInclude(string include)
        {
            Native.ValidatorText.AddInclude(Handle, include);
        }

        /// <inheritdoc cref="IValueValidatorText.AddExclude"/>
        public void AddExclude(string exclude)
        {
            Native.ValidatorText.AddExclude(Handle, exclude);
        }

        /// <inheritdoc cref="IValueValidatorText.AddCharExcludes"/>
        public void AddCharExcludes(string chars)
        {
            Native.ValidatorText.AddCharExcludes(Handle, chars);
        }

        /// <inheritdoc cref="IValueValidatorText.ClearExcludes"/>
        public void ClearExcludes()
        {
            Native.ValidatorText.ClearExcludes(Handle);
        }

        /// <inheritdoc cref="IValueValidatorText.ClearIncludes"/>
        public void ClearIncludes()
        {
            Native.ValidatorText.ClearIncludes(Handle);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.ValidatorText.DeleteValidatorText(Handle);
        }
    }
}
