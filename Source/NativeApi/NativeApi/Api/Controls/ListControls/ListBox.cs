#pragma warning disable
using System;

using Alternet.Drawing;

namespace NativeApi.Api
{
    public class ListBox : Control
    {
        public event EventHandler? SelectionChanged;
        
        public int GetSelection() => default;

        public bool HasBorder { get; set; }
        public bool IsSelected(int n) => default;
        public bool IsSorted() => default;
        public bool SetStringSelection(string s, bool select) => default;
        public int FindString(string s, bool bCase = false) => default;
        public int GetCountPerPage() => default;
        public int GetTopItem() => default;
        public int HitTest(PointD point) => default;
        public string GetString(uint n) => default;
        public uint GetCount() => default;
        public void Deselect(int n) { }
        public void EnsureVisible(int n) { }
        public void SetFirstItem(int n) { }
        public void SetFirstItemStr(string s) { }
        public void SetSelection(int n) { }
        public void SetString(uint n, string s) { }

        public void Clear() { }

        public void Delete(uint n) { }
        
        public int Append(string s) => default;

        public int Insert(string item, uint pos) => default;

        public int GetSelectionsCount() => default;

        public int GetSelectionsItem(int index) => default;

        public void UpdateSelections() { }

        public void SetFlags(Alternet.UI.ListBoxHandlerFlags flags) { }

        public Alternet.UI.ListBoxHandlerFlags GetFlags() => default;
    }
}
