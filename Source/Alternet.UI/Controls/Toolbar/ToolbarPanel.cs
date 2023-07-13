using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains customizable toolbar inside.
    /// </summary>
    public class ToolbarPanel : Panel
    {
        private readonly CustomToolbar toolbar;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolbarPanel"/> class.
        /// </summary>
        public ToolbarPanel()
        {
            toolbar = new()
            {
                NoDivider = true,
            };
            Children.Add(toolbar);
        }

        /// <summary>
        /// Gets or sets the <see cref="Toolbar"/> that is displayed in the control.
        /// </summary>
        /// <value>
        /// A <see cref="Toolbar"/> that represents the toolbar to display in the control.
        /// </value>
        public Toolbar Toolbar
        {
            get => toolbar;
        }

        internal class CustomToolbar : Toolbar
        {
            protected override ControlHandler CreateHandler()
            {
                return new NativeToolbarHandler(false);
            }
        }
    }
}
