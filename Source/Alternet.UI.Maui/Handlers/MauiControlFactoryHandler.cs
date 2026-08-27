using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal partial class MauiControlFactoryHandler : DisposableObject, IControlFactoryHandler
    {
        private static IPopupEntryHandler? popupEntryHandler;

        public IControlHandler CreatePanelHandler(ContainerControl control)
        {
            return new MauiControlHandler();
        }

        public IPopupEntryHandler? GetPopupEntryHandler()
        {
            return popupEntryHandler ??= new Alternet.Maui.MauiPopupEntryHandler();
        }

        IControlHandler IControlFactoryHandler.CreateCalendarHandler(Calendar control)
        {
            return new HandlerForDisposed();
        }

        IPropertyGridChoices IControlFactoryHandler.CreateChoices()
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateControlHandler(AbstractControl control)
        {
            return new MauiControlHandler();
        }

        IControlHandler IControlFactoryHandler.CreateTextBoxHandler(TextBox control)
        {
            return new HandlerForDisposed();
        }

        IPropertyGridVariant IControlFactoryHandler.CreateVariant()
        {
            throw new NotImplementedException();
        }

        IControlHandler IControlFactoryHandler.CreateWindowHandler(Window window)
        {
            return new MauiWindowHandler();
        }
    }
}
