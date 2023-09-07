using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridPropInfoRegistry : IPropertyGridPropInfoRegistry
    {
        private readonly PropertyInfo propInfo;
        private IPropertyGridNewItemParams? newItemParams;

        public PropertyGridPropInfoRegistry(PropertyInfo propInfo)
        {
            this.propInfo = propInfo;
        }

        public PropertyInfo PropInfo => propInfo;

        public IPropertyGridNewItemParams NewItemParams
        {
            get
            {
                newItemParams ??= PropertyGrid.CreateNewItemParams(propInfo);
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
