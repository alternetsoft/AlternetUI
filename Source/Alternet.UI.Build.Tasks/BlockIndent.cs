using System;
using System.CodeDom.Compiler;

namespace Alternet.UI.Build.Tasks
{
    internal class BlockIndent : IDisposable
    {
        private readonly IndentedTextWriter writer;
        private readonly bool noNewlineAtEnd;

        public BlockIndent(IndentedTextWriter writer, bool noNewlineAtEnd = false)
        {
            this.writer = writer;
            this.noNewlineAtEnd = noNewlineAtEnd;
            writer.WriteLine("{");
            writer.Indent++;
        }

        public void Dispose()
        {
            writer.Indent--;
            writer.Write("}");
            if (!noNewlineAtEnd)
                writer.WriteLine();
        }
    }
}