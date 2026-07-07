using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class MauiWindowHandler : MauiControlHandler, IWindowHandler
    {
        private string? title;

        public virtual bool ShowInTaskbar
        {
            get;
            set;
        }

        public virtual bool MaximizeEnabled
        {
            get;
            set;
        }

        public virtual bool MinimizeEnabled
        {
            get;
            set;
        }

        public virtual bool CloseEnabled
        {
            get;
            set;
        }

        public virtual bool AlwaysOnTop
        {
            get;
            set;
        }

        public virtual bool IsToolWindow
        {
            get;
            set;
        }

        public virtual bool Resizable
        {
            get;
            set;
        }

        public override bool HasBorder
        {
            get;
            set;
        }

        public virtual bool HasTitleBar
        {
            get;
            set;
        }

        public virtual bool HasSystemMenu
        {
            get;
            set;
        }

        public virtual string Title
        {
            get => title ?? string.Empty;
            set => title = value;
        }

        public virtual bool IsPopupWindow
        {
            get;
            set;
        }

        public virtual bool IsActive
        {
            get;
        }

        public virtual WindowState State
        {
            get;
            set;
        }

        Window? IWindowHandler.Control => (Window?)Control;

        public virtual void Activate()
        {
        }

        public virtual void Close()
        {
        }

        public virtual void SetIcon(IconSet? value)
        {
        }

        public virtual void SetMaxSize(SizeD size)
        {
        }

        public virtual void SetMinSize(SizeD size)
        {
        }
    }
}
