using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class TextBoxAndLabel : ControlAndLabel
    {
        public TextBoxAndLabel(string title, string? text = default)
            : this()
        {
            Title = title;
            if (text is not null)
                TextBox.Text = text;
        }

        public TextBoxAndLabel()
            : base()
        {
            MainControl.ValidatorReporter = ErrorPicture;
        }

        public new TextBox MainControl => (TextBox)base.MainControl;

        public TextBox TextBox => (TextBox)base.MainControl;

        /// <inheritdoc/>
        protected override Control CreateControl() => new TextBox();

        public class SByteEditor : TextBoxAndLabel
        {
            public SByteEditor(string title, string? text = default)
                : base(title, text)
            {
            }

            public SByteEditor()
                : base()
            {
            }
        }

        public class ByteEditor : TextBoxAndLabel
        {
            public ByteEditor(string title, string? text = default)
                : base(title, text)
            {
            }

            public ByteEditor()
                : base()
            {
            }
        }

        public class Int16Editor : TextBoxAndLabel
        {
            public Int16Editor(string title, string? text = default)
                : base(title, text)
            {
            }

            public Int16Editor()
                : base()
            {
            }
        }

        public class UInt16Editor : TextBoxAndLabel
        {
            public UInt16Editor(string title, string? text = default)
                : base(title, text)
            {
            }

            public UInt16Editor()
                : base()
            {
            }
        }

        public class Int32Editor : TextBoxAndLabel
        {
            public Int32Editor(string title, string? text = default)
                : base(title, text)
            {
            }

            public Int32Editor()
                : base()
            {
            }
        }

        public class UInt32Editor : TextBoxAndLabel
        {
            public UInt32Editor(string title, string? text = default)
                : base(title, text)
            {
            }

            public UInt32Editor()
                : base()
            {
            }
        }

        public class Int64Editor : TextBoxAndLabel
        {
            public Int64Editor(string title, string? text = default)
                : base(title, text)
            {
            }

            public Int64Editor()
                : base()
            {
            }
        }

        public class UInt64Editor : TextBoxAndLabel
        {
            public UInt64Editor(string title, string? text = default)
                : base(title, text)
            {
            }

            public UInt64Editor()
                : base()
            {
            }
        }

        public class SingleEditor : TextBoxAndLabel
        {
            public SingleEditor(string title, string? text = default)
                : base(title, text)
            {
            }

            public SingleEditor()
                : base()
            {
            }
        }

        public class DoubleEditor : TextBoxAndLabel
        {
            public DoubleEditor(string title, string? text = default)
                : base(title, text)
            {
            }

            public DoubleEditor()
                : base()
            {
            }
        }

        public class USingleEditor : TextBoxAndLabel
        {
            public USingleEditor(string title, string? text = default)
                : base(title, text)
            {
            }

            public USingleEditor()
                : base()
            {
            }
        }

        public class UDoubleEditor : TextBoxAndLabel
        {
            public UDoubleEditor(string title, string? text = default)
                : base(title, text)
            {
            }

            public UDoubleEditor()
                : base()
            {
            }
        }

        public class UInt32HexEditor : TextBoxAndLabel
        {
            public UInt32HexEditor(string title, string? text = default)
                : base(title, text)
            {
            }

            public UInt32HexEditor()
                : base()
            {
            }
        }
    }
}
