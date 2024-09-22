using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    internal interface ICharValidator : ICustomCharValidator
    {
        ICharValidator Reset();

        ICharValidator BadChar(char ch);

        ICharValidator ValidChar(char ch, bool isValid = true);

        ICharValidator ValidChars(char minCh, char maxCh, bool isValid = true);

        ICharValidator BadChars(char minCh, char maxCh, bool isValid = true);

        ICharValidator BadChars(params char[] ch);

        ICharValidator BadChars(char[] ch, bool isValid);

        ICharValidator ValidChars(params char[] ch);

        ICharValidator ValidChars(char[] ch, bool isValid);

        ICharValidator ValidChars(string s, bool isValid = true);

        ICharValidator BadChars(string s);

        ICharValidator AllCharsValid(bool isValid = true);

        ICharValidator AllCharsBad();

        ICharValidator AllCategoriesBad();

        ICharValidator AllCategoriesValid(bool isValid = true);

        ICharValidator ValidCategory(UnicodeCategory cat, bool isValid = true);

        ICharValidator BadCategory(UnicodeCategory cat);

        ICharValidator ValidCategories(params UnicodeCategory[] cat);

        ICharValidator ValidCategories(UnicodeCategory[] cat, bool isValid);

        ICharValidator BadCategories(params UnicodeCategory[] cat);
    }
}