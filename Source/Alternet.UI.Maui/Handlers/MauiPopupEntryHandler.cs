using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

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
            return false;
        }
    }
}
