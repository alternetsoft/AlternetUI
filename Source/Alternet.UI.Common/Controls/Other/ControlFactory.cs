using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements static methods and properties which are related to the controls creation.
    /// </summary>
    public static class ControlFactory
    {
        private static IControlFactoryHandler? handler;
        private static IPopupEntryHandler? popupEntryHandler;

        /// <summary>
        /// Gets or sets the <see cref="IPopupEntryHandler"/> interface provider.
        /// </summary>
        public static IPopupEntryHandler? PopupEntryHandler
        {
            get
            {
                if (popupEntryHandler is null)
                    popupEntryHandler = Handler.GetPopupEntryHandler();
                return popupEntryHandler;
            }

            set
            {
                popupEntryHandler = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="IControlFactoryHandler"/> which is used to create controls.
        /// </summary>
        public static IControlFactoryHandler Handler
        {
            get
            {
                return handler ??= App.Handler.CreateControlFactoryHandler();
            }

            set
            {
                handler = value;
            }
        }
    }
}
