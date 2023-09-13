using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class AssemblyResourceDescriptor : IAssetDescriptor
    {
        private readonly Assembly asm;
        private readonly string name;

        public AssemblyResourceDescriptor(Assembly asm, string name)
        {
            this.asm = asm;
            this.name = name;
        }

        public Assembly Assembly => asm;

        public Stream GetStream()
        {
            var s = asm.GetManifestResourceStream(name);
            return s ?? throw new InvalidOperationException(
                $"Could not find manifest resource stream '{name}',");
        }
    }
}
