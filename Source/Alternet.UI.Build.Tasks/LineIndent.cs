using System;
using System.CodeDom.Compiler;

namespace Alternet.UI.Build.Tasks
{
    internal class LineIndent : IDisposable
    {
        private readonly IndentedTextWriter writer;
        private readonly bool noNewlineAtEnd;

        public LineIndent(IndentedTextWriter writer, bool noNewlineAtEnd = false)
        {
            this.writer = writer;
            this.noNewlineAtEnd = noNewlineAtEnd;
            writer.Indent++;
        }

        public void Dispose()
        {
            writer.Indent--;
            if (!noNewlineAtEnd)
                writer.WriteLine();
        }
    }
}