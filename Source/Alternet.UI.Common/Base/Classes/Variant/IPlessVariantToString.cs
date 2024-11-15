using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal interface IPlessVariantToString
    {
        string ToString();

        string ToString(string format);

        string ToString(IFormatProvider provider);

        string ToString(string format, IFormatProvider formatProvider);
    }
}
