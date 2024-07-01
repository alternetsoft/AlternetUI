using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class StreamReadWriteEventArgs : EventArgs
    {
        public StreamReadWriteEventArgs(byte[] buffer, int offset, int count, int? readCount = null)
        {
            Buffer = buffer;
            Offset = offset;
            Count = count;
            ReadCount = readCount;
        }

        public byte[] Buffer { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }

        public int? ReadCount { get; set; }

        public string BufferAsString
        {
            get
            {
                var result = System.Text.Encoding.UTF8.GetString(Buffer, Offset, ReadCount ?? Count);
                return result;
            }
        }
    }
}
