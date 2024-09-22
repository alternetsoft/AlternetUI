using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    internal interface ICustomCharValidator
    {
        bool IsValidCategory(char ch);

        bool IsValidChar(char ch);

        bool IsValidCategory(UnicodeCategory cat);
    }
}
