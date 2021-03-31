using System;
using System.CodeDom.Compiler;

namespace ApiGenerator
{
    internal class BlockIndent : IDisposable
    {
        private readonly IndentedTextWriter writer;

        public BlockIndent(IndentedTextWriter writer)
        {
            this.writer = writer;

            writer.WriteLine("{");
            writer.Indent++;
        }

        public void Dispose()
        {
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}