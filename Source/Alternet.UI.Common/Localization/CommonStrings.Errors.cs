#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    public partial class CommonStrings
    {
        public string ErrWrongActionForCtor { get; set; } = "Wrong action {0} for ctor";

        public string ErrParserAttributeArgsHigh { get; set; } = "Too many attributes are specified for '{0}'.";
        public string ErrParserAttributeArgsLow { get; set; } = "'{0}' requires more attributes.";

        public string ErrRequiresSTA { get; set; } = "The calling thread must be STA, because many UI components require this.";
        public string ErrInputBindingExpectedInputGesture { get; set; } = "Gesture accepts only objects of type '{0}'.";
    }
}