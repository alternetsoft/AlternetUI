using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class PropertyGrid
    {
        public void OnPlatformEventSelected()
        {
            (UIControl as UI.PropertyGrid)?.RaisePropertySelected(EventArgs.Empty);
        }
        public void OnPlatformEventChanged()
        {
            (UIControl as UI.PropertyGrid)?.RaisePropertyChanged(EventArgs.Empty);
        }

        public void OnPlatformEventChanging(CancelEventArgs cea)
        {
            (UIControl as UI.PropertyGrid)?.RaisePropertyChanging(cea);
        }

        public void OnPlatformEventHighlighted()
        {
            (UIControl as UI.PropertyGrid)?.RaisePropertyHighlighted(EventArgs.Empty);
        }

        public void OnPlatformEventButtonClick()
        {
            (UIControl as UI.PropertyGrid)?.RaiseButtonClick(EventArgs.Empty);
        }

        public void OnPlatformEventRightClick()
        {
            (UIControl as UI.PropertyGrid)?.RaisePropertyRightClick(EventArgs.Empty);
        }

        public void OnPlatformEventDoubleClick()
        {
            (UIControl as UI.PropertyGrid)?.RaisePropertyDoubleClick(EventArgs.Empty);
        }

        public void OnPlatformEventItemCollapsed()
        {
            (UIControl as UI.PropertyGrid)?.RaiseItemCollapsed(EventArgs.Empty);
        }

        public void OnPlatformEventItemExpanded()
        {
            (UIControl as UI.PropertyGrid)?.RaiseItemExpanded(EventArgs.Empty);
        }

        public void OnPlatformEventLabelEditBegin(CancelEventArgs cea)
        {
            (UIControl as UI.PropertyGrid)?.RaiseLabelEditBegin(cea);
        }

        public void OnPlatformEventLabelEditEnding(CancelEventArgs cea)
        {
            (UIControl as UI.PropertyGrid)?.RaiseLabelEditEnding(cea);
        }

        public void OnPlatformEventColBeginDrag(CancelEventArgs cea)
        {
            (UIControl as UI.PropertyGrid)?.RaiseColBeginDrag(cea);
        }

        public void OnPlatformEventColDragging()
        {
            (UIControl as UI.PropertyGrid)?.RaiseColDragging(EventArgs.Empty);
        }

        public void OnPlatformEventColEndDrag()
        {
            (UIControl as UI.PropertyGrid)?.RaiseColEndDrag(EventArgs.Empty);
        }
    }
}