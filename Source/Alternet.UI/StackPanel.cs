using System;

namespace Alternet.UI
{
    public class StackPanel : Control
    {
        private StackPanelOrientation orientation;

        private event EventHandler? OrientationChanged;

        public StackPanelOrientation Orientation
        {
            get => orientation;

            set
            {
                if (orientation == value)
                    return;

                orientation = value;

                PerformLayout();
                OnOrientationChanged(EventArgs.Empty);
            }
        }

        protected override ControlHandler CreateHandler()
        {
            return new NativeStackPanelHandler(this);
        }

        protected virtual void OnOrientationChanged(EventArgs e) => OrientationChanged?.Invoke(this, e);
    }
}