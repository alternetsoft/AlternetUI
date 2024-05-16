using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class SkiaFontFactoryHandler : DisposableObject, IFontFactoryHandler
    {
        public int DefaultFontEncoding
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IFontHandler CreateDefaultFont()
        {
            throw new NotImplementedException();
        }

        public IFontHandler CreateDefaultMonoFont()
        {
            throw new NotImplementedException();
        }

        public IFontHandler CreateFont()
        {
            throw new NotImplementedException();
        }

        public IFontHandler CreateFont(Font font)
        {
            throw new NotImplementedException();
        }

        public Font CreateSystemFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        public string[] GetFontFamiliesNames()
        {
            throw new NotImplementedException();
        }

        public string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            throw new NotImplementedException();
        }

        public bool IsFontFamilyValid(string name)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultFont(Font value)
        {
            throw new NotImplementedException();
        }
    }
}
