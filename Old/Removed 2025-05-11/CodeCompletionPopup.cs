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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using Alternet.Common;
using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;
using Alternet.UI;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Represents a pop-up window for Code Completion controller.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Windows declared const")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1405:DebugAssertMustProvideMessageText", Justification = "Reviewed.")]
    //public class CodeCompletionPopup : Popup, ICodeCompletionPopup
    public class CodeCompletionPopup : Object, ICodeCompletionPopup
    {
        private const int ItemsInPage = 10;

        //private ListBoxWithImages listBox;
        private readonly CodeCompletionListBox listBox = new()
        {
        };

        private Control ownerControl;
        private RichToolTip toolTip;

        private Stopwatch doubleClickStopwatch = new Stopwatch();

        //private MessageFilter messageFilter;

        private CodeCompletionPopupConfiguration configuration;

        private Timer documentationPopupDelayTimer;

        private ParameterInfoToolTip documentationPopup;

        public bool Visible
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <c>CodeCompletionPopup</c> class with specified settings.
        /// </summary>
        /// <param name="ownerControl">The control owning this new instance.</param>
        /// <param name="configuration">The <c>CodeCompletionPopupConfiguration</c>
        /// corresponding to this new instance.</param>
        public CodeCompletionPopup(
            Control ownerControl,
            RichToolTip toolTip,
            CodeCompletionPopupConfiguration configuration)
            //: base(/*new ListBoxWithImages()*/)
        {
            this.ownerControl = ownerControl;
            this.toolTip = toolTip;
            this.configuration = configuration;

            //listBox = (ListBoxWithImages)Content;
            //listBox.Font = new Font("Segoe UI", 9);

            const int VerticalItemPadding = 2;
            //listBox.ItemHeight = listBox.Font.Height + (VerticalItemPadding * 2);
            //listBox.MaximumSize = new Size(int.MaxValue, int.MaxValue);
            //listBox.IntegralHeight = false;
            //listBox.ImageList = configuration.ImageList;
            //listBox.AlphaImageList = configuration.AlphaImageList;
            //MainControl.HasBorder = false;
            listBox.HasBorder = false;
            //listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.Margin = new Thickness();
            listBox.Padding = new Thickness();

            listBox.SelectionChanged += (o, e) => OnListBoxSelectionChanged();
            //ShowingAnimation = PopupAnimations.None;
            //HidingAnimation = PopupAnimations.None;
            //FocusOnOpen = false;
            //AutoClose = false;
        }

        /// <summary>
        /// Updates <c>Symbol</c> collection to the pop-up control.
        /// </summary>
        /// <param name="items">List of <c>Symbol</c> to be set.</param>
        public void SetItems(IEnumerable<Symbol> items)
        {
            listBox.BeginUpdate();
            try
            {
                var listBoxItems = listBox.Items;
                listBoxItems.Clear();
                listBoxItems.AddRange(items.Select(
                    x => new ListControlItem(
                        x.Name,
                        x.Documentation)).ToArray());
            }
            finally
            {
                listBox.EndUpdate();
                //SetSizeToContent();
            }

            listBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Closes pop-up control.
        /// </summary>
        public virtual void Close()
        {
            //base.Close();

            CloseDocumentationPopup();
        }

        /// <summary>
        /// Changes selection accordingly to specified command.
        /// </summary>
        /// <param name="command">Specifies selection command.</param>
        public void ChangeSelection(ChangePopupSelectionCommand command)
        {
            int shiftAmount = 0;
            switch (command)
            {
                case ChangePopupSelectionCommand.Up:
                    shiftAmount = -1;
                    break;

                case ChangePopupSelectionCommand.Down:
                    shiftAmount = 1;
                    break;

                case ChangePopupSelectionCommand.PageUp:
                    shiftAmount = -ItemsInPage;
                    break;

                case ChangePopupSelectionCommand.PageDown:
                    shiftAmount = ItemsInPage;
                    break;

                default:
                    throw Check.GetEnumValueNotSupportedException(command);
            }

            if (shiftAmount != 0)
                ShiftSelection(shiftAmount);
        }

        /// <summary>
        /// Finds out currently selected item in the pop-up window.
        /// </summary>
        /// <returns>Selected item in a string form.</returns>
        public string? GetSelectedItem()
        {
            if (listBox.Items.Count == 0 || listBox.SelectedIndex < 0)
                return null;

            return listBox.SelectedItem?.Text;
        }

        /// <summary>
        /// Represents list of items in a string form.
        /// </summary>
        /// <returns>List of items.</returns>
        public IEnumerable<string> GetItems()
        {
            return listBox.Items.Cast<ListControlItem>().Select(x => x.Text);
        }

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_ACTIVATEAPP = 0x1c;
        //    const int WM_MOUSEACTIVATE = 0x0021;
        //    const int MA_NOACTIVATEANDEAT = 4;

        //    if (m.Msg == WM_ACTIVATEAPP)
        //    {
        //        if (m.WParam == IntPtr.Zero)
        //            OnApplicationDeactivated();
        //    }

        //    if (m.Msg == WM_MOUSEACTIVATE)
        //    {
        //        if (m.WParam == Handle)
        //        {
        //            SelectItemUnderMouseCursor();
        //            TrySimulateDoubleClick();

        //            m.Result = (IntPtr)MA_NOACTIVATEANDEAT;
        //            return;
        //        }
        //    }

        //    base.WndProc(ref m);
        //}

        //protected override void OnVisibleChanged(EventArgs e)
        //{
        //    base.OnVisibleChanged(e);

        //    if (Visible)
        //    {
        //        //Debug.Assert(messageFilter == null);
        //        //messageFilter = new MessageFilter(this, ownerControl);
        //        //Application.AddMessageFilter(messageFilter);
        //    }
        //    else
        //    {
        //        //Debug.Assert(messageFilter != null);
        //        //Application.RemoveMessageFilter(messageFilter);
        //        //messageFilter = null;
        //    }
        //}

        private void OnListBoxSelectionChanged()
        {
            CloseDocumentationPopup();

            if (documentationPopupDelayTimer == null)
            {
                const int DocumentationPopupDelay = 500;
                documentationPopupDelayTimer = new Timer { Interval = DocumentationPopupDelay };
                documentationPopupDelayTimer.Tick += (o, e) =>
                {
                    documentationPopupDelayTimer.Stop();
                    DisplayDocumentationPopup();
                };
            }

            documentationPopupDelayTimer.Start();
        }

        private void CloseDocumentationPopup()
        {
            if (documentationPopup != null && documentationPopup.Visible)
                documentationPopup.Close();
        }

        private void DisplayDocumentationPopup()
        {
            if (documentationPopup == null)
            {
                var configuration = new ParameterInfoToolTipConfiguration(listBox, toolTip, i => { });// ???
                documentationPopup = new ParameterInfoToolTip(configuration);
            }

            CloseDocumentationPopup();

            var selectedIndex = listBox.SelectedIndex;

            if (selectedIndex == -1)
                return;

            var documentation = ((Lazy<SymbolDocumentation>)((ListControlItem)listBox.SelectedItem).Value)?.Value;
            if (documentation == null)
                return;

            var data = new ParameterInfoSymbol(documentation.Definition, documentation.Summary, null, 0, 1, null);

            var position = new Point((int)listBox.Width, 0);
            documentationPopup.ShowPopup(data, position);
        }

        private void TrySimulateDoubleClick()
        {
            if (!doubleClickStopwatch.IsRunning)
                doubleClickStopwatch.Start();
            else
            {
                //if (doubleClickStopwatch.ElapsedMilliseconds <= SystemInformation.DoubleClickTime)
                //    SendKeys.Send("{ENTER}");

                doubleClickStopwatch.Restart();
            }
        }

        private void SelectItemUnderMouseCursor()
        {
            //var mousePosition = listBox.PointToClient(Cursor.Position);
            //var itemIndex = listBox.IndexFromPoint(mousePosition);
            //listBox.SelectedIndex = itemIndex;
        }

        private void OnApplicationDeactivated()
        {
            if (listBox.Visible)
                Close();
        }

        //private void SetSizeToContent()
        //{
        //    const int BorderSize = 2;
        //    const int RightPadding = 15;

        //    //var preferredSize = MainControl.GetItemsPreferredSize();
        //    //var pageHeightInPixels = listBox.ItemHeight * ItemsInPage;

        //    //int width = preferredSize.Width + RightPadding + BorderSize;
        //    int height;

        //    //if (preferredSize.Height > pageHeightInPixels)
        //    //{
        //    //    width += SystemInformation.VerticalScrollBarWidth;
        //    //    height = pageHeightInPixels;
        //    //}
        //    //else
        //    //height = preferredSize.Height;

        //    //height += BorderSize;

        //    Size = new Size();// width, height);
        //}

        private void ShiftSelection(int amount)
        {
            if (listBox.Items.Count == 0)
                return;

            if (listBox.SelectedIndex < 0)
            {
                listBox.SelectedIndex = 0;
                return;
            }

            //var index = MathUtilities.Clamp(
            //    listBox.SelectedIndex + amount,
            //    0,
            //    listBox.Items.Count - 1);

            //listBox.SelectedIndex = index;
        }

        public void Show(Control ownerControl, Point position)
        {
            //todo
            //this.ShowPopup(ownerControl);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Represents a ListBox item with text and image.
        /// </summary>
        //public class ListBoxWithImagesItem
        //{
        //    /// <summary>
        //    /// Initializes a new instance of the <c>ListBoxWithImagesItem</c> class with specified settings.
        //    /// </summary>
        //    /// <param name="text">Text of the new instance.</param>
        //    /// <param name="index">Image index of the new instance.</param>
        //    /// <param name="documentation"><c>SymbolDocumentation</c> related to this new instance.</param>
        //    public ListBoxWithImagesItem(string text, int index, Lazy<SymbolDocumentation> documentation)
        //    {
        //        Text = text;
        //        ImageIndex = index;
        //        Documentation = documentation;
        //    }

        //    /// <summary>
        //    /// Gets or sets item text.
        //    /// </summary>
        //    public string Text { get; set; }

        //    /// <summary>
        //    /// Gets or sets the index value of the image displayed on this item.
        //    /// </summary>
        //    public int ImageIndex { get; set; }

        //    /// <summary>
        //    /// Gets or sets <c>SymbolDocumentation</c> related to this item.
        //    /// </summary>
        //    public Lazy<SymbolDocumentation> Documentation { get; set; }

        //    /// <summary>
        //    /// Returns a <c>String</c> that represents the item.
        //    /// </summary>
        //    /// <returns>A <c>String</c> that represents the <c>ListBoxWithImagesItem</c>.</returns>
        //    public override string ToString()
        //    {
        //        return Text;
        //    }
        //}

        /// <summary>
        /// Represents a Windows control to display a list of items with text and image.
        /// </summary>
        //[ToolboxItem(false)]
        //public class ListBoxWithImages : ListBox
        //{
        //    private const int ImageLeftPadding = 2;

        //    private const int ImageToTextPadding = 2;

        //    private static Pen firstNonSelectedItemPen = new Pen(SystemBrushes.Highlight, 1);

        //    /// <summary>
        //    /// Initializes a new instance of the <c>ListBoxWithImages</c> class with default settings.
        //    /// </summary>
        //    public ListBoxWithImages()
        //    {
        //        //DrawMode = DrawMode.OwnerDrawFixed;
        //    }

        //    /// <summary>
        //    /// Gets or sets the ImageList that contains the images to display in this <c>ListBoxWithImages</c>.
        //    /// </summary>
        //    //public ImageList ImageList { get; set; }

        //    /// <summary>
        //    /// Gets or sets the AlphaImageList that contains the images to display in this <c>ListBoxWithImages</c>.
        //    /// </summary>
        //    public AlphaImageList AlphaImageList { get; set; }

        //    /// <summary>
        //    /// Calculates dimension of items area.
        //    /// </summary>
        //    /// <returns>Height and width of the items area.</returns>
        //    public Size GetItemsPreferredSize()
        //    {
        //        int width = int.MinValue;
        //        var font = Font;
        //        //foreach (ListBoxWithImagesItem item in Items)
        //        //{
        //        //    var size = TextRenderer.MeasureText(item.Text, font);
        //        //    width = Math.Max(width, size.Width);
        //        //}

        //        //if (AlphaImageList != null)
        //        //{
        //        //    return new Size(
        //        //        width + AlphaImageList.ImageSize.Width + ImageLeftPadding + ImageToTextPadding,
        //        //        Items.Count * ItemHeight);
        //        //}

        //        //if (ImageList != null)
        //        //{
        //        //    return new Size(
        //        //        width + ImageList.ImageSize.Width + ImageLeftPadding + ImageToTextPadding,
        //        //        Items.Count * ItemHeight);
        //        //}

        //        return new Size();
        //        //return new Size(width, Items.Count * ItemHeight);
        //    }

        //    //protected override void OnDrawItem(DrawItemEventArgs e)
        //    //{
        //    //    e.DrawBackground();
        //    //    e.DrawFocusRectangle();

        //    //    var bounds = e.Bounds;

        //    //    var item = (ListBoxWithImagesItem)Items[e.Index];
        //    //    var imageSize = ImageList.ImageSize;

        //    //    if (AlphaImageList != null)
        //    //    {
        //    //        AlphaImageList.Draw(
        //    //        e.Graphics,
        //    //        bounds.Left + ImageLeftPadding,
        //    //        bounds.Top + ((bounds.Size.Height - imageSize.Height) / 2),
        //    //        item.ImageIndex);
        //    //    }
        //    //    else
        //    //    {
        //    //        ImageList.Draw(
        //    //        e.Graphics,
        //    //        bounds.Left + ImageLeftPadding,
        //    //        bounds.Top + ((bounds.Size.Height - imageSize.Height) / 2),
        //    //        item.ImageIndex);
        //    //    }

        //    //    var textOrigin = new Point(
        //    //        bounds.Left + imageSize.Width + ImageLeftPadding + ImageToTextPadding,
        //    //        bounds.Top + ((bounds.Size.Height - e.Font.Height) / 2));

        //    //    TextRenderer.DrawText(
        //    //        e.Graphics,
        //    //        item.Text,
        //    //        e.Font,
        //    //        textOrigin,
        //    //        e.ForeColor);

        //    //    if (SelectedIndex == -1 && e.Index == 0)
        //    //    {
        //    //        e.Graphics.DrawRectangle(
        //    //            firstNonSelectedItemPen,
        //    //            new Rectangle(textOrigin.X, bounds.Top, bounds.Right - textOrigin.X - 1, bounds.Height - 1));
        //    //    }
        //    //}
        //}

        //private class MessageFilter : IMessageFilter
        //{
        //    private const int WM_LBUTTONDOWN = 0x201;
        //    private const int WM_RBUTTONDOWN = 0x204;
        //    private const int WM_MBUTTONDOWN = 0x207;
        //    private const int WM_NCLBUTTONDOWN = 0x0A1;
        //    private const int WM_NCRBUTTONDOWN = 0x0A4;
        //    private const int WM_NCMBUTTONDOWN = 0x0A7;

        //    private CodeCompletionPopup popup;

        //    private Control ownerControl;

        //    public MessageFilter(CodeCompletionPopup popup, Control textbox)
        //    {
        //        this.popup = popup;
        //        ownerControl = textbox;
        //    }

        //    //public bool PreFilterMessage(ref Message m)
        //    //{
        //    //    if (popup != null)
        //    //    {
        //    //        switch (m.Msg)
        //    //        {
        //    //            case WM_LBUTTONDOWN:
        //    //            case WM_RBUTTONDOWN:
        //    //            case WM_MBUTTONDOWN:
        //    //            case WM_NCLBUTTONDOWN:
        //    //            case WM_NCRBUTTONDOWN:
        //    //            case WM_NCMBUTTONDOWN:
        //    //                OnMouseDown();
        //    //                break;
        //    //        }
        //    //    }

        //    //    return false;
        //    //}

        //    //private void OnMouseDown()
        //    //{
        //    //    Point cursorPos = Cursor.Position;

        //    //    if (!popup.Bounds.Contains(cursorPos))
        //    //    {
        //    //        if (ownerControl?.Parent != null && !ownerControl.Bounds.Contains(ownerControl.Parent.PointToClient(cursorPos)))
        //    //            popup.Close();
        //    //    }
        //    //}
        //}
    }
}