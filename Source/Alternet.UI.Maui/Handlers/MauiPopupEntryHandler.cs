using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;
using Alternet.Maui.Extensions;
using Alternet.UI;
using Alternet.UI.Extensions;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a handler for popup entry controls in a Maui application.
    /// It provides methods to manage popup entries, including closing all entries, closing active entries,
    /// checking for active entries, and showing new entries. This class implements
    /// the <see cref="IPopupEntryHandler"/> interface for handling popup entries in a Maui application.
    /// </summary>
    public partial class MauiPopupEntryHandler : IPopupEntryHandler
    {
        /// <summary>
        /// Gets default height of the popup entry control. This value is used when
        /// it is not possible to get height of the popup entry control dynamically.
        /// </summary>
        public static float DefaultPopupEntryHeight { get; set; } = 32;

        private readonly List<WeakReferenceValue<BasePopupEntry>> activeEntries = new();

        /// <inheritdoc/>
        public virtual bool CloseActivePopupEntry(ObjectUniqueId id)
        {
            var removed = false;

            for (int i = activeEntries.Count - 1; i >= 0; i--)
            {
                var entry = activeEntries[i].Value;
                if (entry is not null && entry.Params.TargetControl?.UniqueId == id)
                {
                    entry.ResetEventActions();
                    entry.IsVisible = false;
                    entry.Params = new();
                    (entry.Parent as AbsoluteLayout)?.Children.Remove(entry);
                    activeEntries.RemoveAt(i);
                    removed = true;
                }
            }

            return removed;
        }

        /// <inheritdoc/>
        public virtual void CloseAllPopupEntries()
        {
            foreach (var entry in activeEntries)
            {
                var value = entry.Value;

                if (value is not null)
                {
                    value.ResetEventActions();
                    value.IsVisible = false;
                    value.Params = new();
                    (value.Parent as AbsoluteLayout)?.Children.Remove(value);
                }
            }

            activeEntries.Clear();
        }

        /// <inheritdoc/>
        public float GetPopupEntryHeight(AbstractControl? control, Font font, bool hasBorder)
        {
            return DefaultPopupEntryHeight;
        }

        /// <inheritdoc/>
        public virtual bool HasActivePopupEntry(ObjectUniqueId id)
        {
            for (int i = activeEntries.Count - 1; i >= 0; i--)
            {
                var entry = activeEntries[i].Value;
                if (entry is not null && entry.Params.TargetControl?.UniqueId == id)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public virtual bool ShowPopupEntry(PopupEntryParams prm)
        {
            var uiControl = prm.ItemContainer;

            if (uiControl is null)
                return false;

            var container = ControlView.GetContainer(uiControl);

            if (container is null)
                return false;

            var absLayout = MauiUtils.GetObjectAbsoluteLayout(uiControl);

            if (absLayout is null)
                return false;

            var entry = MauiUtils.FindViewInContainer<BasePopupEntry>(absLayout);
            if (entry is null)
            {
                entry = new(prm);
                activeEntries.Add(new WeakReferenceValue<BasePopupEntry>(entry));
                entry.ZIndex = int.MaxValue - 1;
            }
            else
            {
                entry.Params = prm;
            }

            entry.IsVisible = false;

            if (entry.Parent is null)
            {
                absLayout.Children.Add(entry);
            }
            else
            {
            }

            var absPosition = MauiUtils.GetAbsolutePositionInParent(container, absLayout);
            var r = prm.ItemRect;
            r.Offset(absPosition.X, absPosition.Y);
            r.Height = -1;

            entry.ResetEventActions();
            entry.WantTab = true;
            entry.WantEscape = true;
            entry.SizeChangedAction = OnEntrySizeChanged;
            entry.TextChangedAction = OnEntryTextChanged;
            entry.CompletedAction = OnEntryCompleted;
            entry.EscapeClickedAction = OnEntryEscapeClicked;
            entry.TabClickedAction = OnEntryTabClicked;
            entry.UnfocusedAction = OnEntryUnfocused;
            entry.BackgroundColor = prm.BackColor?.ToMaui();
            entry.Text = prm.GetItemText?.Invoke();
            entry.Placeholder = prm.EmptyTextHint;
            entry.SelectAllOnFocus = true;

            if (entry.Height > 0)
            {
                OnEntrySizeChanged();
            }
            else
            {
                MauiUtils.SetChildBoundsAbsoluteLayout(entry, r.ToMaui());
            }

            entry.IsVisible = true;
            entry.Focus();
            
            BaseObject.Post(() =>
            {
                entry.SelectAll();
            });

            return true;

            void OnEntryEscapeClicked()
            {
                prm.EscapePressed?.Invoke();
                if (!prm.HideOnEscape)
                    return;
                CloseEntry();
            }

            void OnEntryTabClicked()
            {
                prm.TabPressed?.Invoke();
            }

            void CloseEntry()
            {
                entry.ResetEventActions();
                var id = prm.TargetControl?.UniqueId;
                if (id is not null)
                    CloseActivePopupEntry(id.Value);
            }

            void OnEntryUnfocused()
            {
                CloseEntry();
            }

            void OnEntryCompleted()
            {
                prm.EnterPressed?.Invoke();
                if (!prm.HideOnEnter)
                    return;
                prm.SetItemText?.Invoke(entry.Text);
                CloseEntry();
            }

            void OnEntryTextChanged(string oldText, string newText)
            {
                if (!prm.CommitTextOnKeyPress)
                    return;
                prm.SetItemText?.Invoke(newText);
            }

            void OnEntrySizeChanged()
            {
                var entryHeight = (float)entry.Height;
                prm.EntryHeightChanged?.Invoke(entryHeight);

                var itemHeight = prm.ItemRect.Height;

                r.Top -= (entryHeight - itemHeight) / 2;
                MauiUtils.SetChildBoundsAbsoluteLayout(entry, r.ToMaui());
            }
        }

        private partial class BasePopupEntry : BaseEntry
        {
            private PopupEntryParams prm;

            public BasePopupEntry()
            {
            }

            public BasePopupEntry(PopupEntryParams prm)
            {
                this.prm = prm;
            }

            public PopupEntryParams Params
            {
                get => prm;
                set => prm = value;
            }
        }
    }
}
