using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class PropertyGridHandler : ControlHandler<PropertyGrid>
    {
        public PropertyGridHandler()
            : base()
        {
        }

        public long CreateStyle
        {
            get
            {
                return NativeControl.CreateStyle;
            }

            set
            {
                NativeControl.CreateStyle = value;
            }
        }

        public long CreateStyleEx
        {
            get
            {
                return NativeControl.CreateStyleEx;
            }

            set
            {
                NativeControl.CreateStyleEx = value;
            }
        }

        public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();

        public new Native.PropertyGrid NativeControl =>
            (Native.PropertyGrid)base.NativeControl!;

        public override void OnLayout()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativePropertyGrid(PropertyGrid.DefaultCreateStyle);
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.ButtonClick -= NativeControl_ButtonClick;
            NativeControl.Selected -= NativeControl_Selected;
            NativeControl.Changed -= NativeControl_Changed;
            NativeControl.Changing -= NativeControl_Changing;
            NativeControl.Highlighted -= NativeControl_Highlighted;
            NativeControl.RightClick -= NativeControl_RightClick;
            NativeControl.DoubleClick -= NativeControl_DoubleClick;
            NativeControl.ItemCollapsed -= NativeControl_ItemCollapsed;
            NativeControl.ItemExpanded -= NativeControl_ItemExpanded;
            NativeControl.LabelEditBegin -= NativeControl_LabelEditBegin;
            NativeControl.LabelEditEnding -= NativeControl_LabelEditEnding;
            NativeControl.ColBeginDrag -= NativeControl_ColBeginDrag;
            NativeControl.ColDragging -= NativeControl_ColDragging;
            NativeControl.ColEndDrag -= NativeControl_ColEndDrag;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.ButtonClick += NativeControl_ButtonClick;
            NativeControl.Selected += NativeControl_Selected;
            NativeControl.Changed += NativeControl_Changed;
            NativeControl.Changing += NativeControl_Changing;
            NativeControl.Highlighted += NativeControl_Highlighted;
            NativeControl.RightClick += NativeControl_RightClick;
            NativeControl.DoubleClick += NativeControl_DoubleClick;
            NativeControl.ItemCollapsed += NativeControl_ItemCollapsed;
            NativeControl.ItemExpanded += NativeControl_ItemExpanded;
            NativeControl.LabelEditBegin += NativeControl_LabelEditBegin;
            NativeControl.LabelEditEnding += NativeControl_LabelEditEnding;
            NativeControl.ColBeginDrag += NativeControl_ColBeginDrag;
            NativeControl.ColDragging += NativeControl_ColDragging;
            NativeControl.ColEndDrag += NativeControl_ColEndDrag;
        }

        private void NativeControl_ButtonClick(object? sender, EventArgs e)
        {
            Control.RaiseButtonClick(e);
        }

        private void NativeControl_ColEndDrag(object? sender, EventArgs e)
        {
            Control.RaiseColEndDrag(e);
        }

        private void NativeControl_ColDragging(object? sender, EventArgs e)
        {
            Control.RaiseColDragging(e);
        }

        private void NativeControl_ColBeginDrag(object? sender, CancelEventArgs e)
        {
            Control.RaiseColBeginDrag(e);
        }

        private void NativeControl_LabelEditEnding(object? sender, CancelEventArgs e)
        {
            Control.RaiseLabelEditEnding(e);
        }

        private void NativeControl_LabelEditBegin(object? sender, CancelEventArgs e)
        {
            Control.RaiseLabelEditBegin(e);
        }

        private void NativeControl_ItemExpanded(object? sender, EventArgs e)
        {
            Control.RaiseItemExpanded(e);
        }

        private void NativeControl_ItemCollapsed(object? sender, EventArgs e)
        {
            Control.RaiseItemCollapsed(e);
        }

        private void NativeControl_DoubleClick(object? sender, EventArgs e)
        {
            Control.RaisePropertyDoubleClick(e);
        }

        private void NativeControl_RightClick(object? sender, EventArgs e)
        {
            Control.RaisePropertyRightClick(e);
        }

        private void NativeControl_Highlighted(object? sender, EventArgs e)
        {
            Control.RaisePropertyHighlighted(e);
        }

        private void NativeControl_Changing(object? sender, CancelEventArgs e)
        {
            Control.RaisePropertyChanging(e);
        }

        private void NativeControl_Changed(object? sender, EventArgs e)
        {
            Control.RaisePropertyChanged(e);
        }

        private void NativeControl_Selected(object? sender, EventArgs e)
        {
            Control.RaisePropertySelected(e);
        }

        public class NativePropertyGrid : Native.PropertyGrid
        {
            public NativePropertyGrid(PropertyGridCreateStyle style)
                : base()
            {
                SetNativePointer(NativeApi.PropertyGrid_CreateEx_((int)style));
            }
        }
    }
}