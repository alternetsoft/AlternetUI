using System;
using System.ComponentModel;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class Control : Component
    {
        public Control()
        {
        }

        public IntPtr NativePointer { get; protected set; }
    }
}