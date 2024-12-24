using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void InitTextBoxAndButton(object control)
        {
            if (control is not TextBoxAndButton textBox)
                return;
            textBox.Text = "some text";
            textBox.HasBtnEllipsis = true;
            textBox.InnerSuggestedWidth = 200;
            textBox.ButtonClick += (s, e) =>
            {
                var str = textBox.GetBtnName(e.ButtonId);

                if (!string.IsNullOrEmpty(str))
                    str = ": " + str;

                var prefix = "TextBoxAndButon.ButtonClick";

                App.Log($"{prefix}{str}");
            };
        }

        public static void InitTextBoxAndLabel(object control)
        {
            if (control is not TextBoxAndLabel textBox)
                return;
            textBox.Text = "some text";
            textBox.Label.Text = "Label";
            textBox.TextBox.SuggestedWidth = 200;
        }

        public static void InitComboBoxAndLabel(object control)
        {
            if (control is not ComboBoxAndLabel textBox)
                return;
            textBox.Text = "item 1";
            textBox.Label.Text = "Label";
            textBox.ComboBox.SuggestedWidth = 200;
            textBox.ComboBox.Add("item 1");
            textBox.ComboBox.Add("item 2");
            textBox.ComboBox.Add("item 3");
            textBox.ComboBox.Add("item 4");
            textBox.ComboBox.Add("item 5");
        }

        public static void InitTextBox(object control)
        {
            if (control is not TextBox textBox)
                return;
            textBox.Text = "some text";
            textBox.SuggestedWidth = 200;
            textBox.Activated += TextBox_Activated;
            textBox.Deactivated += TextBox_Deactivated;

            static void TextBox_Deactivated(object? sender, EventArgs e)
            {
                App.Log("TextBox Deactivated");
            }

            static void TextBox_Activated(object? sender, EventArgs e)
            {
                App.Log("TextBox Activated");
            }

            textBox.TextChanged += (s, e) =>
            {
                var prefix = "TextBox.Text Changed:";
                var prefixWithText = $"{prefix} <{textBox.Text}>.";

                if (textBox.HasErrors)
                {
                    var errors = textBox.GetErrors();
                    var error = errors.FirstOrDefault();
                    if (error is not null)
                    {
                        App.LogReplace(
                            $"{prefixWithText} Error: {error}",
                            prefix,
                            LogItemKind.Error);
                    }
                }
                else
                {
                    if (textBox.IsNumber)
                    {
                        App.LogReplace(
                            $"{prefixWithText} Number: <{textBox.TextAsNumber}>.", prefix);
                    }
                    else
                    {
                        App.LogReplace(
                            $"{prefixWithText} ", prefix);
                    }
                }
            };
        }

        public static void InitRichTextBox(object control)
        {
            if (control is not RichTextBox textBox)
                return;
            textBox.SuggestedSize = defaultListSize;
            textBox.Text = LoremIpsum;
        }

        public static void InitMultilineTextBox(object control)
        {
            if (control is not MultilineTextBox textBox)
                return;
            textBox.SuggestedSize = defaultListSize;
            textBox.Text = LoremIpsum;
        }
    }
}
