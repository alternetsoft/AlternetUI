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

    }
}


/*

 */
