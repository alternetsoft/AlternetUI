using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using SkiaSharp.Views.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a simple tree view.
    /// </summary>
    public partial class SimpleTreeView : BaseContentView
    {
        /// <summary>
        /// Default size of the tree button.
        /// </summary>
        public static int DefaultTreeButtonSize = 16;

        /// <summary>
        /// Default size of the SVG image.
        /// </summary>
        public static int DefaultSvgSize = 16;

        private readonly Alternet.UI.TreeControlItem rootItem;
        private readonly Grid grid = new();
        private readonly CollectionView collectionView = new();

        private SKBitmapImageSource? openedImage;
        private SKBitmapImageSource? closedImage;
        private ObservableCollection<Alternet.UI.TreeControlItem>? visibleItems;
        private Alternet.UI.TreeViewButtonsKind treeButtons = Alternet.UI.TreeViewButtonsKind.Null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTreeView"/> class.
        /// </summary>
        public SimpleTreeView()
        {
            rootItem = new(this);

            grid.Add(collectionView);
            Content = grid;

            var doubleClickGesture = new TapGestureRecognizer
            {
                Buttons = ButtonsMask.Primary,
                NumberOfTapsRequired = 2,
            };

            void ItemTapHandler(object? sender, TappedEventArgs e)
            {
                if (sender is View tappedFrame
                && tappedFrame.BindingContext is Alternet.UI.TreeControlItem item)
                {
                    if (!item.HasItems)
                        return;

                    item.IsExpanded = !item.IsExpanded;
                    TreeChanged();
                }
            }

            var imageGestureRecognizer = new TapGestureRecognizer();

            imageGestureRecognizer.Tapped += ItemTapHandler;
            doubleClickGesture.Tapped += ItemTapHandler;

            var tapGesture = new TapGestureRecognizer
            {
                Buttons = ButtonsMask.Secondary,
            };
            tapGesture.Tapped += (sender, e) =>
            {
                if (sender is View tappedFrame
                && tappedFrame.BindingContext is Alternet.UI.TreeControlItem item)
                {
                    collectionView.SelectedItem = item;
                }
            };

            collectionView.ItemTemplate = new DataTemplate(() =>
            {
                HorizontalStackLayout parent = new()
                {
                    Padding = 5,
                };

                BoxView spacer = new()
                {
                    BackgroundColor = Colors.Transparent,
                    HeightRequest = 1,
                    WidthRequest = 0,
                };

                spacer.SetBinding(
                    BoxView.WidthRequestProperty,
                    static (Alternet.UI.TreeControlItem item) => item.ForegroundMarginLeft);

                parent.Children.Add(spacer);

                var image = new TreeButtonImage(this)
                {
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center,
                };

                image.GestureRecognizers.Add(imageGestureRecognizer);

                image.SetBinding(
                    TreeButtonImage.IsVisibleProperty,
                    static (Alternet.UI.TreeControlItem item) => item.HasItems);

                image.SetBinding(
                    TreeButtonImage.IsExpandedProperty,
                    static (Alternet.UI.TreeControlItem item) => item.IsExpanded);

                parent.Children.Add(image);

                Label nameLabel = new()
                {
                    Margin = 5,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                };

                nameLabel.SetBinding(
                    Label.TextProperty,
                    static (Alternet.UI.TreeControlItem item) => item.Text);

                parent.Add(nameLabel);

                parent.GestureRecognizers.Add(tapGesture);
                parent.GestureRecognizers.Add(doubleClickGesture);

                return parent;
            });

            collectionView.SelectionChanged += (s, e) =>
            {
            };

            collectionView.SelectionMode = SelectionMode.Single;

            TreeButtons = Alternet.UI.TreeViewButtonsKind.Angle;

            TreeChanged();
        }

        /// <summary>
        /// Gets or sets the selected item in the tree view.
        /// </summary>
        /// <value>The selected <see cref="Alternet.UI.TreeControlItem"/> in the tree view.</value>
        public Alternet.UI.TreeControlItem? SelectedItem
        {
            get
            {
                return collectionView.SelectedItem as Alternet.UI.TreeControlItem;
            }

            set
            {
                collectionView.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of tree view buttons.
        /// </summary>
        /// <value>The type of tree view buttons.</value>
        public Alternet.UI.TreeViewButtonsKind TreeButtons
        {
            get
            {
                return treeButtons;
            }

            set
            {
                if (treeButtons == value)
                    return;
                treeButtons = value;
                TreeButtonsChanged();
            }
        }

        /// <summary>
        /// Gets the root item of the tree view.
        /// </summary>
        public Alternet.UI.TreeControlItem RootItem => rootItem;

        /// <summary>
        /// Gets the collection of expanded items in the tree view.
        /// </summary>
        public IEnumerable<Alternet.UI.TreeControlItem> VisibleItems
        {
            get
            {
                if (visibleItems is null)
                    TreeChanged();
                return visibleItems!;
            }
        }

        /// <summary>
        /// Updates the tree view buttons when the TreeButtons property changes.
        /// </summary>
        public virtual void TreeButtonsChanged()
        {
            var (closed, opened) = Alternet.UI.KnownSvgImages.GetTreeViewButtonImages(treeButtons);

            var imageSize = DefaultTreeButtonSize;

            closedImage = Alternet.UI.MauiUtils.ImageSourceFromSvg(
                        closed,
                        imageSize,
                        Alternet.UI.MauiUtils.IsDarkTheme,
                        IsEnabled);
            openedImage = Alternet.UI.MauiUtils.ImageSourceFromSvg(
                        opened,
                        imageSize,
                        Alternet.UI.MauiUtils.IsDarkTheme,
                        IsEnabled);
        }

        /// <summary>
        /// Selects the specified item in the tree view and scrolls to it.
        /// </summary>
        public virtual void SelectItem(UI.TreeControlItem item)
        {
            bool InternalSelect()
            {
                if (visibleItems is not null)
                {
                    var index = visibleItems.IndexOf(item);

                    if (index > 0)
                    {
                        collectionView.SelectedItem = item;
                        collectionView.ScrollTo(index);
                        return true;
                    }
                }

                return false;
            }

            if (InternalSelect())
                return;

            item.ExpandAllParents();
            TreeChanged();
            InternalSelect();
        }

        /// <summary>
        /// Selects the last visible item in the tree view and scrolls to it.
        /// </summary>
        public virtual void SelectLastVisibleItem()
        {
            if (visibleItems is null)
                return;

            var index = visibleItems.Count - 1;
            if (index < 0)
                return;
            try
            {
                collectionView.SelectedItem = visibleItems[index];
                collectionView.ScrollTo(index);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Adds the specified item to the tree view on the root level.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="selectItem">If true, the item will be selected after being added.</param>
        public virtual bool Add(UI.TreeControlItem item, bool selectItem = false)
        {
            if (item.Parent is not null || item.Owner is not null)
                return false;

            rootItem.Add(item);
            visibleItems?.Add(item);

            if (selectItem)
            {
                SelectItem(item);
            }

            return true;
        }

        /// <summary>
        /// Clears all items from the tree view.
        /// </summary>
        public virtual void Clear()
        {
            rootItem.Clear();
            visibleItems = null;
            collectionView.ItemsSource = null;
        }

        /// <summary>
        /// Removes the currently selected item from the tree view.
        /// </summary>
        public virtual bool RemoveSelectedItem(bool selectSibling = false)
        {
            var item = SelectedItem;
            if (item is null)
                return false;

            if (selectSibling && visibleItems is not null)
            {
                var index = visibleItems.IndexOf(item);
                var result = Remove(item);
                if (result)
                {
                    if(index > 0)
                        index--;
                    index = Math.Min(visibleItems.Count - 1, index);
                    if(index >= 0)
                    {
                        item = visibleItems[index];
                        SelectItem(item);
                    }
                }

                return result;
            }
            else
            {
                return Remove(SelectedItem);
            }
        }

        /// <summary>
        /// Removes the specified item (with sub-items) from the tree view.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public virtual bool Remove(UI.TreeControlItem? item)
        {
            if (item is null)
                return false;
            if (item.Owner != this)
                return false;
            if(item.Parent is null)
                return false;
            item.Parent.Remove(item);
            if (item.HasItems && item.IsExpanded)
            {
                TreeChanged();
            }
            else
            {
                visibleItems?.Remove(item);
            }

            return true;
        }

        /// <summary>
        /// Updates the tree view when the tree structure changes.
        /// </summary>
        public virtual void TreeChanged()
        {
            ObservableCollection<Alternet.UI.TreeControlItem> collection
                = new(rootItem.EnumExpandedItems());

            int indentPx = Alternet.UI.VirtualTreeControl.DefaultLevelMargin;

            foreach (var item in collection)
            {
                var indentLevel = item.IndentLelel - 1;

                item.ForegroundMargin = (indentPx * indentLevel, 0, 0, 0);
                item.CheckBoxVisible = item.HasItems;
                item.IsChecked = item.IsExpanded;
            }

            visibleItems = collection;
            collectionView.ItemsSource = visibleItems;
        }

        /// <inheritdoc/>
        public override void RaiseSystemColorsChanged()
        {
            TreeButtonsChanged();
            TreeChanged();
        }

        private class TreeButtonImage : Image
        {
            public static readonly BindableProperty IsExpandedProperty =
                BindableProperty.Create(
                    propertyName: nameof(IsExpanded),
                    returnType: typeof(bool),
                    declaringType: typeof(TreeButtonImage),
                    defaultValue: false,
                    defaultBindingMode: BindingMode.OneWay,
                    propertyChanged: OnIsExpandedChanged);

            private readonly SimpleTreeView owner;

            public TreeButtonImage(SimpleTreeView owner)
            {
                this.owner = owner;
                this.Source = owner.closedImage;
            }

            public bool IsExpanded
            {
                get => (bool)GetValue(IsExpandedProperty);
                set => SetValue(IsExpandedProperty, value);
            }

            private static void OnIsExpandedChanged(
                BindableObject bindable,
                object oldValue,
                object newValue)
            {
                var control = (TreeButtonImage)bindable;

                if ((bool)newValue)
                {
                    control.Source = control.owner.openedImage;
                }
                else
                {
                    control.Source = control.owner.closedImage;
                }
            }
        }
    }
}
