using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="PropertyGrid"/> control.
    /// </summary>
    public class PopupPropertyGrid : PopupWindow<PropertyGrid>
    {
        static PopupPropertyGrid()
        {
            PropertyGrid.RegisterCollectionEditors();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupPropertyGrid"/> class.
        /// </summary>
        public PopupPropertyGrid()
        {
            HideOnEnter = false;
            HideOnClick = false;
            HideOnDoubleClick = false;
            HideOnDeactivate = false;
        }

        /// <summary>
        /// Creates properties popup window.
        /// </summary>
        /// <remarks>
        /// In order to use properties popup window you can call <see cref="PropertyGrid.SetProps"/>
        /// for the <see cref="PopupWindow{T}.MainControl"/> and show the popup with call
        /// to <see cref="PopupWindow{T}.ShowPopup(AbstractControl)"/>. Thats it!
        /// </remarks>
        /// <returns></returns>
        public static PopupPropertyGrid CreatePropertiesPopup()
        {
            PopupPropertyGrid popupWindowProps = new()
            {
                Title = CommonStrings.Default.WindowTitleProperties,
                HasTitleBar = true,
                HasBorder = true,
                Resizable = true,
                CloseEnabled = true,
                TopMost = true,
            };

            popupWindowProps.MainControl.SuggestedInitDefaults();
            popupWindowProps.MainControl.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAllAfterSetValue;
            popupWindowProps.AfterHide += PopupWindowProps_AfterHide;
            popupWindowProps.Size = (500, 800);
            popupWindowProps.BottomToolBar.Visible = false;
            return popupWindowProps;

            static void PopupWindowProps_AfterHide(object? sender, EventArgs e)
            {
                (sender as PopupPropertyGrid)?.MainControl.Clear();
            }
        }

        /// <inheritdoc/>
        public override void HidePopup(ModalResult result)
        {
            if (!MainControl.ClearSelection(true))
                return;
            base.HidePopup(result);
        }

        /// <inheritdoc/>
        protected override PropertyGrid CreateMainControl()
        {
            return new PropertyGrid()
            {
                HasBorder = false,
            };
        }
    }
}
