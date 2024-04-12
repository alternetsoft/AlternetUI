﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class AdvDictionaryCached<TKey, TValue> : AdvDictionary<TKey, TValue>
        where TKey : class
    {
        private TKey? lastKey;
        private TValue? lastValue;

        public TValue GetValueOrDefaultCached(TKey key, TValue defaultValue = default!)
        {
            if (lastKey == key)
                return lastValue!;
            return GetValueOrDefault(key, defaultValue);
        }

        public TValue GetOrCreateCached(TKey key, Func<TValue> func)
        {
            if (lastKey == key)
                return lastValue!;
            var result = GetOrCreate(key, func);
            lastKey = key;
            lastValue = result;
            return result;
        }

        public new bool Remove(TKey key)
        {
            ClearLastKey();
            return base.Remove(key);
        }

        public new void Clear()
        {
            ClearLastKey();
            base.Clear();
        }

        private void ClearLastKey()
        {
            lastKey = default;
            lastValue = default;
        }
    }
}
