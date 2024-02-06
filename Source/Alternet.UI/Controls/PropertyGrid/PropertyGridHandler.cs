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
            NativeControl.ButtonClick = null;
            NativeControl.Selected = null;
            NativeControl.Changed = null;
            NativeControl.Changing -= NativeControl_Changing;
            NativeControl.Highlighted = null;
            NativeControl.RightClick = null;
            NativeControl.DoubleClick = null;
            NativeControl.ItemCollapsed = null;
            NativeControl.ItemExpanded = null;
            NativeControl.LabelEditBegin -= NativeControl_LabelEditBegin;
            NativeControl.LabelEditEnding -= NativeControl_LabelEditEnding;
            NativeControl.ColBeginDrag -= NativeControl_ColBeginDrag;
            NativeControl.ColDragging = null;
            NativeControl.ColEndDrag = null;
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

        private void NativeControl_ButtonClick()
        {
            Control.RaiseButtonClick(EventArgs.Empty);
        }

        private void NativeControl_ColEndDrag()
        {
            Control.RaiseColEndDrag(EventArgs.Empty);
        }

        private void NativeControl_ColDragging()
        {
            Control.RaiseColDragging(EventArgs.Empty);
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

        private void NativeControl_ItemExpanded()
        {
            Control.RaiseItemExpanded(EventArgs.Empty);
        }

        private void NativeControl_ItemCollapsed()
        {
            Control.RaiseItemCollapsed(EventArgs.Empty);
        }

        private void NativeControl_DoubleClick()
        {
            Control.RaisePropertyDoubleClick(EventArgs.Empty);
        }

        private void NativeControl_RightClick()
        {
            Control.RaisePropertyRightClick(EventArgs.Empty);
        }

        private void NativeControl_Highlighted()
        {
            Control.RaisePropertyHighlighted(EventArgs.Empty);
        }

        private void NativeControl_Changing(object? sender, CancelEventArgs e)
        {
            Control.RaisePropertyChanging(e);
        }

        private void NativeControl_Changed()
        {
            Control.RaisePropertyChanged(EventArgs.Empty);
        }

        private void NativeControl_Selected()
        {
            Control.RaisePropertySelected(EventArgs.Empty);
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