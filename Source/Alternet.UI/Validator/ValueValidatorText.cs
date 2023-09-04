using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ValueValidatorText : ValueValidator, IValueValidatorText
    {
        public ValueValidatorText(ValueValidatorTextStyle style)
            : base(Native.ValidatorText.CreateValidatorText((int)style), true)
        {
        }

        public ValueValidatorText(IntPtr handle = default, bool disposeHandle = true)
            : base(handle, disposeHandle)
        {
        }

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

        public void AddCharIncludes(string chars)
        {
            Native.ValidatorText.AddCharIncludes(Handle, chars);
        }

        public void AddInclude(string include)
        {
            Native.ValidatorText.AddInclude(Handle, include);
        }

        public void AddExclude(string exclude)
        {
            Native.ValidatorText.AddExclude(Handle, exclude);
        }

        public void AddCharExcludes(string chars)
        {
            Native.ValidatorText.AddCharExcludes(Handle, chars);
        }

        public void ClearExcludes()
        {
            Native.ValidatorText.ClearExcludes(Handle);
        }

        public void ClearIncludes()
        {
            Native.ValidatorText.ClearIncludes(Handle);
        }

        protected override void DisposeUnmanagedResources()
        {
            Native.ValidatorText.DeleteValidatorText(Handle);
        }
    }
}
