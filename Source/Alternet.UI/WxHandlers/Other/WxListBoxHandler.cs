using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxListBoxHandler : WxControlHandler, IListBoxHandler
    {
        /// <summary>
        /// Returns <see cref="ListBox"/> instance with which this handler
        /// is associated.
        /// </summary>
        public new ListBox? Control => (ListBox?)base.Control;

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

        internal new Native.ListBox NativeControl =>
            (Native.ListBox)base.NativeControl!;

        public ListBoxHandlerFlags Flags
        {
            get => NativeControl.GetFlags();
            set => NativeControl.SetFlags(value);
        }

        public override void OnHandleCreated()
        {
            ItemsToPlatform();
        }

        public void ItemsToPlatform()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ListBox();
        }

        public override void OnSystemColorsChanged()
        {
            base.OnSystemColorsChanged();

            if (Control is null)
                return;

            if (App.IsWindowsOS)
            {
                try
                {
                    RecreateWindow();
                }
                finally
                {
                }
            }
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (Control is null)
                return;

            if (App.IsWindowsOS)
                UserPaint = true;

            ItemsToPlatform();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (Control is null)
                return;
        }

        public int GetSelection()
        {
            return NativeControl.GetSelection();
        }

        public bool IsSelected(int n)
        {
            return NativeControl.IsSelected(n);
        }

        public bool IsSorted()
        {
            return NativeControl.IsSorted();
        }

        public bool SetStringSelection(string s, bool select)
        {
            return NativeControl.SetStringSelection(s, select);
        }

        public int FindString(string s, bool bCase = false)
        {
            return NativeControl.FindString(s, bCase);
        }

        public int GetCountPerPage()
        {
            return NativeControl.GetCountPerPage();
        }

        public int GetTopItem()
        {
            return NativeControl.GetTopItem();
        }

        public int HitTest(PointD point)
        {
            return NativeControl.HitTest(point);
        }

        public string GetString(uint n)
        {
            return NativeControl.GetString(n);
        }

        public uint GetCount()
        {
            return NativeControl.GetCount();
        }

        public void Deselect(int n)
        {
            NativeControl.Deselect(n);
        }

        public void EnsureVisible(int n)
        {
            NativeControl.EnsureVisible(n);
        }

        public void SetFirstItem(int n)
        {
            NativeControl.SetFirstItem(n);
        }

        public void SetFirstItemStr(string s)
        {
            NativeControl.SetFirstItemStr(s);
        }

        public void SetSelection(int n)
        {
            NativeControl.SetSelection(n);
        }

        public void SetString(uint n, string s)
        {
            NativeControl.SetString(n, s);
        }

        public void Clear()
        {
            NativeControl.Clear();
        }

        public void Delete(uint n)
        {
            NativeControl.Delete(n);
        }

        public int Append(string s)
        {
            return NativeControl.Append(s);
        }

        public int Insert(string item, uint pos)
        {
            return NativeControl.Insert(item, pos);
        }

        public int GetSelectionsCount()
        {
            return NativeControl.GetSelectionsCount();
        }

        public int GetSelectionsItem(int index)
        {
            return NativeControl.GetSelectionsItem(index);
        }

        public void UpdateSelections()
        {
            NativeControl.UpdateSelections();
        }
    }
}