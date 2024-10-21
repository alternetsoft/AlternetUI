using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which are called when properties of the specific control state object are changed.
    /// </summary>
    public interface IControlStateObjectChanged
    {
        /// <summary>
        /// 'Disabled' state properties are changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        void DisabledChanged(object? sender);

        /// <summary>
        /// 'Normal' state properties are changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        void NormalChanged(object? sender);

        /// <summary>
        /// 'Focused' state properties are changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        void FocusedChanged(object? sender);

        /// <summary>
        /// 'Hovered' state properties are changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        void HoveredChanged(object? sender);

        /// <summary>
        /// 'Pressed' state properties are changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        void PressedChanged(object? sender);

        /// <summary>
        /// 'Selected' state properties are changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        void SelectedChanged(object? sender);
    }
}
