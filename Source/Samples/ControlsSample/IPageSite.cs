namespace ControlsSample
{
    internal interface IPageSite
    {
        void LogEvent(string message);
        string? LastEventMessage { get; }
    }
}