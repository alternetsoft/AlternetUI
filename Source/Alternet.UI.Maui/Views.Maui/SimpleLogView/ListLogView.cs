using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a log view which internally uses <see cref="CollectionView"/>.
    /// </summary>
    internal partial class ListLogView : BaseLogView
    {
        private readonly ObservableCollection<LogItem> logItems = new();
        private readonly CollectionView collectionView = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ListLogView"/> class.
        /// </summary>
        public ListLogView()
        {
            collectionView.ItemsSource = logItems;
            collectionView.SelectionMode = SelectionMode.Single;
            collectionView.VerticalScrollBarVisibility = ScrollBarVisibility.Always;
            collectionView.HorizontalScrollBarVisibility = ScrollBarVisibility.Always;

            Content = new Grid()
            {
                Children =
                {
                    collectionView,
                },
            };
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            logItems.Clear();
        }

        /// <inheritdoc/>
        public override void GoToBegin()
        {
            if (logItems.Count > 0)
            {
                collectionView.SelectedItem = logItems[0];
                collectionView.ScrollTo(0);
            }
        }

        /// <inheritdoc/>
        protected override void AddItem(string s)
        {
            LogItem item = new(s);
            logItems.Add(item);
            collectionView.SelectedItem = item;
            collectionView.ScrollTo(item, -1, ScrollToPosition.MakeVisible, false);
        }
    }
}
