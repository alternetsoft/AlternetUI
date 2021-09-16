using System.Collections.Generic;

namespace Alternet.UI.Integration.IntelliSense.EventBinding
{
    public abstract class EventBinder
    {
        public abstract IEnumerable<string> GetSuitableHandlerMethodNames(string codeText, MetadataEvent @event);
        public abstract CreatedEventHandler CreateEventHandler(string codeText, MetadataEvent @event, string componentName);
    }
}