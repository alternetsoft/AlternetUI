﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public abstract class PlessImageHandler : DisposableObject, IImageHandler
    {
        public virtual double ScaleFactor
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual SizeI DipSize
        {
            get => throw new NotImplementedException();
        }
        
        public virtual double ScaledHeight
        {
            get => throw new NotImplementedException();
        }
        
        public virtual SizeI ScaledSize
        {
            get => throw new NotImplementedException();
        }
        
        public virtual double ScaledWidth
        {
            get => throw new NotImplementedException();
        }

        public virtual SizeI PixelSize
        {
            get => throw new NotImplementedException();
        }
        
        public virtual bool IsOk
        {
            get => throw new NotImplementedException();
        }
        
        public virtual bool HasAlpha
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        
        public virtual int Depth
        {
            get => throw new NotImplementedException();
        }

        public virtual IImageHandler ConvertToDisabled(byte brightness = 255)
        {
            throw new NotImplementedException();
        }

        public virtual IImageHandler GetSubBitmap(RectI rect)
        {
            throw new NotImplementedException();
        }

        public virtual bool GrayScale()
        {
            throw new NotImplementedException();
        }

        public virtual bool Load(string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public virtual bool LoadFromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public virtual bool LoadFromStream(Stream stream, BitmapType type)
        {
            return LoadFromStream(stream);
        }

        public virtual void Rescale(SizeI sizeNeeded)
        {
            throw new NotImplementedException();
        }

        public virtual void ResetAlpha()
        {
            throw new NotImplementedException();
        }

        public virtual bool SaveToFile(string name, int quality)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(name);
                var bitmapType = Image.GetBitmapTypeFromFileName(name);
                return SaveToStream(stream, bitmapType, quality);
            });
        }

        public virtual bool SaveToFile(string name, BitmapType type, int quality)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(name);
                return SaveToStream(stream, type, quality);
            });
        }

        public virtual bool SaveToStream(Stream stream, ImageFormat format, int quality)
        {
            var bitmapType = format.AsBitmapType();
            if (bitmapType == BitmapType.Invalid)
                return false;
            return SaveToStream(stream, bitmapType, quality);
        }

        public virtual bool SaveToStream(Stream stream, BitmapType type, int quality)
        {
            throw new NotImplementedException();
        }

        public virtual GenericImage ToGenericImage()
        {
            throw new NotImplementedException();
        }
    }
}