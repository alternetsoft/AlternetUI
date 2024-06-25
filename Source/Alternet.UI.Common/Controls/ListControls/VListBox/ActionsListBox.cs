using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements list box control with list of actions. When item is double clicked,
    /// associated action is executed.
    /// </summary>
    public class ActionsListBox : VListBox
    {
        /// <summary>
        /// Adds an empty space to the <see cref="ActionsListBox"/>.
        /// </summary>
        /// <remarks>
        /// This method allows to separate different action groups.
        /// </remarks>
        public virtual void AddActionSpacer()
        {
            Add(string.Empty);
        }

        /// <summary>
        /// Adds <see cref="Action"/> to the <see cref="ActionsListBox"/>.
        /// </summary>
        /// <param name="title">Action title.</param>
        /// <param name="action">Action method.</param>
        public virtual void AddAction(string title, Action? action)
        {
            ListControlItem item = new(title)
            {
                DoubleClickAction = action,
            };

            Add(item);
        }
    }
}
