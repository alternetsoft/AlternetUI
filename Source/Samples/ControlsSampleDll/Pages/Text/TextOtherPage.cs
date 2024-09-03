using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class TextOtherPage : VerticalStackPanel
    {
        [IsTextLocalized(true)]
        private readonly ValueEditorEMail emailEdit = new("E-mail")
        {
            Margin = new(0, 0, 0, 5),
            InnerSuggestedWidth = 200,
        };

        [IsTextLocalized(true)]
        private readonly ValueEditorUrl urlEdit = new("Url")
        {
            Margin = new(0, 0, 0, 5),
            InnerSuggestedWidth = 200,
        };

        public TextOtherPage()
        {
            Margin = 10;
            Group(emailEdit, urlEdit).Parent(this).LabelSuggestedWidthToMax();
        }
    }
}