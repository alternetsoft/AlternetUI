using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IWindowHandler : IControlHandler
    {
        bool ShowInTaskbar { get; set; }
        
        bool MaximizeEnabled { get; set; }

        bool MinimizeEnabled { get; set; }
        
        bool CloseEnabled { get; set; }
        
        bool AlwaysOnTop { get; set; }
        
        bool IsToolWindow { get; set; }
        
        bool Resizable { get; set; }
        
        bool HasBorder { get; set; }
        
        bool HasTitleBar { get; set; }
        
        bool HasSystemMenu { get; set; }
        
        void SetIcon(IconSet? value);
        
        void SetMenu(object? value);
        
        void SetToolBar(object? value);
        
        Action? StateChanged { get; set; }

        string Title { get; set; }

        bool IsModal { get; }

        bool IsPopupWindow { get; set; }

        WindowStartLocation StartLocation { get; set; }

        bool IsActive { get; }

        WindowState State { get; set; }

        Window[] OwnedWindows { get; }

        ModalResult ModalResult { get; set; }

        object? StatusBar { get; set; }

        ModalResult ShowModal(IWindow? owner);

        void Close();

        void Activate();

        void AddInputBinding(InputBinding value);

        void RemoveInputBinding(InputBinding item);
    }
}
