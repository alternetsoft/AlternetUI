using System;
using System.Reflection;
using System.Xml;

namespace Alternet.UI.Integration.Remoting
{
    [AlternetUIRemoteMessageGuid("41659480-1AC4-4445-9C35-CAD66C994648")]
    public class UpdateXamlMessage
    {
        public string Xaml { get; set; }
        public string AssemblyPath { get; set; }
        public string XamlFileProjectPath { get; set; }

        public int OwnerWindowX { get; set; }
        public int OwnerWindowY { get; set; }
    }

    [AlternetUIRemoteMessageGuid("3427FD02-55AE-4309-B445-9FD70F11F3BB")]
    public class UpdateXamlResultMessage
    {
        public string Error { get; set; }
        public string Handle { get; set; }
        public ExceptionDetails Exception { get; set; }
    }

    [AlternetUIRemoteMessageGuid("3A848AC5-62D8-4382-882A-D69F9BC077F1")]
    public class StartDesignerSessionMessage
    {
        public string SessionId { get; set; }
    }
    
    public class ExceptionDetails
    {
        public ExceptionDetails()
        {
        }

        public ExceptionDetails(Exception e)
        {
            if (e is TargetInvocationException)
            {
                e = e.InnerException;
            }

            ExceptionType = e.GetType().Name;
            Message = e.Message;
            StackTrace = e.StackTrace;

            if (e is XmlException xml)
            {
                LineNumber = xml.LineNumber;
                LinePosition = xml.LinePosition;
            }
        }

        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int? LineNumber { get; set; }
        public int? LinePosition { get; set; }
    }
}
