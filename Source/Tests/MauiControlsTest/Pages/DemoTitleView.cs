using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllQuickStarts.Pages
{
    public class DemoTitleView: Label
    {
        public DemoTitleView(string title)
        {
            Text = title;
            Margin = new Thickness(5, 5, 5, 5);

            var vo = VerticalOptions;
            vo.Alignment = LayoutAlignment.Center;
            VerticalOptions = vo;

            var ho = HorizontalOptions;
            ho.Alignment = LayoutAlignment.Center;
            HorizontalOptions = vo;
        }
    }
}
