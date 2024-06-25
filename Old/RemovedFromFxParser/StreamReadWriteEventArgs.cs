using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Syntax.Parsers.Lsp.Fx
{
    public class StreamReadWriteEventArgs : EventArgs
    {
        public StreamReadWriteEventArgs(byte[] buffer, int offset, int count, int readCount = 0)
        {
            Buffer = buffer;
            Offset = offset;
            Count = count;
            ReadCount = readCount;
        }

        public byte[] Buffer { get; set; }

        public int Offset { get; set; }

        public int Count { get; set; }

        public int ReadCount { get; set; }
    }
}
