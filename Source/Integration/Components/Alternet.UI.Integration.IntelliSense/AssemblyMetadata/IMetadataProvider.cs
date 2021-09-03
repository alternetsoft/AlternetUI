using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI.Integration.IntelliSense.AssemblyMetadata
{
    public interface IMetadataProvider
    {
        IMetadataReaderSession GetMetadata(IEnumerable<string> paths);
    }

    public interface IMetadataReaderSession : IDisposable
    {
        IEnumerable<IAssemblyInformation> Assemblies { get; }
    }
}
