#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_numeric_property_validator.html
    public class ValidatorNumericProperty : ValidatorText
    {
        public static void DeleteValidatorNumericProperty(IntPtr handle) { }
        public static IntPtr CreateValidatorNumericProperty(int numericType, int valBase) => default;
    }
}