using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents configurable font settings that control text rendering behavior.
    /// </summary>
    /// <remarks>This class provides properties to adjust font rendering parameters
    /// such as scaling, subpixel rendering, hinting, edging and other.
    /// These settings are applied globally to the default font rendering
    /// configuration. Some of the settings will affect only on newly created fonts,
    /// for the existing fonts, they will not have any effect until they are recreated.</remarks>
    public class SkiaFontSettings : BaseObjectWithNotify
    {
        /// <summary>
        /// Occurs when any of the <see cref="SkiaFontSettings"/> instances is changed.
        /// </summary>
        /// <remarks>This event is triggered whenever a change is made.
        /// Subscribers can use this event to respond to updates in settings,
        /// such as refreshing UI elements or
        /// reloading configuration-dependent resources.</remarks>
        public static event EventHandler? SettingsChanged;

        /// <inheritdoc cref="Font.DefaultSkiaTextScaleX"/>.
        public float TextScaleX
        {
            get => Font.DefaultSkiaTextScaleX;
            set
            {
                Font.DefaultSkiaTextScaleX = value;
                RaisePropertyChanged(nameof(TextScaleX));
            }
        }

        /// <inheritdoc cref="Font.DefaultSkiaFontSubpixel"/>.
        public bool Subpixel
        {
            get => Font.DefaultSkiaFontSubpixel;
            set
            {
                Font.DefaultSkiaFontSubpixel = value;
                RaisePropertyChanged(nameof(Subpixel));
            }
        }

        /// <inheritdoc cref="Font.DefaultSkiaFontHinting"/>.
        public SKFontHinting Hinting
        {
            get => Font.DefaultSkiaFontHinting;
            set
            {
                Font.DefaultSkiaFontHinting = value;
                RaisePropertyChanged(nameof(Hinting));
            }
        }

        /// <inheritdoc cref="Font.DefaultSkiaTextScaleX"/>.
        public SKFontEdging Edging
        {
            get => Font.DefaultSkiaFontEdging;
            set
            {
                Font.DefaultSkiaFontEdging = value;
                RaisePropertyChanged(nameof(Edging));
            }
        }

        /// <inheritdoc cref="Font.DefaultSkiaFontForceAutoHinting"/>.
        public bool ForceAutoHinting
        {

            get => Font.DefaultSkiaFontForceAutoHinting;

            set
            {
                Font.DefaultSkiaFontForceAutoHinting = value;
                RaisePropertyChanged(nameof(ForceAutoHinting));
            }
        }

        /// <summary>
        /// Shows the font settings dialog with adjustable options which allows to
        /// customize text rendering.
        /// </summary>
        /// <param name="onClose">The optional callback to be invoked
        /// when the dialog is accepted.</param>
        public static void ShowFontSettingsDialog(Action? onClose = null)
        {
            PopupPropertyGrid.ShowPropertiesPopup(new SkiaFontSettings(), onClose);
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}