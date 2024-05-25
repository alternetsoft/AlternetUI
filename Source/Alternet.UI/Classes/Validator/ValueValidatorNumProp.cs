using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ValueValidatorNumProp : ValueValidatorText
    {
        public ValueValidatorNumProp(
            ValueValidatorNumStyle numericType,
            int valueBase = 10)
            : base(
                  Native.ValidatorNumericProperty.CreateValidatorNumericProperty(
                      (int)numericType,
                      valueBase),
                  true)
        {
        }

        protected override void DisposeUnmanaged()
        {
            Native.ValidatorNumericProperty.DeleteValidatorNumericProperty(Handle);
        }
    }
}
