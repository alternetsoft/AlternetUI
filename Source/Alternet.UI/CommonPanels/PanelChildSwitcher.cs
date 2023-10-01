using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to switch child controls, so only one of them is visible and other are hidden.
    /// </summary>
    /// <remarks>
    /// It behaves like <see cref="TabControl"/> but has no tab titles.
    /// </remarks>
    public class PanelChildSwitcher : Control
    {
        private readonly VerticalStackPanel waitLabelContainer = new()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Size = new Size(400, 400),
        };

        private readonly Label waitLabel = new()
        {
            Text = "Page is loading...",
            Margin = new Thickness(100, 100, 0, 0),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelChildSwitcher"/> class.
        /// </summary>
        public PanelChildSwitcher()
        {
            waitLabel.Parent = waitLabelContainer;
        }

        /// <summary>
        /// Gets pages with child controls.
        /// </summary>
        public Collection<Page> Pages { get; } = new Collection<Page>();

        /// <summary>
        /// Sets active page.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        public void SetActivePage(int? pageIndex)
        {
            if (pageIndex == null)
                return;

            SuspendLayout();
            try
            {
                GetVisibleChildOrNull()?.Hide();
                var page = Pages[pageIndex.Value];
                var loaded = page.ControlCreated;

                if (!loaded)
                {
                    waitLabelContainer.Parent = this;
                    waitLabelContainer.Visible = true;
                    waitLabelContainer.Refresh();
                    Application.DoEvents();
                }

                var control = page.Control;
                control.Parent = this;
                control.Visible = true;
                control.PerformLayout();
                waitLabelContainer.Visible = false;
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public int Add(string title, Control control)
        {
            Pages.Add(new PanelChildSwitcher.Page(title, control));
            return Pages.Count - 1;
        }

        /// <summary>
        /// Adds new page.
        /// </summary>
        /// <param name="title">Page title.</param>
        /// <param name="fnCreate">Function which creates the control.</param>
        /// <returns></returns>
        public int Add(string title, Func<Control> fnCreate)
        {
            Pages.Add(new PanelChildSwitcher.Page(title, fnCreate));
            return Pages.Count - 1;
        }

        /// <summary>
        /// Individual page of the <see cref="PanelChildSwitcher"/>
        /// </summary>
        public class Page
        {
            private readonly Func<Control>? action;
            private Control? control;

            internal Page(string title, Control control)
            {
                Title = title;
                this.control = control;
            }

            internal Page(string title, Func<Control> action)
            {
                Title = title;
                this.action = action;
            }

            /// <summary>
            /// Gets page title.
            /// </summary>
            public string Title { get; }

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
}
