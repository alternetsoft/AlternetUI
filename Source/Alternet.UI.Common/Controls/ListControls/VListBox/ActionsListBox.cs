using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements list box control with list of actions. When item is double clicked,
    /// associated action is executed.
    /// </summary>
    public class ActionsListBox : VirtualListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ActionsListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsListBox"/> class.
        /// </summary>
        public ActionsListBox()
        {
        }

        /// <summary>
        /// Adds an empty space to the <see cref="ActionsListBox"/>.
        /// </summary>
        /// <remarks>
        /// This method allows to separate different action groups.
        /// </remarks>
        public virtual ListControlItem AddActionSpacer(bool drawLine = false)
        {
            ListControlItem result;

            if (drawLine)
            {
                result = new ListControlSeparatorItem();
            }
            else
            {
                result = new ListControlEmptyItem();
            }

            Add(result);
            return result;
        }

        /// <summary>
        /// Adds <see cref="Action"/> to the <see cref="ActionsListBox"/>.
        /// </summary>
        /// <param name="title">Action title.</param>
        /// <param name="action">Action method.</param>
        public virtual ListControlItem AddAction(string title, Action? action)
        {
            ListControlItem item = new(title)
            {
                DoubleClickAction = action,
            };

            Add(item);
            return item;
        }

        /// <summary>
        /// Adds "heavy" <see cref="Action"/> to the <see cref="ActionsListBox"/>. When action
        /// executes, application's cursor is changed to the "busy" cursor.
        /// </summary>
        /// <param name="title">Action title.</param>
        /// <param name="action">Action method.</param>
        public virtual ListControlItem AddBusyAction(string title, Action action)
        {
            return AddAction(title, Fn);

            void Fn()
            {
                App.DoInsideBusyCursor(() =>
                {
                    App.LogBeginUpdate();
                    try
                    {
                        action();
                    }
                    finally
                    {
                        App.LogEndUpdate();
                    }
                });
            }
        }
    }
}
