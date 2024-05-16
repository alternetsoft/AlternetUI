using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the color and other settings of the art provider for the
    /// <see cref="AuiManager"/>.
    /// </summary>
    internal interface IAuiDockArt : IDisposableObject
    {
        /// <summary>
        /// Gets the color of a certain setting.
        /// </summary>
        /// <param name="id">Setting identifier.</param>
        Color GetColor(AuiDockArtSetting id);

        /// <summary>
        /// Gets the value of a certain setting.
        /// </summary>
        /// <param name="id">Setting identifier.</param>
        int GetMetric(AuiDockArtSetting id);

        /// <summary>
        /// Sets a certain setting with the value color.
        /// </summary>
        /// <param name="id">Setting identifier.</param>
        /// <param name="color">Color value.</param>
        void SetColor(AuiDockArtSetting id, Color color);

        /// <summary>
        /// Sets the value of a certain setting.
        /// </summary>
        /// <param name="id">Setting identifier.</param>
        /// <param name="value">New setting value.</param>
        void SetMetric(AuiDockArtSetting id, int value);
    }
}
