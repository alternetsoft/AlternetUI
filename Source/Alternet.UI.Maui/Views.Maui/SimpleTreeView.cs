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
    public partial class SimpleTreeView : BaseContentView, UI.ITreeControlItemContainer
    {
        /// <summary>
        /// Default size of the tree button.
        /// </summary>
        public static int DefaultTreeButtonSize = 16;

        /// <summary>
        /// Default size of the SVG image.
        /// </summary>
        public static int DefaultSvgSize = 16;

        private readonly Alternet.UI.TreeControlRootItem rootItem;
        private readonly Grid grid = new();
        private readonly CollectionView collectionView = new();

        private int updateCount;
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
        public event EventHandler<UI.TreeControlEventArgs>? ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs after the tree item is expanded.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? AfterExpand;

        /// <summary>
        /// Occurs after the tree item is collapsed.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? AfterCollapse;

        /// <summary>
        /// Occurs before the tree item is expanded. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeControlCancelEventArgs>? BeforeExpand;

        /// <summary>
        /// Occurs before the tree item is collapsed. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeControlCancelEventArgs>? BeforeCollapse;

        /// <summary>
        /// Occurs after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? ExpandedChanged;

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
        /// <value>The selected <see cref="Alternet.UI.TreeControlItem"/> items in the tree view.</value>
        public virtual IEnumerable<Alternet.UI.TreeControlItem> SelectedItems
        {
            get
            {
                return collectionView.SelectedItems.Cast<Alternet.UI.TreeControlItem>();
            }
        }

        /// <summary>
        /// Gets or sets the selected item in the tree view.
        /// </summary>
        /// <value>The selected <see cref="Alternet.UI.TreeControlItem"/> in the tree view.</value>
        public virtual Alternet.UI.TreeControlItem? SelectedItem
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
            if(updateCount == 0)
            {
                TreeChanged();
            }
        }

        /// <summary>
        /// Called when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        public virtual void RaiseItemAdded(UI.TreeControlItem item)
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
        public virtual void RaiseItemRemoved(UI.TreeControlItem item)
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
        public virtual void RaiseAfterExpand(UI.TreeControlItem item)
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
        public virtual void RaiseAfterCollapse(UI.TreeControlItem item)
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
        public virtual void RaiseBeforeExpand(UI.TreeControlItem item, ref bool cancel)
        {
            if (BeforeExpand is not null)
            {
                UI.TreeControlCancelEventArgs e = new(item);
                BeforeExpand(this, e);
                cancel = e.Cancel;
            }
        }

        /// <summary>
        /// Called before the tree item is collapsed.
        /// </summary>
        public virtual void RaiseBeforeCollapse(UI.TreeControlItem item, ref bool cancel)
        {
            if(BeforeCollapse is not null)
            {
                UI.TreeControlCancelEventArgs e = new(item);
                BeforeCollapse(this, e);
                cancel = e.Cancel;
            }
        }

        /// <summary>
        /// Called after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        public virtual void RaiseExpandedChanged(UI.TreeControlItem item)
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
        protected virtual void TreeChanged()
        {
            if (updateCount > 0)
                return;

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