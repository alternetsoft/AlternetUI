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
    public partial class PopupPropertyGrid : PopupWindow<PropertyGrid>
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
        /// Shows properties popup window with specified object as a source of properties.
        /// </summary>
        /// <param name="propSource">The object to retrieve properties from.</param>
        /// <param name="onClose">The optional callback to be invoked
        /// when the dialog is accepted.</param>
        public static void ShowPropertiesPopup(object propSource, Action? onClose = null)
        {
            var dialog = PopupPropertyGrid.CreatePropertiesPopup();
            dialog.MainControl.SetProps(propSource);
            dialog.ShowPopup(
                HorizontalAlignment.Right,
                VerticalAlignment.Top,
                null,
                shrinkSize: true);
            dialog.AfterHide += (s, e) =>
            {
                if (dialog.IsPopupAccepted)
                {
                    onClose?.Invoke();
                }
            };
        }

        /// <summary>
        /// Creates properties popup window.
        /// </summary>
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
        public override void HidePopup(ModalResult result, bool focusOwner = true)
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
