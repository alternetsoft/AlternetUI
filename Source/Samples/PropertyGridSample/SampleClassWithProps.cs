using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyGridSample
{
    internal class SampleClassWithProps
    {
        private int sampleProp;

        public int SampleProp1
        {
            get => sampleProp;
            set => sampleProp = value;
        }

        public int SampleProp2
        {
            get => sampleProp;
            set => sampleProp = value;
        }
    }
}
