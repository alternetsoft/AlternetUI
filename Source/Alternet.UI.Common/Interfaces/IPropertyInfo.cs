using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alternet.UI.Port
{
    internal interface IPropertyInfo
    {
        bool CanSet { get; }

        bool CanGet { get; }

        Type PropertyType { get; }

        string Name { get; }

        object? Get(object target);

        void Set(object target, object? value);
    }
}