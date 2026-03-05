using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains defaults used when <see cref="ListControlItem"/> is painted.
    /// Implements <see cref="IListControlItemDefaults"/>.
    /// </summary>
    public class ListControlItemDefaults : BaseObjectWithNotify, IListControlItemDefaults
    {
        private Thickness itemMargin = VirtualListBox.DefaultItemMargin;
        private bool selectionUnderImage = true;
        private SizeI? svgImageSize;
        private bool selectedItemIsBold = false;
        private float minItemHeight = VirtualListBox.DefaultMinItemHeight;
        private Color? itemTextColor;
        private HVAlignment itemAlignment = ListControlItem.DefaultItemAlignment;
        private Color? selectedItemTextColor;
        private Color? disabledItemTextColor;
        private bool selectionVisible = true;
        private bool checkBoxVisible = false;
        private bool checkBoxThreeState = false;
        private bool textVisible = true;
        private Color? selectedItemBackColor;
        private bool currentItemBorderVisible = true;
        private BorderSettings? currentItemBorder;
        private BorderSettings? selectionBorder;
        private Color? unfocusedSelectedItemTextColor;
        private Color? unfocusedSelectedItemBackColor;
        private bool checkBoxAllowAllStatesForUser;

        /// <inheritdoc/>
        public virtual Thickness ItemMargin
        {
            get => itemMargin;
            set
            {
                SetProperty(ref itemMargin, value, nameof(ItemMargin));
            }
        }

        /// <inheritdoc/>
        public virtual bool SelectionUnderImage
        {
            get => selectionUnderImage;
            set
            {
                SetProperty(ref selectionUnderImage, value, nameof(SelectionUnderImage));
            }
        }

        /// <inheritdoc/>
        public virtual SizeI? SvgImageSize
        {
            get => svgImageSize;
            set
            {
                SetProperty(ref svgImageSize, value, nameof(SvgImageSize));
            }
        }

        /// <inheritdoc/>
        public virtual bool SelectedItemIsBold
        {
            get => selectedItemIsBold;
            set
            {
                SetProperty(ref selectedItemIsBold, value, nameof(SelectedItemIsBold));
            }
        }

        /// <inheritdoc/>
        public virtual Coord MinItemHeight
        {
            get => minItemHeight;
            set
            {
                SetProperty(ref minItemHeight, value, nameof(MinItemHeight));
            }
        }

        /// <inheritdoc/>
        public virtual Color? ItemTextColor
        {
            get => itemTextColor;
            set
            {
                SetProperty(ref itemTextColor, value, nameof(ItemTextColor));
            }
        }

        /// <inheritdoc/>
        public virtual HVAlignment ItemAlignment
        {
            get => itemAlignment;
            set
            {
                SetProperty(ref itemAlignment, value, nameof(ItemAlignment));
            }
        }

        /// <inheritdoc/>
        public virtual Color? SelectedItemTextColor
        {
            get => selectedItemTextColor;
            set
            {
                SetProperty(ref selectedItemTextColor, value, nameof(SelectedItemTextColor));
            }
        }

        /// <inheritdoc/>
        public virtual Color? DisabledItemTextColor
        {
            get => disabledItemTextColor;
            set
            {
                SetProperty(ref disabledItemTextColor, value, nameof(DisabledItemTextColor));
            }
        }

        /// <inheritdoc/>
        public virtual bool SelectionVisible
        {
            get => selectionVisible;
            set
            {
                SetProperty(ref selectionVisible, value, nameof(SelectionVisible));
            }
        }

        /// <inheritdoc/>
        public virtual bool CheckBoxVisible
        {
            get => checkBoxVisible;
            set
            {
                SetProperty(ref checkBoxVisible, value, nameof(CheckBoxVisible));
            }
        }

        /// <inheritdoc/>
        public virtual bool CheckBoxThreeState
        {
            get => checkBoxThreeState;
            set
            {
                SetProperty(ref checkBoxThreeState, value, nameof(CheckBoxThreeState));
            }
        }

        /// <inheritdoc/>
        public virtual bool TextVisible
        {
            get => textVisible;
            set
            {
                SetProperty(ref textVisible, value, nameof(TextVisible));
            }
        }

        /// <inheritdoc/>
        public virtual Color? SelectedItemBackColor
        {
            get => selectedItemBackColor;
            set
            {
                SetProperty(ref selectedItemBackColor, value, nameof(SelectedItemBackColor));
            }
        }

        /// <inheritdoc/>
        public virtual bool CurrentItemBorderVisible
        {
            get => currentItemBorderVisible;
            set
            {
                SetProperty(ref currentItemBorderVisible, value, nameof(CurrentItemBorderVisible));
            }
        }

        /// <inheritdoc/>
        public virtual BorderSettings? CurrentItemBorder
        {
            get => currentItemBorder;
            set
            {
                SetProperty(ref currentItemBorder, value, nameof(CurrentItemBorder));
            }
        }

        /// <inheritdoc/>
        public virtual BorderSettings? SelectionBorder
        {
            get => selectionBorder;
            set
            {
                SetProperty(ref selectionBorder, value, nameof(SelectionBorder));
            }
        }

        /// <inheritdoc/>
        public virtual Color? UnfocusedSelectedItemTextColor
        {
            get => unfocusedSelectedItemTextColor;
            set
            {
                SetProperty(ref unfocusedSelectedItemTextColor, value, nameof(UnfocusedSelectedItemTextColor));
            }
        }

        /// <inheritdoc/>
        public virtual Color? UnfocusedSelectedItemBackColor
        {
            get => unfocusedSelectedItemBackColor;
            set
            {
                SetProperty(ref unfocusedSelectedItemBackColor, value, nameof(UnfocusedSelectedItemBackColor));
            }
        }

        /// <inheritdoc/>
        public virtual bool CheckBoxAllowAllStatesForUser
        {
            get => checkBoxAllowAllStatesForUser;
            set
            {
                SetProperty(ref checkBoxAllowAllStatesForUser, value, nameof(CheckBoxAllowAllStatesForUser));
            }
        }
    }
}
