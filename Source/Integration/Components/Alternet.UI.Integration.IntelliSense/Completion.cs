using System;
using System.Collections.Generic;

namespace Alternet.UI.Integration.IntelliSense
{
    public enum CompletionKind
    {
        None,
        Class,
        Property,
        AttachedProperty,
        StaticProperty,
        Namespace,
        Enum,
        MarkupExtension,
        Event,
        AttachedEvent
    }

    public class Completion
    {
        private Dictionary<object, object> properties;

        public Completion(string displayText, string insertText, string description, CompletionKind kind, int? recommendedCursorOffset = null)
        {
            DisplayText = displayText;
            InsertText = insertText;
            Description = description;
            Kind = kind;
            RecommendedCursorOffset = recommendedCursorOffset;
        }

        public Completion(string insertText, CompletionKind kind) : this(insertText, insertText, insertText, kind)
        {
        }

        public string DisplayText { get; }

        public string InsertText { get; }

        public string Description { get; }

        public CompletionKind Kind { get; }

        public int? RecommendedCursorOffset { get; }

        public Dictionary<object, object> Properties => properties ??= new Dictionary<object, object>();
        public Dictionary<object, object> TryGetProperties() => properties;

        public override string ToString() => DisplayText;
    }
}