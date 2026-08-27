using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Localization;

namespace ControlsSample
{
    public static class LocalizationManagerRu
    {
        public static void Initialize()
        {
            PropNameStrings.RegisterPropNameLocalizations(typeof(EnumValuesRu));
            PropNameStrings.RegisterPropNameLocalizations(typeof(PropNamesRu));
            KnownColorStrings.Default.LoadFromStaticFields(typeof(ColorNamesRu));
        }
    }
}
