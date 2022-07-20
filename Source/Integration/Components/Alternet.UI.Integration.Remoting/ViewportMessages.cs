using System;

namespace Alternet.UI.Integration.Remoting
{
    public enum PixelFormat
    {
        Rgb565,
        Rgba8888,
        Bgra8888,
        MaxValue = Bgra8888
    }

    [AlternetUIRemoteMessageGuid("15561F88-A8EF-435F-BDF3-3747D4BE93A5")]
    public class MeasureViewportMessage
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

    [AlternetUIRemoteMessageGuid("E82E9607-103E-48AE-8899-594E2902F18E")]
    public class ClientViewportAllocatedMessage
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double DpiX { get; set; }
        public double DpiY { get; set; }
    }

    [AlternetUIRemoteMessageGuid("42435DF2-B065-443D-A9E8-12761FC0F1E1")]
    public class RequestViewportResizeMessage
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

    [AlternetUIRemoteMessageGuid("DB57CCA9-AA6E-48AB-9652-07327AA23361")]
    public class ClientSupportedPixelFormatsMessage
    {
        public PixelFormat[] Formats { get; set; }
    }

    [AlternetUIRemoteMessageGuid("BEFD0FC4-4BA0-4099-87FB-38155E55BB53")]
    public class ClientRenderInfoMessage
    {
        public double DpiX { get; set; }
        public double DpiY { get; set; }
    }

    [AlternetUIRemoteMessageGuid("BBE3ABFB-E40C-432F-A9F7-7151796589B5")]
    public class PreviewDataMessage
    {
        public long SequenceId { get; set; }
        public long WindowHandle { get; set; }
    }

    [AlternetUIRemoteMessageGuid("ACA6224C-7888-4E14-9F55-759C49870EDC")]
    public class PreviewDataReceivedMessage
    {
        public long SequenceId { get; set; }
    }
}
