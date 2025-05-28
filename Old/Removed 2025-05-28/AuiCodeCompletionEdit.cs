#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.ComponentModel;

using Alternet.Common;
using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Editor.CodeCompletion
{
    /// <summary>
    /// Represents class that implements <c>ICodeCompletionEdit</c> interface.
    /// This object is used to display a popup window containing Code Completion
    /// information presented in the form of an edit with a label.
    /// </summary>
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    internal class AuiCodeCompletionEdit : AuiCodeCompletionWindow, ICodeCompletionEdit
    {
        private readonly ICodeCompletionBox completionBox;

        /// <summary>
        /// Initializes a new instance of the <c>CodeCompletionEdit</c>
        /// class with specified parameters.
        /// </summary>
        /// <param name="owner">The <c>SyntaxEdit</c> control owning this new instance.</param>
        /// <param name="completionBox">Specifies parent of this new instance.</param>
        public AuiCodeCompletionEdit(Control owner, ICodeCompletionBox completionBox)
            : base(owner)
        {
            this.completionBox = completionBox;

            var canvas = owner?.MeasureCanvas ?? MeasureCanvas;

            MinWidth = 300;

            /* Size = (300, RealFont.GetHeight(canvas) + 2);*/

            if (completionBox != null)
                CompletionFlags = completionBox.CompletionFlags;
            CompletionFlags &= ~(CodeCompletionFlags.AcceptOnDblClick
                | CodeCompletionFlags.AcceptOnEnter | CodeCompletionFlags.AcceptOnDelimiter
                | CodeCompletionFlags.CloseOnEscape | CodeCompletionFlags.AcceptOnEnter);
            CompletionFlags |= CodeCompletionFlags.KeepActive;
            Edit.EditBox.TextChanged += DoTextChanged;

            Closed += (_, _) =>
            {
                App.DebugLogIf($"Code CompletionEdit Closed: {PopupResult}", false);
            };

            SetSizeToContent(WindowSizeToContentMode.WidthAndHeight, 2);
        }

        /// <summary>
        /// Gets or sets a value that indicates caption of the Edit label.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string EditField
        {
            get
            {
                return Edit.Label.Text;
            }

            set
            {
                if (Edit.Label.Text != value)
                {
                    Edit.Label.Text = value;
                    Edit.UpdateSize();
                }
            }
        }

        /// <summary>
        /// Indicates whether <c>CodeCompletionEdit</c> control has an input focus.
        /// </summary>
        public override bool IsFocused
        {
            get
            {
                return Focused || Edit.Focused || Edit.EditBox.Focused || Edit.PathLabel.Focused;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates text of the field being edited.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EditPath
        {
            get
            {
                return Edit.PathLabel.Text;
            }

            set
            {
                if (Edit.PathLabel.Text != value)
                {
                    Edit.PathLabel.Text = value;
                    OnEditPathChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates text of the field being edited.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EditText
        {
            get
            {
                return Edit.EditBox.Text;
            }

            set
            {
                if ((Edit != null) && (Edit.EditBox != null) && (Edit.EditBox.Text != value))
                {
                    Edit.EditBox.Text = value;
                    OnEditTextChanged();
                }
            }
        }

        /// <summary>
        /// Represents a completion list box control.
        /// </summary>
        public virtual ICompletionEdit Edit
        {
            get
            {
                return PopupControl as ICompletionEdit;
            }
        }

        /// <summary>
        /// Retrieves a value indicating whether the popup window contains the specified control.
        /// </summary>
        /// <param name="control">The Control to evaluate.</param>
        /// <returns>True if the popup window contains the specified control;
        /// otherwise, false.</returns>
        public override bool ContainsControl(Control control)
        {
            return base.ContainsControl(control)
                || ((control != null) && ((control == completionBox)
                || (control == completionBox.PopupControl)));
        }

        public override void CloseDelayed(bool accept)
        {
            base.CloseDelayed(accept);
        }

        public override void Close()
        {
            base.Close();
        }

        public override bool SetFocus()
        {
            return PopupControl.SetFocus();
        }

        protected override Control CreatePopupControl()
        {
            return new AuiCompletionEdit();
        }

        protected virtual void DoTextChanged(object sender, EventArgs e)
        {
            completionBox?.PerformSearch();
        }

        protected override bool Close(bool accept, bool setFocus, char keyChar)
        {
            return base.Close(accept, setFocus, keyChar);
        }

        protected override void OnBeforeChildKeyDown(object sender, KeyEventArgs e)
        {
            base.OnBeforeChildKeyDown(sender, e);

            if (e.IsHandledOrSupressed)
                return;

            switch (e.Key)
            {
                case Key.Down:
                case Key.Up:
                case Key.Next:
                case Key.Prior:
                case Key.Enter:
                case Key.Escape:
                    (completionBox as AuiCodeCompletionBox)?.HandleParentKeyDown(e);
                    break;
            }
        }

        protected virtual void OnEditPathChanged()
        {
        }

        protected virtual void OnEditTextChanged()
        {
        }
    }
}
