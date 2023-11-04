using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorCustom : TextBoxAndLabel
    {
        public ValueEditorCustom(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorCustom()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.Options |= TextBoxOptions.DefaultValidation;
        }
    }
}
