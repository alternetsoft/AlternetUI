﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.Drawing
{
    public interface ISkiaSurface : IDisposable
    {
        ImageLockMode LockMode { get; }

        int Width { get; }

        int Height { get; }

        SKColorType ColorType { get; }

        SKAlphaType AlphaType { get; }

        SKSurface? Surface { get; }

        SKBitmap? Bitmap { get; }

        SKCanvas Canvas { get; }

        bool IsOk { get; }
    }
}
