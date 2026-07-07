using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Maui.Extensions;
using Alternet.UI;
using Alternet.UI.Extensions;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    internal class MauiPopupEntryHandler : IPopupEntryHandler
    {
        /// <inheritdoc/>
        public virtual bool CloseActivePopupEntry(ObjectUniqueId id)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual void CloseAllPopupEntries()
        {
        }

        /// <inheritdoc/>
        public virtual bool HasActivePopupEntry(ObjectUniqueId id)
        {
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
            if(entry is null)
            {
                entry = new (prm);
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
            MauiUtils.SetChildBoundsAbsoluteLayout(entry, r.ToMaui());
            entry.IsVisible = true;

            return true;
        }

        private partial class BasePopupEntry : BaseEntry
        {
            private PopupEntryParams prm;

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
