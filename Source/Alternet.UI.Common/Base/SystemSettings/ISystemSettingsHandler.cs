using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to get different system settings.
    /// </summary>
    public interface ISystemSettingsHandler : IDisposableObject
    {
        /// <summary>
        /// Gets or sets application name.
        /// </summary>
        string AppName { get; set; }

        /// <summary>
        /// Gets or sets application name for the display purposes.
        /// </summary>
        string AppDisplayName { get; set; }

        /// <inheritdoc cref="App.AppClassName"/>
        string AppClassName { get; set; }

        /// <inheritdoc cref="App.VendorName"/>
        string VendorName { get; set; }

        /// <inheritdoc cref="App.VendorDisplayName"/>
        string VendorDisplayName { get; set; }

        /// <inheritdoc cref="App.UseBestVisual"/>
        bool UseBestVisual { get; set; }

        /// <summary>
        /// Creates <see cref="IDisplayFactoryHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IDisplayFactoryHandler CreateDisplayFactoryHandler();

        /// <inheritdoc cref="App.SetNativeTheme"/>
        bool SetNativeTheme(string theme);

        /// <inheritdoc cref="App.SetUseBestVisual"/>
        void SetUseBestVisual(bool flag, bool forceTrueColour = false);

        /// <inheritdoc cref="SystemSettings.AppearanceName"/>
        string GetAppearanceName();

        /// <inheritdoc cref="SystemSettings.AppearanceIsDark"/>
        bool GetAppearanceIsDark();

        /// <inheritdoc cref="SystemSettings.IsUsingDarkBackground"/>
        bool IsUsingDarkBackground();

        /// <inheritdoc cref="SystemSettings.GetMetric(SystemSettingsMetric)"/>
        int GetMetric(SystemSettingsMetric index);

        /// <inheritdoc cref="SystemSettings.HasFeature"/>
        bool HasFeature(SystemSettingsFeature index);

        /// <inheritdoc cref="SystemSettings.GetColor"/>
        ColorStruct? GetColor(KnownSystemColor index);

        /// <inheritdoc cref="SystemSettings.GetMetric(SystemSettingsMetric, AbstractControl)"/>
        int GetMetric(SystemSettingsMetric index, AbstractControl? control);

        /// <summary>
        /// Gets library version.
        /// </summary>
        /// <returns></returns>
        string GetLibraryVersionString();

        /// <summary>
        /// Gets user interface version.
        /// </summary>
        /// <returns></returns>
        string? GetUIVersion();

        /// <summary>
        /// Sets system option.
        /// </summary>
        /// <param name="name">System option name.</param>
        /// <param name="value">System option value.</param>
        void SetSystemOption(string name, int value);

        /// <summary>
        /// Gets language direction.
        /// </summary>
        /// <returns></returns>
        LangDirection GetLangDirection();

        /// <summary>
        /// Gets platform kind.
        /// </summary>
        /// <returns></returns>
        UIPlatformKind GetPlatformKind();

        /// <summary>
        /// Gets default value of the control background color.
        /// </summary>
        /// <param name="controlType">Type of control.</param>
        /// <param name="renderSize">Render Size. Optional.</param>
        /// <returns></returns>
        Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        /// <summary>
        /// Gets default value of the control foreground color.
        /// </summary>
        /// <param name="controlType">Type of control.</param>
        /// <param name="renderSize">Render Size. Optional.</param>
        /// <returns></returns>
        Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        /// <summary>
        /// Gets default value of the control font.
        /// </summary>
        /// <param name="controlType">Type of control.</param>
        /// <param name="renderSize">Render Size. Optional.</param>
        /// <returns></returns>
        Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);
    }
}
