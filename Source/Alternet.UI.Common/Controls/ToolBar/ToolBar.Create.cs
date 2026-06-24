using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class ToolBar
    {
        /// <summary>
        /// Creates a <see cref="PictureBox"/> control for use in a toolbar.
        /// Override this method to provide custom initialization for the created <see cref="PictureBox"/> control.
        /// </summary>
        /// <returns>A new instance of <see cref="PictureBox"/> representing
        /// a picture box in the toolbar.</returns>
        protected virtual PictureBox CreatePictureBox()
        {
            var result = new PictureBox();
            return result;
        }

        /// <summary>
        /// Creates a new instance of a tool separator item for use in a toolbar.
        /// Override this method to provide custom initialization
        /// for the created <see cref="ToolBarSeparatorItem"/> control.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes
        /// to customize the creation of the tool separator item.
        /// By default, it returns a new instance of <see cref="ToolBarSeparatorItem"/>.</remarks>
        /// <returns>A new instance of <see cref="ToolBarSeparatorItem"/> representing
        /// a separator in the toolbar.</returns>
        protected virtual ToolBarSeparatorItem CreateToolSeparator()
        {
            ToolBarSeparatorItem border = new();
            return border;
        }

        /// <summary>
        /// Creates a new instance of a spacer item for use in a toolbar.
        /// Override this method to provide custom initialization for the created <see cref="Spacer"/> control.
        /// </summary>
        /// <returns>A new instance of <see cref="Spacer"/> representing
        /// a spacer in the toolbar.</returns>
        protected virtual Spacer CreateToolSpacer()
        {
            Spacer spacer = new();
            return spacer;
        }

        /// <summary>
        /// Creates control for use in the toolbar as a label.
        /// Override to create customized label controls.
        /// By default returns a new instance of <see cref="Label"/> class.
        /// </summary>
        /// <returns>A new instance of <see cref="Label"/> representing
        /// a label in the toolbar.</returns>
        protected virtual AbstractControl CreateToolLabel()
        {
            return new Label();
        }

        /// <summary>
        /// Creates <see cref="SpeedButton"/> for use in the toolbar.
        /// Override to create customized speed buttons.
        /// </summary>
        /// <returns>A new instance of <see cref="SpeedButton"/> representing
        /// a speed button in the toolbar.</returns>
        protected virtual SpeedButton CreateToolSpeedButton()
        {
            if (customButtonType is null)
                return new SpeedButton();
            return (SpeedButton?)Activator.CreateInstance(customButtonType) ?? new SpeedButton();
        }

        /// <summary>
        /// Creates <see cref="SpeedTextButton"/> for use in the toolbar.
        /// Override to create customized speed text buttons.
        /// </summary>
        /// <returns>A new instance of <see cref="SpeedTextButton"/> representing
        /// a speed text button in the toolbar.</returns>
        protected virtual SpeedButton CreateToolSpeedTextButton()
        {
            if (customButtonType is null)
                return new SpeedTextButton();
            return (SpeedButton?)Activator.CreateInstance(customButtonType) ?? new SpeedTextButton();
        }

        /// <summary>
        /// Creates a control for the specified panel.
        /// Override this method to provide custom control creation logic based on the panel type.
        /// </summary>
        /// <param name="item">The panel for which to create the control.</param>
        /// <param name="suggestedIndex">The suggested index at which to insert the control.</param>
        /// <returns>The created control, or null if no control was created.</returns>
        protected virtual AbstractControl? CreatePanelControl(BarPanel item, int? suggestedIndex = null)
        {
            AbstractControl? panelControl = null;

            var index = suggestedIndex ?? GetPanelControlInsertIndex(item);

            void OnControlClick(object? sender, EventArgs e)
            {
                if (sender is not AbstractControl control)
                    return;

                var id = control.CustomAttr.GetAttribute(BarPanelIdPropName);

                if (id is not ObjectUniqueId uniqueId)
                    return;

                var panel = GetPanelById(uniqueId);
                if (panel is null)
                    return;

                panel.RaiseControlClicked();
            }

            switch (item.Kind)
            {
                default:
                case BarPanelKind.Text:
                    panelControl = InsertTextCore(index, item.Text);
                    panelControl.Click += OnControlClick;
                    break;
                case BarPanelKind.Separator:
                    panelControl = InsertSeparatorCore(index);
                    break;
                case BarPanelKind.PictureBox:
                    panelControl = InsertPictureCore(index, item.ImageSet, item.DisabledImageSet, item.ToolTip);
                    panelControl.Click += OnControlClick;
                    break;
                case BarPanelKind.SpeedButton:
                    panelControl = InsertSpeedBtnCore(
                                index,
                                ItemKind.Button,
                                item.Text,
                                item.ImageSet,
                                item.DisabledImageSet,
                                item.ToolTip,
                                OnControlClick);
                    break;
                case BarPanelKind.TextButton:
                    panelControl = InsertTextBtnCore(index, item.Text, item.ToolTip, OnControlClick);
                    break;
                case BarPanelKind.ProgressBar:
                    StdProgressBar progressBar = new()
                    {
                        MinHeight = DefaultProgressBarSize.Height,
                        AutoSize = false,
                        VerticalAlignment = VerticalAlignment.Center,
                        SuggestedWidth = DefaultProgressBarSize.Width,
                        SuggestedHeight = DefaultProgressBarSize.Height,
                    };

                    InsertControl(index, progressBar);
                    panelControl = progressBar;
                    break;
                case BarPanelKind.Spacer:
                    float? spacerWidth = float.IsNaN(item.Width) ? null : item.Width;
                    panelControl = InsertSpacerCore(index, spacerWidth);
                    break;
                case BarPanelKind.CustomControl:

                    if (item.CustomControl is not null)
                    {
                        InsertControl(index, item.CustomControl);
                        panelControl = item.CustomControl;
                    }

                    break;
            }

            if (panelControl != null)
            {
                panelControl.CustomAttr.SetAttribute(BarPanelIdPropName, item.UniqueId);
                item.RaiseControlCreated();
                UpdatePanelControl(item);
            }

            return panelControl;
        }
    }
}
