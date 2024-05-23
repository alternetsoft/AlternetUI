using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class IdAndData<T>
    {
        private readonly ConcurrentStack<int> freedIds = new();
        private readonly List<T?> dataList;
        private readonly object lockObject = new();

        public IdAndData(int capacity = 32000)
        {
            dataList = new(capacity);
            dataList.Add(default);
        }

        public T? GetData(int id)
        {
            int index = unchecked((int)id);
            return dataList[index];
        }

        public int AllocID(T data)
        {
            if(freedIds.TryPop(out int result))
            {
                dataList[result] = data;
                return result;
            }

            lock (lockObject)
            {
                dataList.Add(data);
                return dataList.Count - 1;
            }
        }

        public void FreeId(int id)
        {
            lock (lockObject)
            {
                dataList[id] = default;
                freedIds.Push(id);
            }
        }
    }
}
