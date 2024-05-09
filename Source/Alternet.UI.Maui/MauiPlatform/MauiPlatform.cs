using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class MauiPlatform : NativePlatform
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new SkiaDrawing();
            Default = new MauiPlatform();
            NativeControl.Default = new MauiPlatformControl();
            initialized = true;
        }

        /// <inheritdoc/>
        public override void ProcessPendingEvents()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override LangDirection GetLangDirection()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool SystemSettingsAppearanceIsDark()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override string SystemSettingsAppearanceName()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Color SystemSettingsGetColor(SystemSettingsColor index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Font SystemSettingsGetFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int SystemSettingsGetMetric(SystemSettingsMetric index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool SystemSettingsHasFeature(SystemSettingsFeature index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool SystemSettingsIsUsingDarkBackground()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void BeginBusyCursor()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void EndBusyCursor()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ExitMainLoop()
        {
            throw new NotImplementedException();
        }

        public override bool IsBusyCursor()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int SystemSettingsGetMetric(SystemSettingsMetric index, IControl? control)
        {
            throw new NotImplementedException();
        }
    }
}
