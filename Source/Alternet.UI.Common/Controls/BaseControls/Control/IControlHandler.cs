using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IControlHandler : IDisposableObject
    {
        Action? Idle { get; set; }

        Action? Paint { get; set; }

        Action? MouseEnter { get; set; }

        Action? MouseLeave { get; set; }

        Action? MouseClick { get; set; }

        Action? VisibleChanged { get; set; }

        Action? MouseCaptureLost { get; set; }

        Action? GotFocus { get; set; }

        Action? LostFocus { get; set; }

        Action? DragLeave { get; set; }

        Action? VerticalScrollBarValueChanged { get; set; }

        Action? HorizontalScrollBarValueChanged { get; set; }

        Action? SizeChanged { get; set; }

        Action? Activated { get; set; }

        Action? Deactivated { get; set; }

        Action? HandleCreated { get; set; }

        Action? HandleDestroyed { get; set; }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Gets a value indicating whether handler is attached
        /// to a <see cref="Control"/>.
        /// </summary>
        bool IsAttached { get; }

        bool IsNativeControlCreated { get; }

        object GetNativeControl();

        void RaiseChildInserted(Control childControl);

        void RaiseChildRemoved(Control childControl);

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
        void Attach(Control control);

        /// <summary>
        /// Detaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        void Detach();

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        void OnLayoutChanged();
    }
}
