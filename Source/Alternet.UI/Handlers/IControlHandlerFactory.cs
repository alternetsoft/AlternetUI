using System;
using System.Collections.ObjectModel;

namespace Alternet.UI
{
    public interface IControlHandlerFactory
    {
        ControlHandler CreateControlHandler(Control control);
    }
}