using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridPropInfoRegistry : IPropertyGridPropInfoRegistry
    {
        private IPropertyGridNewItemParams? newItemParams;

        public IPropertyGridNewItemParams NewItemParams
        {
            get
            {
                newItemParams ??= PropertyGrid.CreateNewItemParams();
                return newItemParams;
            }

            set
            {
                newItemParams = value;
            }
        }

        public bool HasNewItemParams => newItemParams != null;
    }
}
