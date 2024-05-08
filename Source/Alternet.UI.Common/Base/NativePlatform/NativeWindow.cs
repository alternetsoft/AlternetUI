using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public abstract class NativeWindow
    {
        public static NativeWindow Default = new NotImplementedWindow();

        public abstract Window? GetActiveWindow();

        public abstract Window[] GetOwnedWindows(IWindow window);

        public abstract ModalResult ShowModal(IWindow window, IWindow? owner);

        public abstract ModalResult GetModalResult(IWindow window);

        public abstract void SetModalResult(IWindow window, ModalResult value);

        public abstract void Close(IWindow window);

        public abstract bool GetModal(IWindow window);

        public abstract object CreateWindowHandler();

        public abstract void SetDefaultBounds(RectD defaultBounds);

        public abstract bool IsActive(IWindow window);

        public abstract void SetIsPopupWindow(IWindow window, bool value);

        public abstract WindowStartLocation GetStartLocation(IWindow window);

        public abstract void SetStartLocation(IWindow window, WindowStartLocation value);

        public abstract WindowState GetState(IWindow window);

        public abstract void SetState(IWindow window, WindowState value);

        public abstract void SetStatusBar(
            IWindow window,
            FrameworkElement? oldValue,
            FrameworkElement? value);

        public abstract void Activate(IWindow window);
    }
}
