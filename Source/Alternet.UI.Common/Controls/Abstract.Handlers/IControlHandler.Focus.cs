using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial interface IControlHandler
    {
        /// <summary>
        /// Gets or sets an action which is called when 'GotFocus' event is raised.
        /// </summary>
        Action? GotFocus { get; set; }

        /// <summary>
        /// Gets or sets an action which is called when 'LostFocus' event is raised.
        /// </summary>
        Action? LostFocus { get; set; }

        /// <inheritdoc cref="AbstractControl.TabStop"/>
        bool TabStop { get; }

        /// <inheritdoc cref="AbstractControl.CanSelect"/>
        bool CanSelect { get; }

        /// <inheritdoc cref="AbstractControl.Focused"/>
        bool IsFocused { get; }

        /// <summary>
        /// Sets focus related flags.
        /// </summary>
        /// <param name="canSelect">Whether or not this control can be selected by mouse.</param>
        /// <param name="tabStop">Whether or not this control can be selected by keyboard.</param>
        /// <param name="acceptsFocusRecursively">Whether or not this control accepts focus recursively.</param>
        void SetFocusFlags(bool canSelect, bool tabStop, bool acceptsFocusRecursively);

        /// <inheritdoc cref="AbstractControl.FocusNextControl"/>
        void FocusNextControl(bool forward = true, bool nested = true);

        /// <inheritdoc cref="AbstractControl.SetFocus"/>
        bool SetFocus();
    }
}
