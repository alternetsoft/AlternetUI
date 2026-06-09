using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with generic platform independent image.
    /// </summary>
    public interface IGenericImageHandler : IDisposable, ILockImageBits
    {
        /// <inheritdoc cref="GenericImage.Pixels"/>
        SKColor[] Pixels { get; set; }

        /// <inheritdoc cref="GenericImage.HasAlpha"/>
        bool HasAlpha { get; }

        /// <inheritdoc cref="GenericImage.HasMask"/>
        bool HasMask { get; }

        /// <inheritdoc cref="GenericImage.IsOk"/>
        bool IsOk { get; }
    }
}
