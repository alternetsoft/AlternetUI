using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class UIResourceDescriptor : IAssetDescriptor
    {
        private readonly int offset;
        private readonly int length;

        public UIResourceDescriptor(Assembly asm, int offset, int length)
        {
            this.offset = offset;
            this.length = length;
            Assembly = asm;
        }

        public Assembly Assembly { get; }

        public Stream GetStream()
        {
            var s = Assembly.GetManifestResourceStream(ResourceConsts.UIResourceName) ??
                    throw new InvalidOperationException(
                        $"Could not find manifest resource stream '{ResourceConsts.UIResourceName}',");
            return new SlicedStream(s, offset, length);
        }
    }
}