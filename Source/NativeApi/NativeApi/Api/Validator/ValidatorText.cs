#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_text_validator.html
    public class ValidatorText : Validator
    {
        public static void DeleteValidatorText(IntPtr handle) { }
        public static IntPtr CreateValidatorText(long style) => default;

        public static long GetStyle(IntPtr handle) => default;
        public static void SetStyle(IntPtr handle, long style) { }

        public static void SetCharIncludes(IntPtr handle, string chars) { }
        public static void AddCharIncludes(IntPtr handle, string chars) { }
        public static string GetCharIncludes(IntPtr handle) => default;

        public static void AddInclude(IntPtr handle, string include) { }
        public static void AddExclude(IntPtr handle, string exclude) { }

        public static void SetCharExcludes(IntPtr handle, string chars) { }
        public static void AddCharExcludes(IntPtr handle, string chars) { }
        public static string GetCharExcludes(IntPtr handle) => default;

        public static void ClearExcludes(IntPtr handle) { }
        public static void ClearIncludes(IntPtr handle) { }

        // Returns the error message if the contents of val are invalid or
        // the empty string if val is valid.
        public static string IsValid(IntPtr handle, string val) => default;
    }
}