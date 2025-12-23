using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a log view which internally uses <see cref="Editor"/>.
    /// </summary>
    public partial class EditorLogView : BaseLogView
    {
        private readonly StringBuilder builder = new();
        private readonly Editor editor = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorLogView"/> class.
        /// </summary>
        public EditorLogView()
        {
            editor.IsReadOnly = true;
            Content = editor;
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            builder.Clear();
            UpdateEditor();
        }

        /// <inheritdoc/>
        public override void GoToBegin()
        {
            editor.CursorPosition = 0;
        }

        /// <inheritdoc/>
        protected override void AddItem(string s)
        {
            builder.Append(s + Environment.NewLine);
            UpdateEditor();
        }

        private void UpdateEditor()
        {
            Alternet.UI.App.AddBackgroundInvokeAction(() =>
            {
                var s = builder.ToString();
                editor.Text = s;
                editor.CursorPosition = s.Length - 1;
            });
        }
    }
}
