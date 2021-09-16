using System;

namespace Alternet.UI.Integration.IntelliSense.EventBinding
{
    public static class EventBinderProvider
    {
        public static EventBinder GetEventBinder(Language language)
        {
            if (language == SupportedLanguages.CSharp)
                return new CSharp.CSharpEventBinder();
            else
                throw new InvalidOperationException();
        }
    }
}