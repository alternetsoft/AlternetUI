using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class VListBoxHandler : WxControlHandler, IVListBoxHandler
    {
        public VListBoxHandler()
        {
        }

        public int ItemsCount
        {
            get => NativeControl.ItemsCount;
            set => NativeControl.ItemsCount = value;
        }

        /// <summary>
        /// Gets a <see cref="VirtualListBox"/> this handler provides the
        /// implementation for.
        /// </summary>
        public new VirtualListBox? Control => (VirtualListBox?)base.Control;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public override bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        internal new Native.VListBox NativeControl => (Native.VListBox)base.NativeControl;

        public void EnsureVisible(int itemIndex)
        {
            if(itemIndex >= 0 && NativeControl.ItemsCount > 0)
                NativeControl.EnsureVisible(itemIndex);
        }

        RectD? IVListBoxHandler.GetItemRect(int index)
        {
            var resultI = NativeControl.GetItemRectI(index);
            if (resultI.SizeIsEmpty)
                return null;
            var resultD = Control?.PixelToDip(resultI);
            return resultD;
        }

        bool IVListBoxHandler.ScrollToRow(int pages)
        {
            return NativeControl.ScrollToRow(pages);
        }
        
        void IVListBoxHandler.RefreshRow(int row)
        {
            NativeControl.RefreshRow(row);
        }

        void IVListBoxHandler.RefreshRows(int from, int to)
        {
            NativeControl.RefreshRows(from, to);
        }

        int IVListBoxHandler.GetVisibleEnd()
        {
            return NativeControl.GetVisibleEnd();
        }

        int IVListBoxHandler.GetVisibleBegin()
        {
            return NativeControl.GetVisibleBegin();
        }

        public int? HitTest(PointD position)
        {
            int index = NativeControl.ItemHitTest(position);
            return index == -1 ? null : index;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeVListBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.ItemsCount = Control?.Items.Count ?? 0;
        }

        private class NativeVListBox : Native.VListBox
        {
            public NativeVListBox()
            {
                SetNativePointer(NativeApi.VListBox_CreateEx_(1));
            }

            public NativeVListBox(IntPtr nativePointer)
                : base(nativePointer)
            {
            }
        }
    }
}