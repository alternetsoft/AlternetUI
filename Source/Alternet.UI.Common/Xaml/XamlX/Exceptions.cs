#pragma warning disable
using System;
using System.Xml;
using XamlX.Ast;

#pragma warning disable SA1402
#pragma warning disable SA1649
namespace XamlX
{
#if !XAMLX_INTERNAL
    public
#endif
    class XamlParseException : Alternet.UI.BaseXmlException
    {
        public XamlParseException(string message)
            : base(message, null, 0, 0)
        {
        }

        public XamlParseException(string message, int line, int position)
            : base(message, null, line, position)
        {
        }

        public XamlParseException(string message, IXamlLineInfo lineInfo)
            : this(message, lineInfo.Line, lineInfo.Position)
        {
        }
    }

#if !XAMLX_INTERNAL
    public
#endif
    class XamlTransformException : XamlParseException
    {
        public XamlTransformException(string message, IXamlLineInfo lineInfo)
            : base(message, lineInfo)
        {
        }
    }

#if !XAMLX_INTERNAL
    public
#endif
    class XamlLoadException : XamlParseException
    {
        public XamlLoadException(string message, IXamlLineInfo lineInfo)
            : base(message, lineInfo)
        {
        }
    }

#if !XAMLX_INTERNAL
    public
#endif
    class XamlTypeSystemException : Exception
    {
        public XamlTypeSystemException(string message)
            : base(message)
        {
        }
    }
}
