using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WindowAsControlHandler : WxControlHandler, IWindowHandler
    {
        public Action? StateChanged { get; set; }
        
        public Action<HandledEventArgs<string>>? InputBindingCommandExecuted { get; set; }
        
        public Action<CancelEventArgs>? Closing { get; set; }
        
        public bool ShowInTaskbar { get; set; }
        
        public bool MaximizeEnabled { get; set; }
        
        public bool MinimizeEnabled { get; set; }
        
        public bool CloseEnabled { get; set; }
        
        public bool AlwaysOnTop { get; set; }
        
        public bool IsToolWindow { get; set; }
        
        public bool Resizable { get; set; }
        
        public bool HasTitleBar { get; set; }
        
        public bool HasSystemMenu { get; set; }
        
        public string Title { get; set; } = string.Empty;        

        public bool IsModal { get; }
        
        public bool IsPopupWindow { get; set; }
        
        public WindowStartLocation StartLocation { get; set; }
        
        public bool IsActive { get; }
        
        public WindowState State { get; set; }
        
        public Window[] OwnedWindows => [];
        
        public ModalResult ModalResult { get; set; }
        
        public DisposableObject? StatusBar { get; set; }
        
        Window? IWindowHandler.Control => (Window?)base.Control;

        public void Activate()
        {
        }

        public void AddInputBinding(InputBinding value)
        {
        }

        public void Close()
        {
        }

        public void RemoveInputBinding(InputBinding item)
        {
        }

        public void SetIcon(IconSet? value)
        {
        }

        public void SetMaxSize(SizeD size)
        {
        }

        public void SetMenu(DisposableObject? value)
        {
        }

        public void SetMinSize(SizeD size)
        {
        }

        public void SetOwner(Window? owner)
        {
        }

        public ModalResult ShowModal(IWindow? owner)
        {
            return ModalResult.Canceled;
        }

        public void ShowModalAsync(Window? owner, Action<ModalResult> onResult)
        {
        }
    }
}
