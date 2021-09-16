using System.Collections.Generic;

namespace Alternet.UI.Integration.IntelliSense.EventBinding
{
    public record TextInsertion(int Line, int Column, string Text);

    public abstract class EventBinder
    {
        public abstract IEnumerable<string> GetSuitableHandlerMethodNames(string codeText, MetadataEvent @event);
        public abstract string CreateUniqueHandlerName(string codeText, MetadataEvent @event, MetadataType componentType, string componentName);
        public abstract TextInsertion TryAddEventHandler(string codeText, MetadataEvent @event, string handlerName);
        public abstract bool CanAddEventHandlers(string codeText);
    }
}