using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ListEditSourceGradientStops : ListEditSource
    {
        public override IEnumerable? RootItems
        {
            get
            {
                var value = PropInfo?.GetValue(Instance);
                var result = value as IEnumerable;
                return result;
            }
        }

        public override object? CreateNewItem() => new GradientStop();

        public override object CloneItem(object item)
        {
            var result = new GradientStop();
            var gradientStop = item as GradientStop;
            if(gradientStop != null)
                result.Assign(gradientStop);
            return result;
        }

        public override object? GetProperties(object item)
        {
            return item;
        }

        public override void ApplyData(IEnumerableTree tree)
        {
            List<GradientStop> value = new();
            value.AddRange(GetItems<GradientStop>(tree));
            GradientStop[] asArray = value.ToArray();
            PropInfo?.SetValue(Instance, asArray);
        }
    }
}