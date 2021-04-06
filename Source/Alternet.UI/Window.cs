using System;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Window : Control
    {
        private string? title = null;

        public event EventHandler? TitleChanged;

        public string? Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override ControlHandler CreateHandler()
        {
            return new NativeWindowHandler(this);
        }
    }
}