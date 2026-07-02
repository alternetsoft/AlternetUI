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
            CanSelect = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// Adds the specified child controls to the container.
        /// </summary>
        /// <param name="children">The child controls to add.</param>
        /// <returns>The current instance of <see cref="ContainerControl"/>.</returns>
        public virtual ContainerControl WithChildren(params AbstractControl[] children)
        {
            DoInsideLayout(() =>
            {
                Children.AddRange(children);
            });

            return this;
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreatePanelHandler(this);
        }
    }
}
