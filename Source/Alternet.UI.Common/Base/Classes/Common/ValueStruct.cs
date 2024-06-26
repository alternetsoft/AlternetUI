﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal struct ValueStruct<T>
        where T : notnull, IEquatable<T>
    {
        public Action? Changed;

        private T value;

        public readonly T GetValue()
        {
            return value;
        }

        public void SetValue(T value)
        {
            if (((IEquatable<T>)this.value).Equals(value))
                return;
            this.value = value;
            Changed?.Invoke();
        }
    }
}
