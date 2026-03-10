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
        private readonly ValueEditorEMail emailEdit = new("E-mail");

        [IsTextLocalized(true)]
        private readonly ValueEditorUrl urlEdit = new("Url");

        static TextOtherPage()
        {
        }

        public TextOtherPage()
        {
            Margin = 10;

            new Label("These editors validate entered text. Try to enter invalid values to see the validation in action.")
                .SetWordWrap(true).SetMargin(0, 0, 0, 10).SetParent(this);

            Group(emailEdit, urlEdit).InnerSuggestedWidth(200).Margin(0, 0, 0, 5).Parent(this).LabelSuggestedWidthToMax();

        }
    }
}