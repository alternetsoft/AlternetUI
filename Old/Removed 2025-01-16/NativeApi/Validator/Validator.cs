#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/overview_validator.html
    //https://docs.wxwidgets.org/3.2/classwx_validator.html
    public class Validator
    {
        public static void SuppressBellOnError(bool suppress) { }
        public static bool IsSilent() => default;
    }
}

/*

 */