using System;

namespace XamlX.Transform
{
#if !XAMLX_INTERNAL
    public
#endif
    interface IXamlIdentifierGenerator
    {
        string GenerateIdentifierPart();
    }

#if !XAMLX_INTERNAL
    public
#endif
    class GuidIdentifierGenerator : IXamlIdentifierGenerator
    {
        public string GenerateIdentifierPart() => Guid.NewGuid().ToString().Replace("-","");
    }
}