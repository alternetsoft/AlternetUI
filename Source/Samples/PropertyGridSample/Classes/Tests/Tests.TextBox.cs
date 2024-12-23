using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        internal void TestMemoFind()
        {
            TestMemoFindReplace(false);
        }

        internal void TestMemoReplace()
        {
            TestMemoFindReplace(true);
        }

        void TestRichFindReplace(bool replace)
        {
            var control = GetSelectedControl<RichTextBox>();
            if (control is null)
                return;

        }

        internal void TestRichFind()
        {
            TestRichFindReplace(false);
        }

        internal void TestRichReplace()
        {
            TestRichFindReplace(true);
        }

        void InitTestsTextBox()
        {
            AddControlAction<TextBox>("Edit sbyte", (c) =>
            {
                c.SetValueAndValidator((sbyte)5, true);
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionStart++", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionStart += 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionStart--", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionStart -= 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionLength--", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionLength -= 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("SelectionLength++", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;
                control.SelectionLength += 1;
            });

            PropertyGrid.AddSimpleAction<TextBox>("Change SelectedText", () =>
            {
                var control = GetSelectedControl<TextBox>();
                if (control is null)
                    return;

                TextFromUserParams prm = new();
                prm.OnApply = (s) =>
                {
                    control.SelectedText = s ?? string.Empty;
                };

                DialogFactory.GetTextFromUserAsync(prm);
            });
        }
    }
}