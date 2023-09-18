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
        public override object? CreateNewItem() => new GradientStop();

        public override object CloneItem(object item)
        {
            var result = new GradientStop();
            var gradientStop = item as GradientStop;
            if(gradientStop != null)
                result.Assign(gradientStop);
            return result;
        }

        public override void ApplyData(IEnumerableTree tree)
        {
            ApplyDataAsArray<GradientStop>(tree);
        }
    }
}