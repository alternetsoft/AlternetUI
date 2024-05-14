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
    }
}
