using System.ComponentModel;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class Control : Component
    {
        public Control()
        {
        }

        internal abstract Native.Control NativeControl { get; }
    }
}