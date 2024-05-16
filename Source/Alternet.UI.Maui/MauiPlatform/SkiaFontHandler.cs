﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI
{
    public class SkiaFontHandler : DisposableObject, IFontHandler
    {
        private readonly SKFont font;

        public SkiaFontHandler()
            : this(new SKFont())
        {
        }

        public SkiaFontHandler(SKFont font)
        {
            this.font = font;
        }

        string IFontHandler.Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IFontHandler.Name
        {
            get
            {
                return font.Name;
            }
        }

        FontStyle IFontHandler.Style
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        double IFontHandler.SizeInPoints
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IFontHandler.Equals(Font font)
        {
            throw new NotImplementedException();
        }

        int IFontHandler.GetEncoding()
        {
            throw new NotImplementedException();
        }

        int IFontHandler.GetNumericWeight()
        {
            throw new NotImplementedException();
        }

        SizeI IFontHandler.GetPixelSize()
        {
            throw new NotImplementedException();
        }

        bool IFontHandler.GetStrikethrough()
        {
            throw new NotImplementedException();
        }

        bool IFontHandler.GetUnderlined()
        {
            throw new NotImplementedException();
        }

        FontWeight IFontHandler.GetWeight()
        {
            throw new NotImplementedException();
        }

        bool IFontHandler.IsFixedWidth()
        {
            throw new NotImplementedException();
        }

        bool IFontHandler.IsUsingSizeInPixels()
        {
            throw new NotImplementedException();
        }

        string IFontHandler.Serialize()
        {
            throw new NotImplementedException();
        }

        void IFontHandler.Update(IFontHandler.FontParams prm)
        {
            throw new NotImplementedException();
        }
    }
}
