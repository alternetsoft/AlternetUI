using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
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

        public PanelChildSwitcher()
        {
            waitLabel.Parent = waitLabelContainer;
        }

        public Collection<Page> Pages { get; } = new Collection<Page>();

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

        public int Add(string title, Control control)
        {
            Pages.Add(new PanelChildSwitcher.Page(title, control));
            return Pages.Count - 1;
        }

        public int Add(string title, Func<Control> fnCreate)
        {
            Pages.Add(new PanelChildSwitcher.Page(title, fnCreate));
            return Pages.Count - 1;
        }

        public class Page
        {
            private readonly Func<Control>? action;
            private Control? control;

            public Page(string title, Control control)
            {
                Title = title;
                this.control = control;
            }

            public Page(string title, Func<Control> action)
            {
                Title = title;
                this.action = action;
            }

            public string Title { get; }

            public bool ControlCreated => control != null;

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
