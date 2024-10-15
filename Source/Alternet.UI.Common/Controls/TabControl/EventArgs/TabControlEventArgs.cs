using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the
    /// 'Selected' or 'Deselected'
    /// events of a <see cref="TabControl" /> control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="TabControlEventArgs" /> that contains the event data.</param>
    public delegate void TabControlEventHandler(object? sender, TabControlEventArgs e);

    /// <summary>
    /// Provides data for the 'Selected' and
    /// 'Deselected' events of a <see cref="TabControl" /> control.</summary>
    public class TabControlEventArgs : BaseEventArgs
    {
        private readonly AbstractControl tabPage;
        private readonly int tabPageIndex;
        private readonly TabControlAction action;

        /// <summary>Initializes a new instance of the
        /// <see cref="TabControlEventArgs" /> class.</summary>
        /// <param name="tabPage">The tab page the event is occurring for.</param>
        /// <param name="tabPageIndex">The zero-based index
        /// of <paramref name="tabPage" /> in the tab pages collection.</param>
        /// <param name="action">One of the <see cref="TabControlAction" /> values.</param>
        public TabControlEventArgs(AbstractControl tabPage, int tabPageIndex, TabControlAction action)
        {
            this.tabPage = tabPage;
            this.tabPageIndex = tabPageIndex;
            this.action = action;
        }

        /// <summary>
        /// Gets the tab page the event is occurring for.
        /// </summary>
        /// <returns>The tab page the event is occurring for.</returns>
        public AbstractControl TabPage => tabPage;

        /// <summary>
        /// Gets the zero-based index of the tab page
        /// in the tab pages collection.</summary>
        /// <returns>The zero-based index of the tab page
        /// in the tab pages collection.</returns>
        public int TabPageIndex => tabPageIndex;

        /// <summary>
        /// Gets a value indicating which event is occurring.
        /// </summary>
        /// <returns>One of the <see cref="TabControlAction"/> values.</returns>
        public TabControlAction Action => action;
    }
}
