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
    public partial class ActionsListBox : VirtualTreeControl
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
        /// Adds "heavy" <see cref="Action"/> to the <see cref="VirtualListBox"/>. When action
        /// executes, application's cursor is changed to the "busy" cursor.
        /// </summary>
        /// <param name="onAddAction">Function which is called when action is added.</param>
        /// <param name="title">Action title.</param>
        /// <param name="action">Action method.</param>
        public static ListControlItem AddBusyAction(
            Func<string, Action, ListControlItem> onAddAction,
            string title,
            Action action)
        {
            return onAddAction(title, Fn);

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

        /// <summary>
        /// Adds an empty space to the <see cref="ActionsListBox"/>.
        /// </summary>
        /// <remarks>
        /// This method allows to separate different action groups.
        /// </remarks>
        public virtual ListControlItem AddActionSpacer(bool drawLine = false)
        {
            TreeControlItem result;

            if (drawLine)
            {
                result = new TreeControlSeparatorItem();
            }
            else
            {
                result = new TreeControlEmptyItem();
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
            TreeControlItem item = new(title)
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
            return AddBusyAction(AddAction, title, action);
        }
    }
}
