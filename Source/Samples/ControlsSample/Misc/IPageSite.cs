namespace ControlsSample
{
    internal interface IPageSite
    {
        void LogEventSmart(string message, string? prefix);
        void LogEvent(string message);
        string? LastEventMessage { get; }
    }
}