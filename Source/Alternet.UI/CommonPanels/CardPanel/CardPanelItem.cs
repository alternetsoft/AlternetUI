using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Individual page of the <see cref="CardPanel"/>
    /// </summary>
    public class CardPanelItem : BaseControlItem
    {
        private readonly Func<Control>? action;
        private Control? control;

        internal CardPanelItem(string? title, Control control)
        {
            Title = title;
            this.control = control;
        }

        internal CardPanelItem(string? title, Func<Control> action)
        {
            Title = title;
            this.action = action;
        }

        /// <summary>
        /// Gets page title.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        /// Gets whether child control was created.
        /// </summary>
        public bool ControlCreated => control != null;

        /// <summary>
        /// Child control.
        /// </summary>
        public Control Control
        {
            get
            {
                control ??= action!();
                return control;
            }
        }
    }
}
