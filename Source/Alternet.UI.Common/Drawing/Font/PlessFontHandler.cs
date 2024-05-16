using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless <see cref="IFontHandler"/> implementation.
    /// </summary>
    public class PlessFontHandler : DisposableObject, IFontHandler
    {
        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public FontStyle Style
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double SizeInPoints
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Equals(Font font)
        {
            throw new NotImplementedException();
        }

        public int GetEncoding()
        {
            throw new NotImplementedException();
        }

        public int GetNumericWeight()
        {
            throw new NotImplementedException();
        }

        public SizeI GetPixelSize()
        {
            throw new NotImplementedException();
        }

        public bool GetStrikethrough()
        {
            throw new NotImplementedException();
        }

        public bool GetUnderlined()
        {
            throw new NotImplementedException();
        }

        public FontWeight GetWeight()
        {
            throw new NotImplementedException();
        }

        public bool IsFixedWidth()
        {
            throw new NotImplementedException();
        }

        public bool IsUsingSizeInPixels()
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public void Update(IFontHandler.FontParams prm)
        {
            throw new NotImplementedException();
        }
    }
}
