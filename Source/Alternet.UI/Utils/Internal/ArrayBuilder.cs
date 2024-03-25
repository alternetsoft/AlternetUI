using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal struct ArrayBuilder<T>
    {
        private const int DefaultCapacity = 4;
        private const int MaxCoreClrArrayLength = 2146435071;
        private T[] farray = Array.Empty<T>();
        private int fcount;

        public ArrayBuilder(int capacity)
        {
            this = default(ArrayBuilder<T>);
            if (capacity > 0)
            {
                farray = new T[capacity];
            }
        }

        public readonly int Capacity
        {
            get
            {
                T[] array = farray;
                if (array == null)
                {
                    return 0;
                }

                return array.Length;
            }
        }

        public int Count => fcount;

        public T this[int index]
        {
            readonly get
            {
                return farray[index];
            }

            set
            {
                farray[index] = value;
            }
        }

        public void Add(T item)
        {
            if (fcount == Capacity)
            {
                EnsureCapacity(fcount + 1);
            }

            UncheckedAdd(item);
        }

        public T First()
        {
            return farray[0];
        }

        public T Last()
        {
            return farray[fcount - 1];
        }

        public T[] ToArray()
        {
            if (fcount == 0)
            {
                return new T[0];
            }

            T[] array = farray;
            if (fcount < array.Length)
            {
                array = new T[fcount];
                Array.Copy(farray, 0, array, 0, fcount);
            }

            return array;
        }

        public void UncheckedAdd(T item)
        {
            farray[fcount++] = item;
        }

        private void EnsureCapacity(int minimum)
        {
            int capacity = Capacity;
            int num = (capacity == 0) ? DefaultCapacity : (2 * capacity);
            if ((uint)num > MaxCoreClrArrayLength)
            {
                num = Math.Max(capacity + 1, 2146435071);
            }

            num = Math.Max(num, minimum);
            T[] array = new T[num];
            if (fcount > 0)
            {
                Array.Copy(farray, 0, array, 0, fcount);
            }

            farray = array;
        }
    }
}
