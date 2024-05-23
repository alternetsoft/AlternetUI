using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface ITreeViewHandler : IControlHandler
    {
        /// <inheritdoc cref="TreeView.HideRoot"/>
        bool HideRoot { get; set; }

        /// <inheritdoc cref="TreeView.HasBorder"/>
        bool HasBorder { get; set; }

        /// <inheritdoc cref="TreeView.VariableRowHeight"/>
        bool VariableRowHeight { get; set; }

        /// <inheritdoc cref="TreeView.TwistButtons"/>
        bool TwistButtons { get; set; }

        /// <inheritdoc cref="TreeView.StateImageSpacing"/>
        uint StateImageSpacing { get; set; }

        /// <inheritdoc cref="TreeView.Indentation"/>
        public uint Indentation { get; set; }

        /// <inheritdoc cref="TreeView.RowLines"/>
        bool RowLines { get; set; }

        /// <inheritdoc cref="TreeView.ShowLines"/>
        bool ShowLines { get; set; }

        /// <inheritdoc cref="TreeView.ShowRootLines"/>
        bool ShowRootLines { get; set; }

        bool ShowExpandButtons { get; set; }

        /// <inheritdoc cref="TreeView.TopItem"/>
        TreeViewItem? TopItem { get; }

        /// <inheritdoc cref="TreeView.FullRowSelect"/>
        bool FullRowSelect { get; set; }

        /// <inheritdoc cref="TreeView.AllowLabelEdit"/>
        bool AllowLabelEdit { get; set; }

        void ExpandAll();

        void CollapseAll();

        TreeViewHitTestInfo HitTest(PointD point);

        bool IsItemSelected(TreeViewItem item);

        void BeginLabelEdit(TreeViewItem item);

        void EndLabelEdit(TreeViewItem item, bool cancel);

        void ExpandAllChildren(TreeViewItem item);

        void CollapseAllChildren(TreeViewItem item);

        void MakeAsListBox();

        void EnsureVisible(TreeViewItem item);

        void ScrollIntoView(TreeViewItem item);

        void SetFocused(TreeViewItem item, bool value);

        bool IsItemFocused(TreeViewItem item);

        void SetItemIsBold(TreeViewItem item, bool isBold);

        void SetItemBackgroundColor(TreeViewItem item, Color? color);

        void SetItemTextColor(TreeViewItem item, Color? color);

        void SetItemText(TreeViewItem item, string text);

        void SetItemImageIndex(TreeViewItem item, int? imageIndex);
    }
}
