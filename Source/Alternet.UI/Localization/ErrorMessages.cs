namespace Alternet.UI
{
    internal class ErrorMessages
    {
        public static readonly ErrorMessages Default = new();

        public string CurrentApplicationIsNotSet { get; set; } =
            "Current application instance is not set";

        public string CannotDisposeImmutableObject { get; set; } =
            "Cannot dispose an immutable object.";

        public string OnlySolidBrushInstancesSupported { get; set; } =
            "Only SolidBrush instances are supported.";

        public string CannotChangeImmutableObject { get; set; } =
            "Cannot change an immutable object.";

        public string InvalidParameter { get; set; } = "Invalid Parameter";
    }
}