using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    public class CardsPanelHeader : Control
    {
        private readonly Collection<CardsPanelHeaderItem> tabs = new();
        private CardsPanelHeaderItem? selectedTab;

        private readonly HorizontalStackPanel stackPanel = new();

        private readonly Border border = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            BorderWidth = new(0, 0, 0, 3),
            Padding = new(0, 0, 0, 5),
            Margin = new(0, 0, 0, 5),
        };

        public CardsPanelHeader()
        {
            stackPanel.Parent = border;
            border.Parent = this;
        }

        public event EventHandler? TabClick;

        public IReadOnlyList<CardsPanelHeaderItem> Tabs => tabs;

        public bool UpdateCardsVisible { get; set; } = true;
        public bool UpdateCardsWidth { get; set; } = true;

        public CardsPanelHeaderItem? SelectedTab
        {
            get => selectedTab;
            set
            {
                if (selectedTab == value)
                    return;
                selectedTab = value;
                foreach(var tab in tabs)
                {
                    if (selectedTab == tab)
                        tab.HeaderControl.Font = Font.Default.AsBold;
                    else
                        tab.HeaderControl.Font = null;
                }
            }
        }

        public int? SelectedTabIndex
        {
            get
            {
                if (selectedTab == null)
                    return null;
                var result = tabs.IndexOfOrNull(selectedTab);
                return result;
            }
        }

        private Control? GetCardsParent()
        {
            foreach(var tab in tabs)
            {
                if(tab.CardControl is not null)
                {
                    return tab.CardControl.Parent;
                }
            }

            return null;
        }

        private double[] GetCardWidths()
        {
            double[] result = new double[tabs.Count];
            for (int i = 0; i < tabs.Count; i++)
            {
                var control = tabs[i].CardControl;
                if (control is null)
                    continue;
                result[i] = control.Bounds.Width;                
            }
            return result;
        }

        public void DoUpdateCardsVisible()
        {
            foreach (var tab in tabs)
            {
                if (tab.CardControl is null || SelectedTab == tab)
                    continue;
                tab.CardControl.Visible = false;
            }

            var control = SelectedTab?.CardControl;

            if (control is not null)
                control.Visible = true;
        }

        public void DoUpdateCardsWidth()
        {
            var parent = GetCardsParent();
            if (parent != null && Double.IsNaN(parent.SuggestedWidth))
            {
                var maxWidth = MathUtils.Max(GetCardWidths());
                parent.SuspendLayout();
                parent.SuggestedWidth = Math.Max(maxWidth + 30, parent.Bounds.Width);
                parent.ResumeLayout();
            }
        }

        public void Add(string text, Control? cardControl = null)
        {
            var control = new Button(text)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Parent = stackPanel,
            };
            control.Click += Item_Click;

            var item = new CardsPanelHeaderItem(control)
            {
                CardControl = cardControl
            };

            tabs.Add(item);
        }

        private void Item_Click(object? sender, EventArgs e)
        {
            foreach(var tab in tabs)
            {
                if(sender == tab.HeaderControl && SelectedTab != tab)
                {
                    SelectedTab = tab;
                    if(UpdateCardsVisible) DoUpdateCardsVisible();
                    if(UpdateCardsWidth) DoUpdateCardsWidth();
                    TabClick?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}