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

        public Action<HandledEventArgs<string>>? InputBindingCommandExecuted { get; set; }

        public Action<CancelEventArgs>? Closing { get; set; }

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

        public Action? StateChanged
        {
            get;
            set;
        }

        public virtual string Title
        {
            get => title ?? string.Empty;
            set => title = value;
        }

        public virtual bool IsModal
        {
            get;
        }

        public virtual bool IsPopupWindow
        {
            get;
            set;
        }

        public virtual WindowStartLocation StartLocation
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

        public virtual Window[] OwnedWindows { get; } = Array.Empty<Window>();

        public virtual ModalResult ModalResult
        {
            get;
            set;
        }

        public virtual DisposableObject? StatusBar
        {
            get;
            set;
        }

        Window? IWindowHandler.Control => (Window?)Control;

        public virtual void Activate()
        {
        }

        public virtual void AddInputBinding(InputBinding value)
        {
        }

        public virtual void Close()
        {
        }

        public virtual void RemoveInputBinding(InputBinding item)
        {
        }

        public virtual void SetIcon(IconSet? value)
        {
        }

        public virtual void SetMaxSize(SizeD size)
        {
        }

        public virtual void SetMenu(DisposableObject? value)
        {
        }

        public virtual void SetMinSize(SizeD size)
        {
        }

        public virtual void SetOwner(Window? owner)
        {
        }

        public virtual ModalResult ShowModal(IWindow? owner)
        {
            return ModalResult.Canceled;
        }

        public void ShowModalAsync(Window? owner, Action<ModalResult> onResult)
        {
            onResult(ModalResult.Canceled);
        }
    }
}
