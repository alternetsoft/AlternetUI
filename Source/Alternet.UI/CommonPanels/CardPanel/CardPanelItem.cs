using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Occurs when property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets page title.
        /// </summary>
        public string? Title
        {
            get;
        }

        /// <summary>
        /// Gets whether child control was created.
        /// </summary>
        [Browsable(false)]
        public bool ControlCreated => control != null;

        /// <summary>
        /// Child control.
        /// </summary>
        [Browsable(false)]
        public Control Control
        {
            get
            {
                var oldControl = control;
                control ??= action?.Invoke() ?? new Control();
                if(oldControl != control)
                    RaisePropertyChanged(nameof(Control));
                return control;
            }
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new(propName));
        }
    }
}
