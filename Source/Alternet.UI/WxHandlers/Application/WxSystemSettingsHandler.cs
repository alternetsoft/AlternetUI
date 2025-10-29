using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxSystemSettingsHandler : DisposableObject, ISystemSettingsHandler
    {
        public UI.Native.Application? NativeApplication => WxApplicationHandler.NativeApplication;

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        /// <remarks>
        /// It is used for paths, config, and other places the user doesn't see.
        /// By default it is set to the executable program name.
        /// </remarks>
        public string AppName
        {
            get => NativeApplication?.Name ?? string.Empty;

            set
            {
                if (NativeApplication is not null)
                    NativeApplication.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the application display name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The display name is the name shown to the user in titles, reports, etc.
        /// while the application name is used for paths, config, and other
        /// places the user doesn't see.
        /// </para>
        /// <para>
        /// By default the application display name is the same as application
        /// name or a capitalized version of the program if the application
        /// name was not set either.
        /// It's usually better to set it explicitly to something nicer.
        /// </para>
        /// </remarks>
        public string AppDisplayName
        {
            get => NativeApplication?.DisplayName ?? string.Empty;

            set
            {
                if (NativeApplication is not null)
                    NativeApplication.DisplayName = value;
            }
        }

        /// <summary>
        /// Gets or sets the application class name.
        /// </summary>
        /// <remarks>
        /// It should be set by the application itself, there are
        /// no reasonable defaults.
        /// </remarks>
        public string AppClassName
        {
            get => NativeApplication?.AppClassName ?? string.Empty;

            set
            {
                if (NativeApplication is not null)
                    NativeApplication.AppClassName = value;
            }
        }

        /// <summary>
        /// Gets or sets the vendor name.
        /// </summary>
        /// <remarks>
        /// It is used in some areas such as configuration, standard paths, etc.
        /// It should be set by the application itself, there are
        /// no reasonable defaults.
        /// </remarks>
        public string VendorName
        {
            get => NativeApplication?.VendorName ?? string.Empty;

            set
            {
                if (NativeApplication is not null)
                    NativeApplication.VendorName = value;
            }
        }

        /// <summary>
        /// Gets or sets whether application will use the best visual on systems that
        /// support different visuals.
        /// </summary>
        public bool UseBestVisual
        {
            get => NativeApplication?.GetUseBestVisual() ?? true;

            set
            {
                NativeApplication?.SetUseBestVisual(value, false);
            }
        }

        /// <summary>
        /// Gets or sets the vendor display name.
        /// </summary>
        /// <remarks>
        /// It is shown in titles, reports, dialogs to the user, while
        /// the vendor name is used in some areas such as configuration,
        /// standard paths, etc.
        /// It should be set by the application itself, there are
        /// no reasonable defaults.
        /// </remarks>
        public string VendorDisplayName
        {
            get => NativeApplication?.VendorDisplayName ?? string.Empty;

            set
            {
                if (NativeApplication is not null)
                    NativeApplication.VendorDisplayName = value;
            }
        }

        public string? GetUIVersion()
        {
            Assembly thisAssembly = typeof(App).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();
            Version? ver = thisAssemblyName?.Version;
            return ver?.ToString();
        }

        public string GetLibraryVersionString()
        {
            return WebBrowser.GetLibraryVersionString();
        }

        public void SetSystemOption(string name, int value)
        {
            Native.Application.SetSystemOptionInt(name, value);
        }

        public UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.WxWidgets;
        }

        public LangDirection GetLangDirection()
        {
            return (LangDirection?)NativeApplication?.GetLayoutDirection()
                ?? LangDirection.LeftToRight;
        }

        public ColorStruct? GetColor(KnownSystemColor index)
        {
            WxSystemSettingsColor systemSettingsColor = WxColorUtils.Convert(index);
            return (ColorStruct)UI.Native.WxOtherFactory.SystemSettingsGetColor((int)systemSettingsColor);
        }

        public int GetMetric(SystemSettingsMetric index, AbstractControl? control)
        {
            return Native.WxOtherFactory.SystemSettingsGetMetric(
                (int)index,
                WxApplicationHandler.WxWidget(control));
        }

        public int GetMetric(SystemSettingsMetric index)
        {
            return Native.WxOtherFactory.SystemSettingsGetMetric((int)index, default);
        }

        public string GetAppearanceName()
        {
            return Native.WxOtherFactory.SystemAppearanceGetName();
        }

        public bool GetAppearanceIsDark()
        {
            return Native.WxOtherFactory.SystemAppearanceIsDark();
        }

        public IDisplayFactoryHandler CreateDisplayFactoryHandler()
        {
            return new WxDisplayFactoryHandler();
        }

        /// <summary>
        /// Allows runtime switching of the UI environment theme.
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public bool SetNativeTheme(string theme)
        {
            return NativeApplication?.SetNativeTheme(theme) ?? false;
        }

        /// <summary>
        /// Allows the programmer to specify whether the application will use the best
        /// visual on systems that support several visual on the same display.
        /// </summary>
        public void SetUseBestVisual(bool flag, bool forceTrueColour = false)
        {
            NativeApplication?.SetUseBestVisual(flag, forceTrueColour);
        }

        /// <inheritdoc/>
        public Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesBgColor(
                (int)controlType,
                (int)renderSize);
        }

        /// <inheritdoc/>
        public Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesFgColor(
                (int)controlType,
                (int)renderSize);
        }

        /// <inheritdoc/>
        public Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            var font = Native.Control.GetClassDefaultAttributesFont(
                (int)controlType,
                (int)renderSize);
            return Font.FromInternal(font);
        }

        public bool IsUsingDarkBackground()
        {
            return Native.WxOtherFactory.SystemAppearanceIsUsingDarkBackground();
        }

        public bool HasFeature(SystemSettingsFeature index)
        {
            return Native.WxOtherFactory.SystemSettingsHasFeature((int)index);
        }

        public Font GetFont(SystemSettingsFont systemFont)
        {
            var fnt = Native.WxOtherFactory.SystemSettingsGetFont((int)systemFont);
            return new Font(fnt);
        }
    }
}
