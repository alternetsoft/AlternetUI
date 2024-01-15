using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInit
    {
        public static void InitTextBox(object control)
        {
            if (control is not TextBox textBox)
                return;
            textBox.Text = "some text";
            textBox.SuggestedWidth = 200;
            textBox.Activated += PictureBox_Activated;
            textBox.Deactivated += PictureBox_Deactivated;

            static void PictureBox_Deactivated(object? sender, EventArgs e)
            {
                Application.Log("TextBox Deactivated");
            }

            static void PictureBox_Activated(object? sender, EventArgs e)
            {
                Application.Log("TextBox Activated");
            }
        }

        public static void InitRichTextBox(object control)
        {
            if (control is not RichTextBox textBox)
                return;
            textBox.SuggestedSize = defaultListSize;
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
