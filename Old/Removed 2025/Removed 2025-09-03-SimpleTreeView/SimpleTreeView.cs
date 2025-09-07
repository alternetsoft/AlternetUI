using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class SimpleTreeView : BaseContentView,
        UI.ITreeViewItemContainer, UI.IListControlItemContainer
    {
        /// <summary>
        /// Default margin for the item label.
        /// </summary>
        public static Thickness DefaultItemLabelMargin = 5;

        /// <summary>
        /// Default padding for the item container.
        /// </summary>
        public static Thickness DefaultItemContainerPadding = 5;

        /// <summary>
        /// Default margin for the item image.
        /// </summary>
        public static Thickness DefaultItemImageMargin = new(2, 0, 2, 0);

        /// <summary>
        /// Default margin for the tree button image.
        /// </summary>
        public static Thickness DefaultTreeButtonImageMargin = new(0, 0, 0, 0);

        /// <summary>
        /// Default size of the tree button.
        /// </summary>
        public static int DefaultTreeButtonSize = 16;

        /// <summary>
        /// Default size of the SVG image.
        /// </summary>
        public static int DefaultSvgSize = 16;

        private readonly Grid grid = new();
        private readonly CollectionView collectionView = new();
        private readonly TapGestureRecognizer imageGestureRecognizer = new();
        private readonly UI.IListControlItemDefaults itemDefaults = new UI.ListControlItemDefaults();

        private readonly TapGestureRecognizer doubleClickGesture = new()
        {
            Buttons = ButtonsMask.Primary,
            NumberOfTapsRequired = 2,
        };

        private readonly TapGestureRecognizer tapGesture = new()
        {
            Buttons = ButtonsMask.Secondary,
        };

        private Alternet.UI.TreeViewRootItem rootItem;
        private int updateCount;
        private SKBitmapImageSource? openedImage;
        private SKBitmapImageSource? closedImage;
        private ObservableCollection<Alternet.UI.TreeViewItem>? visibleItems;
        private Alternet.UI.TreeViewButtonsKind treeButtons = Alternet.UI.TreeViewButtonsKind.Null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTreeView"/> class.
        /// </summary>
        public SimpleTreeView()
        {
            rootItem = new(this);

            grid.Add(collectionView);
            Content = grid;

            void ItemTapHandler(object? sender, TappedEventArgs e)
            {
                if (sender is View tappedFrame
                && tappedFrame.BindingContext is Alternet.UI.TreeViewItem item)
                {
                    if (!item.HasItems)
                        return;

                    item.IsExpanded = !item.IsExpanded;
                }
            }

            imageGestureRecognizer.Tapped += ItemTapHandler;
            doubleClickGesture.Tapped += ItemTapHandler;

            tapGesture.Tapped += (sender, e) =>
            {
                if (sender is View tappedFrame
                && tappedFrame.BindingContext is Alternet.UI.TreeViewItem item)
                {
                    collectionView.SelectedItem = item;
                }
            };

            collectionView.ItemTemplate = CreateItemTemplate();

            collectionView.SelectionChanged += (s, e) =>
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            };

            collectionView.SelectionMode = SelectionMode.Single;

            TreeButtons = Alternet.UI.TreeViewButtonsKind.Angle;

            TreeChanged();
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedItem"/> property or the
        /// <see cref="SelectedItems"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when
        /// the selected item in the tree view has been changed.
        /// This can be useful when you need to display information in other
        /// controls based on the current selection in the tree view.
        /// <para>
        /// The <see cref="SelectedItems"/> collection changes whenever an
        /// individual item selection changes.
        /// The property change can occur programmatically or when the user selects
        /// an item or clears the selection of an item.
        /// </para>
        /// </remarks>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        public event EventHandler? SelectionModeChanged;

        /// <summary>
        /// Occurs when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs after the tree item is expanded.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? AfterExpand;

        /// <summary>
        /// Occurs after the tree item is collapsed.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? AfterCollapse;

        /// <summary>
        /// Occurs before the tree item is expanded. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeViewCancelEventArgs>? BeforeExpand;

        /// <summary>
        /// Occurs before the tree item is collapsed. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeViewCancelEventArgs>? BeforeCollapse;

        /// <summary>
        /// Occurs after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? ExpandedChanged;

        /// <summary>
        /// Gets or sets the selection mode of the tree view.
        /// </summary>
        /// <value>The selection mode of the tree view.</value>
        public virtual SelectionMode SelectionMode
        {
            get
            {
                return collectionView.SelectionMode;
            }

            set
            {
                if (SelectionMode == value)
                    return;
                collectionView.SelectionMode = value;
                SelectionModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the selected items in the tree view.
        /// </summary>
        /// <value>The selected <see cref="Alternet.UI.TreeViewItem"/> items in the tree view.</value>
        public virtual IEnumerable<Alternet.UI.TreeViewItem> SelectedItems
        {
            get
            {
                return collectionView.SelectedItems.Cast<Alternet.UI.TreeViewItem>();
            }
        }

        /// <summary>
        /// Gets or sets the selected item in the tree view.
        /// </summary>
        /// <value>The selected <see cref="Alternet.UI.TreeViewItem"/> in the tree view.</value>
        public virtual Alternet.UI.TreeViewItem? SelectedItem
        {
            get
            {
                return collectionView.SelectedItem as Alternet.UI.TreeViewItem;
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
        public virtual Alternet.UI.TreeViewButtonsKind TreeButtons
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
        /// Gets or sets the root item of the tree view.
        /// </summary>
        public virtual Alternet.UI.TreeViewRootItem RootItem
        {
            get
            {
                return rootItem;
            }

            set
            {
                if (rootItem == value)
                    return;
                if (value is null)
                    value = new(this);
                else
                    value.SetOwner(this);
                rootItem?.SetOwner(null);
                rootItem = value;
                RefreshTree();
            }
        }

        /// <summary>
        /// Gets the collection of expanded items in the tree view.
        /// </summary>
        public IEnumerable<Alternet.UI.TreeViewItem> VisibleItems
        {
            get
            {
                if (visibleItems is null)
                    TreeChanged();
                return visibleItems!;
            }
        }

        UI.IListControlItemContainer UI.ITreeViewItemContainer.ListContainer => this;

        Drawing.SvgImage? UI.IListControlItemContainer.CheckImageUnchecked => null;

        Drawing.SvgImage? UI.IListControlItemContainer.CheckImageChecked => null;

        Drawing.SvgImage? UI.IListControlItemContainer.CheckImageIndeterminate => null;

        Drawing.ImageList? UI.IListControlItemContainer.ImageList => null;

        UI.AbstractControl? UI.IListControlItemContainer.Control => null;

        UI.IListControlItemDefaults UI.IListControlItemContainer.Defaults => itemDefaults;

        bool UI.IListControlItemContainer.Focused => IsFocused;

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
                        !IsEnabled);
            openedImage = Alternet.UI.MauiUtils.ImageSourceFromSvg(
                        opened,
                        imageSize,
                        Alternet.UI.MauiUtils.IsDarkTheme,
                        !IsEnabled);
        }

        /// <summary>
        /// Selects the specified item in the tree view and scrolls to it.
        /// </summary>
        public virtual void SelectItem(UI.TreeViewItem item)
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

            BeginUpdate();
            try
            {
                item.ExpandAllParents();
            }
            finally
            {
                EndUpdate();
            }

            InternalSelect();
        }

        /// <inheritdoc/>
        public virtual Coord GetLevelMargin()
        {
            int GetWidth(SKBitmapImageSource? source)
            {
                return source?.Bitmap.Width ?? UI.StdTreeView.DefaultLevelMargin;
            }

            var openedImageWidth = GetWidth(openedImage);
            var closedImageWidth = GetWidth(closedImage);

            var result = Math.Max(openedImageWidth, closedImageWidth);

            return result;
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
        /// Adds a child item to the specified parent item in the tree view.
        /// </summary>
        /// <param name="parentItem">The parent item to which the child item will be added.
        /// If null, the child item will be added to the root item.</param>
        /// <param name="childItem">The child item to add.</param>
        /// <param name="selectItem">If true, the child item will be selected after being added.</param>
        /// <returns>true if the child item was successfully added; otherwise, false.</returns>
        public virtual bool AddChild(
            UI.TreeViewItem? parentItem,
            UI.TreeViewItem childItem,
            bool selectItem = false)
        {
            if (childItem.Parent is not null || childItem.Owner is not null)
                return false;

            parentItem ??= rootItem;

            parentItem.Add(childItem);

            if (selectItem)
            {
                SelectItem(childItem);
            }

            return true;
        }

        /// <summary>
        /// Adds the specified item to the tree view on the root level.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="selectItem">If true, the item will be selected after being added.</param>
        public bool Add(UI.TreeViewItem item, bool selectItem = false)
        {
            return AddChild(null, item, selectItem);
        }

        /// <summary>
        /// Clears all items from the tree view.
        /// </summary>
        public virtual void Clear()
        {
            BeginUpdate();
            try
            {
                rootItem.Clear();
                visibleItems = null;
                collectionView.ItemsSource = null;
            }
            finally
            {
                EndUpdate();
            }
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
                    if (index > 0)
                        index--;
                    index = Math.Min(visibleItems.Count - 1, index);
                    if (index >= 0)
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
        public virtual bool Remove(UI.TreeViewItem? item)
        {
            if (item is null)
                return false;
            if (item.Owner != this)
                return false;
            if (item.Parent is null)
                return false;
            item.Parent.Remove(item);

            return true;
        }

        /// <summary>
        /// Begins an update of the tree view. Call this method before making
        /// multiple changes to the tree view to prevent multiple refreshes.
        /// </summary>
        public virtual void BeginUpdate()
        {
            updateCount++;
        }

        /// <summary>
        /// Ends an update of the tree view. Call this method after making
        /// multiple changes to the tree view to refresh the view.
        /// </summary>
        /// <exception cref="Exception">Thrown if EndUpdate is called without
        /// a preceding BeginUpdate call.</exception>
        public virtual void EndUpdate()
        {
            if (updateCount <= 0)
                throw new Exception("Call BeginUpdate before calling EndUpdate");
            updateCount--;
            if (updateCount == 0)
            {
                TreeChanged();
            }
        }

        /// <summary>
        /// Called when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        public virtual void RaiseItemAdded(UI.TreeViewItem item)
        {
            if (item.Parent == rootItem && visibleItems is not null)
            {
                visibleItems.Add(item);
            }
            else
            {
                TreeChanged();
            }

            if (ItemAdded is not null)
            {
                ItemAdded(this, new(item));
            }
        }

        /// <summary>
        /// Called when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public virtual void RaiseItemRemoved(UI.TreeViewItem item)
        {
            if (item.HasItems && item.IsExpanded)
            {
                TreeChanged();
            }
            else
            {
                visibleItems?.Remove(item);
            }

            if (ItemRemoved is not null)
            {
                ItemRemoved(this, new(item));
            }
        }

        /// <summary>
        /// Called after the tree item is expanded.
        /// </summary>
        public virtual void RaiseAfterExpand(UI.TreeViewItem item)
        {
            TreeChanged();

            if (AfterExpand is not null)
            {
                AfterExpand(this, new(item));
            }
        }

        /// <summary>
        /// Called after the tree item is collapsed.
        /// </summary>
        public virtual void RaiseAfterCollapse(UI.TreeViewItem item)
        {
            TreeChanged();

            if (AfterCollapse is not null)
            {
                AfterCollapse(this, new(item));
            }
        }

        /// <summary>
        /// Called before the tree item is expanded.
        /// </summary>
        public virtual void RaiseBeforeExpand(UI.TreeViewItem item, ref bool cancel)
        {
            if (BeforeExpand is not null)
            {
                UI.TreeViewCancelEventArgs e = new(item);
                BeforeExpand(this, e);
                cancel = e.Cancel;
            }
        }

        /// <summary>
        /// Called before the tree item is collapsed.
        /// </summary>
        public virtual void RaiseBeforeCollapse(UI.TreeViewItem item, ref bool cancel)
        {
            if (BeforeCollapse is not null)
            {
                UI.TreeViewCancelEventArgs e = new(item);
                BeforeCollapse(this, e);
                cancel = e.Cancel;
            }
        }

        /// <summary>
        /// Called after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        public virtual void RaiseExpandedChanged(UI.TreeViewItem item)
        {
            if (ExpandedChanged is not null)
            {
                ExpandedChanged(this, new(item));
            }
        }

        /// <inheritdoc/>
        public override void RaiseSystemColorsChanged()
        {
            TreeButtonsChanged();
            TreeChanged();
        }

        /// <summary>
        /// Updates the tree view when the tree structure changes.
        /// </summary>
        public virtual void TreeChanged()
        {
            if (updateCount > 0)
                return;
            RefreshTree();
        }

        void UI.ITreeViewItemContainer.Invalidate()
        {
            BeginUpdate();
            EndUpdate();
        }

        void UI.ITreeViewItemContainer.RaiseItemSelectedChanged(UI.TreeViewItem item, bool selected)
        {
        }

        void UI.ITreeViewItemContainer.EnsureVisible(UI.TreeViewItem? item)
        {
            if (item is null)
                return;
            SelectedItem = item;
        }

        void UI.ITreeViewItemContainer.Refresh()
        {
            (this as UI.ITreeViewItemContainer).Invalidate();
        }

        void UI.ITreeViewItemContainer.ScrollIntoView(UI.TreeViewItem? item)
        {
            (this as UI.ITreeViewItemContainer).EnsureVisible(item);
        }

        UI.ListControlItem? UI.IListControlItemContainer.SafeItem(int index)
        {
            if (visibleItems is null)
                return null;

            if (index < 0 || index >= visibleItems.Count)
                return null;

            return visibleItems[index];
        }

        string UI.IListControlItemContainer.GetItemText(int index, bool forDisplay)
        {
            var item = (this as UI.IListControlItemContainer).SafeItem(index);
            if (item is null)
                return string.Empty;

            if (forDisplay)
                return item.DisplayText ?? item.Text;
            return item.Text;
        }

        void UI.ITreeViewItemContainer.RaiseItemPropertyChanged(UI.TreeViewItem item, string? propertyName)
        {
        }

        int UI.IListControlItemContainer.GetItemCount()
        {
            if (visibleItems is null)
                return 0;

            return visibleItems.Count;
        }

        /// <summary>
        /// Creates the item template for the tree view.
        /// </summary>
        /// <returns>A <see cref="DataTemplate"/> representing the item template.</returns>
        protected virtual DataTemplate CreateItemTemplate()
        {
            var result = new DataTemplate(() =>
            {
                HorizontalStackLayout parent = new()
                {
                    Padding = DefaultItemContainerPadding,
                };

                ItemSpacer spacer = new()
                {
                    BackgroundColor = Colors.Transparent,
                    HeightRequest = 1,
                    WidthRequest = 0,
                };

                parent.Children.Add(spacer);

                var buttonImage = new TreeButtonImage(this)
                {
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center,
                };

                buttonImage.GestureRecognizers.Add(imageGestureRecognizer);

                parent.Children.Add(buttonImage);

                var itemImage = new ItemImage(this)
                {
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center,
                };

                parent.Children.Add(itemImage);

                ItemLabel nameLabel = new()
                {
                    Margin = DefaultItemLabelMargin,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                };

                parent.Add(nameLabel);

                parent.GestureRecognizers.Add(tapGesture);
                parent.GestureRecognizers.Add(doubleClickGesture);

                return parent;
            });

            return result;
        }

        private void RefreshTree()
        {
            ObservableCollection<Alternet.UI.TreeViewItem> collection
                = new(rootItem.EnumExpandedItems());
            visibleItems = collection;
            collectionView.ItemsSource = visibleItems;
        }

        private partial class ItemLabel : Label
        {
            private Alternet.UI.TreeViewItem? item;

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();

                if (item is not null)
                {
                    item.PropertyChanged -= ItemPropertyChanged;
                }

                if (BindingContext is not Alternet.UI.TreeViewItem newItem)
                    return;

                item = newItem;

                if (item is not null)
                {
                    Text = item.Text;
                    item.PropertyChanged += ItemPropertyChanged;
                }
            }

            protected void ItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                Text = item?.Text ?? string.Empty;
            }
        }

        private partial class ItemSpacer : BoxView
        {
            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                if (BindingContext is not Alternet.UI.TreeViewItem item)
                    return;
                WidthRequest = item.ForegroundMarginLeft;
            }
        }

        private partial class ItemImage : Image
        {
            private readonly SimpleTreeView owner;

            public ItemImage(SimpleTreeView owner)
            {
                this.owner = owner;
                IsVisible = false;
                Margin = DefaultItemImageMargin;
            }

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                if (BindingContext is not Alternet.UI.TreeViewItem item)
                    return;

                var isVisible = item.SvgImage is not null;

                IsVisible = isVisible;

                if (isVisible)
                {
                    Source = Alternet.UI.MauiUtils.ImageSourceFromSvg(
                                item.SvgImage,
                                DefaultSvgSize,
                                Alternet.UI.MauiUtils.IsDarkTheme,
                                !owner.IsEnabled);
                }
                else
                {
                    Source = null;
                }
            }
        }

        private partial class TreeButtonImage : Image
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
                Margin = DefaultTreeButtonImageMargin;
                this.owner = owner;
                this.Source = owner.closedImage;
            }

            public bool IsExpanded
            {
                get => (bool)GetValue(IsExpandedProperty);
                set => SetValue(IsExpandedProperty, value);
            }

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();
                if (BindingContext is not Alternet.UI.TreeViewItem item)
                    return;
                IsVisible = item.HasItems;
                IsExpanded = item.IsExpanded;
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