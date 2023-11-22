using System;
using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public class PageContainer : SplitterPanel
    {
        private readonly TreeView pagesControl = new()
        {
            HasBorder = false,
        };

        private readonly CardPanel cardPanel = new()
        {
        };

        public PageContainer()
        {
            pagesControl.MakeAsListBox();

            pagesControl.SelectionChanged += PagesListBox_SelectionChanged;
            pagesControl.Parent = this;
            cardPanel.Parent = this;
            cardPanel.DebugBackgroundColor(Color.Yellow, "PageContainer.cardPanel");
            cardPanel.CardPropertyChanged += CardPanel_CardPropertyChanged;
            SplitVertical(pagesControl, cardPanel, PixelFromDip(140));
            DebugBackgroundColor(Color.Green, nameof(PageContainer));
        }

        private void CardPanel_CardPropertyChanged(object? sender, ObjectPropertyChangedEventArgs e)
        {
            if (e.Instance is not CardPanelItem card)
                return;
            if (card.ControlCreated)
                card.Control.DebugBackgroundColor(Color.Navy, "PageContainer.cardPanel.card");
        }

        public int? SelectedIndex
        {
            get => pagesControl?.SelectedItem?.Index;
            set
            {
                pagesControl.SelectedItem = pagesControl.Items[(int)value!];
            }
        }

        public TreeView PagesControl => pagesControl;

        private void PagesListBox_SelectionChanged(object? sender, System.EventArgs e)
        {
            SetActivePageControl();
        }

        public void Add(string title, Func<Control> action)
        {
            var index = cardPanel.Add(title, action);
            var item = new TreeViewItem(title)
            {
                Tag = index
            };
            pagesControl.Add(item);
        }

        private void SetActivePageControl()
        {
            var pageIndex = pagesControl.SelectedItem?.Tag;
            if (pageIndex == null)
                return;
            cardPanel.SelectedCardIndex = (int)pageIndex;
        }
    }
}