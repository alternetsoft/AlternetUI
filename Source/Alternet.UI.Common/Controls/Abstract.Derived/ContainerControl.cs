using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all container controls.
    /// </summary>
    public partial class ContainerControl : HiddenBorder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ContainerControl(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerControl"/> class.
        /// </summary>
        public ContainerControl()
        {
            TabStop = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is focused when it is clicked.
        /// </summary>
        public virtual bool FocusOnClick { get; set; } = true;

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && FocusOnClick)
            {
                SetFocusIfPossible();
            }

            base.OnMouseDown(e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreatePanelHandler(this);
        }
    }
}
