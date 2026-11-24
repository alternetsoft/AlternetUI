using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a structure that provides access to flags and attributes associated with an item.
    /// </summary>
    public struct FlagsAndAttributesStruct
    {
        private readonly Lock locker = new();

        private IFlagsAndAttributes? flagsAndAttributes;
        private IIntFlagsAndAttributes? intFlagsAndAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagsAndAttributesStruct"/> structure.
        /// </summary>
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
