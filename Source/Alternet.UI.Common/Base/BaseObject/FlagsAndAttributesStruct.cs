using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    internal struct FlagsAndAttributesStruct
    {
        private readonly object locker = new();

        private IFlagsAndAttributes? flagsAndAttributes;
        private IIntFlagsAndAttributes? intFlagsAndAttributes;

        public FlagsAndAttributesStruct()
        {
        }

        /// <summary>
        /// Gets or sets flags and attributes provider which allows to
        /// access items using integer identifiers.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public IIntFlagsAndAttributes IntFlagsAndAttributes
        {
            get
            {
                if (intFlagsAndAttributes is null)
                {
                    lock (locker)
                    {
                        intFlagsAndAttributes = FlagsAndAttributesFactory.CreateIntFlagsAndAttributes();
                    }
                }

                return intFlagsAndAttributes;
            }

            set
            {
                intFlagsAndAttributes = value;
            }
        }

        /// <summary>
        /// Gets custom flags and attributes provider associated with the item.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public IFlagsAndAttributes FlagsAndAttributes
        {
            get
            {
                if (flagsAndAttributes is null)
                {
                    lock (locker)
                    {
                        flagsAndAttributes = FlagsAndAttributesFactory.Create();
                    }
                }

                return flagsAndAttributes;
            }

            set
            {
                flagsAndAttributes = value;
            }
        }
    }
}
