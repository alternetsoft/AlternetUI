#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

using Alternet.UI;

using Alternet.Common;
using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    public class EvaluationCodeCompletionController : EvaluationCodeCompletionControllerBase
    {
        private readonly Keys[] codeCompletionAcceptKeys
            = new[] { Keys.Enter, Keys.Space, Keys.Tab, Keys.OemPeriod };

        private readonly Keys[] codeCompletionAcceptNotSuppressedKeys
            = new[] { Keys.Space, Keys.OemPeriod };

        private ICodeCompletionPopup? codeCompletionPopup;

        private IParameterInfoToolTip? parameterInfoToolTip;

        private Timer? expressionTextBoxSelectionChangeTrackingTimer;

        private int expressionTextBoxSelectionStartOld = -1;

        private char? lastExpressionTextBoxPressedKey;

        private Timer? completionDelayTimer;

        public EvaluationCodeCompletionController(
            ISite site,
            IScriptDebuggerBase debugger,
            IExpressionComboBox expressionComboBox,
            RichToolTip toolTip)
            : base(site, debugger)
        {
            ExpressionComboBox = expressionComboBox;
            ToolTip = toolTip;

            expressionComboBox.TextChanged += ExpressionTextBox_TextChanged;
            expressionComboBox.KeyDown += ExpressionTextBox_KeyDown;
            expressionComboBox.KeyPress += ExpressionTextBox_KeyPress;
            expressionComboBox.PreviewKeyDown += ExpressionTextBox_PreviewKeyDown;

            InitializeExpressionTextBoxSelectionChangeTrackingTimer();
            InitializeCompletionDelayTimer();
        }

        public bool IsActive
        {
            get
            {
                return IsCodeCompletionPopupVisible;
            }
        }

        protected IExpressionComboBox ExpressionComboBox { get; private set; }

        protected RichToolTip ToolTip { get; private set; }

        protected override bool IsCodeCompletionPopupVisible
        {
            get
            {
                return CodeCompletionPopup.Visible;
            }
        }

        protected override bool IsExpressionTextBoxFocused
        {
            get
            {
                return ExpressionComboBox.Focused;
            }
        }

        protected override string ExpressionText
        {
            get
            {
                return ExpressionComboBox.Text;
            }

            set
            {
                ExpressionComboBox.Text = value;
            }
        }

        protected override int CursorPositionInExpression
        {
            get
            {
                return ExpressionComboBox.TextSelectionStart;
            }

            set
            {
                ExpressionComboBox.SelectTextRange(value, 0);
            }
        }

        protected virtual ICodeCompletionPopup CodeCompletionPopup
        {
            get
            {
                if (codeCompletionPopup == null)
                {
                    var configuration = new CodeCompletionPopupConfiguration();
                    codeCompletionPopup = CreateCodeCompletionPopup(configuration);
                }

                return codeCompletionPopup;
            }
        }

        protected virtual IParameterInfoToolTip ParameterInfoToolTip
        {
            get
            {
                if (parameterInfoToolTip == null)
                {
                    var configuration = new ParameterInfoToolTipConfiguration(
                        (Control)ExpressionComboBox,
                        ToolTip,
                        ChangeParameterInfoCurrentSymbol);
                    parameterInfoToolTip = CreateParameterInfoToolTip(configuration);
                }

                return parameterInfoToolTip;
            }
        }

        private Point? CodeCompletionPopupDisplayPoint { get; set; }

        protected virtual ICodeCompletionPopup CreateCodeCompletionPopup(CodeCompletionPopupConfiguration configuration)
        {
            return new CodeCompletionPopup((Control)ExpressionComboBox, ToolTip, configuration);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                ExpressionComboBox.TextChanged -= ExpressionTextBox_TextChanged;
                ExpressionComboBox.KeyDown -= ExpressionTextBox_KeyDown;
                ExpressionComboBox.KeyPress -= ExpressionTextBox_KeyPress;
                ExpressionComboBox.PreviewKeyDown -= ExpressionTextBox_PreviewKeyDown;

                Utilities.Dispose(ref expressionTextBoxSelectionChangeTrackingTimer);
                Utilities.Dispose(ref completionDelayTimer);
            }
        }

        protected virtual IParameterInfoToolTip CreateParameterInfoToolTip(ParameterInfoToolTipConfiguration configuration)
        {
            return new ParameterInfoToolTip(configuration);
        }

        protected override void HideParameterInfoTooltip()
        {
            if (ParameterInfoToolTip.Visible)
                ParameterInfoToolTip.Close();
        }

        protected override void HideCodeCompletionPopup()
        {
            base.HideCodeCompletionPopup();

            CodeCompletionPopup.Close();
            CodeCompletionPopupDisplayPoint = null;
        }

        protected override void ShowParameterInfoTooltip(ParameterInfoSymbol symbol)
        {
            ParameterInfoToolTip.ShowPopup(symbol, new Point(0, (int)ExpressionComboBox.Height));
        }

        protected override void SetCodeCompletionPopupItemsCore(IEnumerable<Symbol> items)
        {
            CodeCompletionPopup.SetItems(items);
            ExpressionComboBox.SetItems(items);
        }

        protected override IEnumerable<string> GetCodeCompletionPopupItems()
        {
            return CodeCompletionPopup.GetItems();
        }

        protected override void ShowCodeCompletionPopup()
        {
            CodeCompletionPopup.Show(
                (Control)ExpressionComboBox,
                GetCodeCompletionPopupDisplayPoint());
        }

        protected override void TryShowCodeCompletionDependingOnPressedKey()
        {
            if (lastExpressionTextBoxPressedKey != null)
            {
                TryShowCodeCompletionDependingOnPressedKey(lastExpressionTextBoxPressedKey.Value);
                lastExpressionTextBoxPressedKey = null;
            }
        }

        protected virtual Point CalculateCodeCompletionPopupDisplayPoint()
        {
            var p = Caret.GetPosition((Control)ExpressionComboBox);
            if (p == null)
                return Point.Empty;
            Point pt = p.Value;
            pt.Y += (int)ExpressionComboBox.Height;

            if (ParameterInfoToolTip.Visible)
                pt.Y += (int)ParameterInfoToolTip.GetPreferredSize().Height;
            return pt;
        }

        /// <summary>
        /// Returns popup display point in text box client coordinates.
        /// </summary>
        private Point GetCodeCompletionPopupDisplayPoint()
        {
            if (CodeCompletionPopupDisplayPoint == null)
                CodeCompletionPopupDisplayPoint = CalculateCodeCompletionPopupDisplayPoint();

            return CodeCompletionPopupDisplayPoint.Value;
        }

        private void InitializeExpressionTextBoxSelectionChangeTrackingTimer()
        {
            expressionTextBoxSelectionChangeTrackingTimer = new Timer { Interval = 100 };
            expressionTextBoxSelectionChangeTrackingTimer.Tick += ExpressionTextBoxSelectionChangeTrackingTimer_Tick;
            expressionTextBoxSelectionChangeTrackingTimer.Start();
        }

        private void ExpressionTextBoxSelectionChangeTrackingTimer_Tick(object sender, EventArgs e)
        {
            if (expressionTextBoxSelectionStartOld != ExpressionComboBox.TextSelectionStart)
            {
                expressionTextBoxSelectionStartOld = ExpressionComboBox.TextSelectionStart;
                OnExpressionTextBoxSelectionStartChanged();
            }
        }

        private void InitializeCompletionDelayTimer()
        {
            completionDelayTimer = new Timer
            {
                Interval = (int)CompletionDelay.TotalMilliseconds,
            };

            completionDelayTimer.Tick += (o, e) =>
            {
                completionDelayTimer.Stop();
                DoCodeCompletion();
            };
        }

        private void ExpressionTextBox_TextChanged(object sender, EventArgs e)
        {
            completionDelayTimer?.Start();
        }

        private ChangePopupSelectionCommand? GetChangeSelectionCommand(Keys pressedKey)
        {
            switch (pressedKey)
            {
                case Keys.Up:
                    return ChangePopupSelectionCommand.Up;

                case Keys.Down:
                    return ChangePopupSelectionCommand.Down;

                case Keys.PageUp:
                    return ChangePopupSelectionCommand.PageUp;

                case Keys.PageDown:
                    return ChangePopupSelectionCommand.PageDown;
            }

            return null;
        }

        private void ExpressionTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            lastExpressionTextBoxPressedKey = e.KeyChar;
        }

        private void ExpressionTextBox_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        private void ExpressionTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && e.Control)
            {
                DisableCodeCompletion = false;
                TryShowCodeCompletion(autocompleteOnSingleSuggestion: true);
                e.SuppressKeyPress = true;
            }

            if (codeCompletionAcceptKeys.Contains(e.KeyData))
            {
                if (ParameterInfoToolTip.Visible)
                    HideParameterInfoTooltip();

                if (CodeCompletionPopup.Visible)
                {
                    var selectedItem = CodeCompletionPopup.GetSelectedItem();
                    if (selectedItem == null && e.KeyData == Keys.Tab)
                    {
                        CodeCompletionPopup.ChangeSelection(ChangePopupSelectionCommand.Down);
                        selectedItem = CodeCompletionPopup.GetSelectedItem();
                    }

                    if (selectedItem != null)
                        CompleteExpression(selectedItem);

                    HideCodeCompletionPopup();
                }
                else if (e.KeyData == Keys.Enter)
                    Site.EvaluateCurrentExpressionAsync();

                e.SuppressKeyPress = !codeCompletionAcceptNotSuppressedKeys.Contains(e.KeyData);
            }

            if (e.KeyData == Keys.Escape)
            {
                if (CodeCompletionPopup.Visible)
                {
                    HideCodeCompletionPopup();

                    // disableCodeCompletion = true;
                }
                else if (ParameterInfoToolTip.Visible)
                {
                    HideParameterInfoTooltip();
                }
                else
                    Site.Close();

                // This fixes beeping sounds on Esc key press.
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (CodeCompletionPopup.Visible)
            {
                ProcessCodeCompletionPopupKeys(e);
            }
            else if (ParameterInfoToolTip.Visible)
            {
                ProcessParameterInfoToolTipKeys(e);
            }
        }

        private void ProcessCodeCompletionPopupKeys(KeyEventArgs e)
        {
            var command = GetChangeSelectionCommand(e.KeyData);
            if (command != null)
            {
                e.SuppressKeyPress = true;
                CodeCompletionPopup.ChangeSelection(command.Value);
            }
        }

        private void OnExpressionTextBoxSelectionStartChanged()
        {
            if (ParameterInfoToolTip.Visible)
                TryShowParameterInfo();
        }

        private void ProcessParameterInfoToolTipKeys(KeyEventArgs e)
        {
            int difference = 0;
            if (e.KeyData == Keys.Up)
                difference = -1;
            else if (e.KeyData == Keys.Down)
                difference = 1;

            if (difference != 0)
            {
                e.SuppressKeyPress = true;
                ChangeParameterInfoCurrentSymbol(difference);
            }
        }
    }
}