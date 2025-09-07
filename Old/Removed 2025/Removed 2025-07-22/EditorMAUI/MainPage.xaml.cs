using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Maui;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Alternet.Maui;

namespace EditorMAUI;

public partial class MainPage : Alternet.UI.DisposableContentPage
{
    public bool LogToWindowTitle = false;

    internal string NewFileNameNoExt = "embres:EditorMAUI.Content.newfile";

    static MainPage()
    {
        Alternet.UI.DebugUtils.RegisterExceptionsLogger((e) =>
        {
            /*
            if (global::System.Diagnostics.Debugger.IsAttached)
                global::System.Diagnostics.Debugger.Break();
            */

            Alternet.UI.BaseObject.Nop();
        });

        if (Alternet.UI.App.IsWindowsOS && Alternet.UI.DebugUtils.IsDebugDefined)
        {
            Alternet.UI.App.LogFileIsEnabled = true;
        }
    }

    public MainPage()
    {
        Alternet.UI.PlessMouse.ShowTestMouseInControl = false;

        InitializeComponent();

        var innerForm = CreateInnerForm();

        var showDialog = toolbar.AddDialogButton(
            null,
            "Test Dialog",
            Alternet.UI.KnownSvgImages.ImgDiamondFilled,
            () =>
            {
                innerForm.SetAlignedPosition(editorPanel, Alternet.UI.HVAlignment.TopRight);
                innerForm.Owner = this;
                innerForm.IsVisible = true;
                innerForm.Entry.Focus();
            });

        showDialogAbsPosition.Clicked += (s, e) =>
        {
            innerForm.SetAbsolutePosition(editorPanel, 150, 100);
            innerForm.IsVisible = true;
            innerForm.Owner = this;
            innerForm.Entry.Focus();
        };

        CollectionLogView.IsDebugWriteLineCalled = true;

        var logListBox = new SyntaxEditLogView();

        logListBox.BindApplicationLog();
        rootGrid.Add(logListBox, 0, 2);

        InitEdit();

        EditorView.Interior.HasBorder = false;

        if (!Alternet.UI.App.IsDesktopDevice)
        {
            Editor.RunAfterGotFocus = Alternet.UI.GenericControlAction.ShowKeyboardIfUnknown;

            Editor.Selection.Options |= SelectionOptions.DisableSelectionByMouse;
            Alternet.UI.BaseObject.UseNamesInToString = false;
        }
        else
        {
            panel.Padding = new(0);
        }

        BindingContext = this;

        var ho = editorPanel.HorizontalOptions;
        ho.Expands = true;
        ho.Alignment = LayoutAlignment.Fill;
        editorPanel.HorizontalOptions = ho;

        var vo = editorPanel.VerticalOptions;
        vo.Expands = true;
        vo.Alignment = LayoutAlignment.Fill;
        editorPanel.VerticalOptions = vo;

        Alternet.UI.Control.FocusedControlChanged += Control_FocusedControlChanged;

        Alternet.UI.PlessMouse.LastMousePositionChanged += PlessMouse_LastMousePositionChanged;

        EditorView.Interior.CornerClick += Interior_CornerClick;
        Editor.LongTap += Editor_LongTap;

        Editor.CanLongTap = true;

        setHeight.Clicked += SetHeight_Clicked;

        if (Alternet.UI.App.IsMacOS)
        {
            Editor.Font = Editor.RealFont.IncSize(2);
        }

        Editor.SizeChanged += (s, e) =>
        {
        };

        LoadFile(NewFileNameNoExt, "cs");

        toolbar.IsBottomBorderVisible = true;

        Alternet.UI.ConsoleUtils.BindConsoleOutput();
        Alternet.UI.ConsoleUtils.BindConsoleError();

        clearLogItem.Clicked += (s, e) =>
        {
            logListBox.Clear();
        };

        enterStrToStdInput.Clicked += (s, e) =>
        {
        };
    }

    public SimpleInputDialog CreateInnerForm()
    {
        var innerForm = SimpleInputDialog.CreateGoToLineDialog();

        innerForm.OkButtonClicked += (s, e) =>
        {
            Alternet.UI.App.Log("Ok button clicked");
        };

        innerForm.CancelButtonClicked += (s, e) =>
        {
            Alternet.UI.App.Log("Cancel button clicked");
        };

        return innerForm;
    }

    private void SetHeight_Clicked(object? sender, EventArgs e)
    {
        if (Window is not null)
        {
            Window.Height = (Window.Width / 3) * 2;
        }
    }

    private void Editor_LongTap(object? sender, Alternet.UI.LongTapEventArgs e)
    {
        var caret = Editor.CaretInfo;
        if (caret is not null)
        {
            caret.OverlayColor = Alternet.Drawing.LightDarkColors.Blue;
            caret.TopAndBottomOverlayVisible = true;
            Editor.InvalidateCaret();
        }
    }

    private void Interior_CornerClick(object? sender, EventArgs e)
    {
        Alternet.UI.Keyboard.ToggleKeyboardVisibility(Editor);
    }

    private void PlessMouse_LastMousePositionChanged(object? sender, EventArgs e)
    {
        /*
        if(Window is not null)
            Window.Title = Alternet.UI.Mouse.GetPosition(editor.Control).ToString();
        */
    }

    private void Control_FocusedControlChanged(object? sender, EventArgs e)
    {
        Alternet.UI.App.LogIf($"FocusedControlChanged: {sender?.GetType()}", false);
    }

    public SyntaxEditView EditorView => editorPanel.EditorView;

    public SyntaxEdit Editor => editorPanel.EditorView.Editor;

    private void InitEdit()
    {
        var ed = EditorView.Editor;
        ed.Outlining.AllowOutlining = true;
        ed.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        ed.IndentOptions = ed.IndentOptions;
        ed.NavigateOptions = ed.NavigateOptions;
        ed.Selection.Options = ed.Selection.Options | SelectionOptions.SelectBeyondEol;
        ed.ContextMenuStrip = ed.DefaultMenu;
        ed.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
    }

    public void LoadFile(string urlPrefix, string extension)
    {
        Editor.Text = string.Empty;
        Editor.Lexer = null;
        Editor.Source.BookMarks.Clear();
        Editor.Source.LineStyles.Clear();
        Editor.Source.OptimizedForMemory = false;

        var url = urlPrefix + "."+ extension;

        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

        if (stream is null || !Editor.LoadStream(stream))
        {
            Editor.Text = $"Error loading text: {url}";
            return;
        }

        switch (extension)
        {
            case "cs":
                break;
            case "py":
                break;
            default:
                throw new Exception("Unknown script type");
        }

        Editor.Source.HighlightReferences = true;

    }

    internal void InsertText(string s)
    {
        var ed = Editor;
        var position = ed.Position;
        ed.Lines.Insert(0, s);
        ed.Position = position;
    }

    void AppendText(string s)
    {
        var ed = Editor;
        var position = ed.Position;
        ed.Lines.Add(s);
        ed.Position = position;
    }

    void LogToEditor(Action action)
    {
        var ed = Editor;
        ed.Lexer = null;
        ed.BeginUpdate();
        try
        {
            ed.Lines.Clear();
            Alternet.UI.LogUtils.LogActionToAction(action, AppendText);
        }
        finally
        {
            ed.EndUpdate();
        }
    }

    private void Button1_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.LogUtils.RedirectLogFromFileToScreen = true;

        LogToEditor(Fn);

        void Fn()
        {
            Alternet.UI.Display.Log();
            Alternet.UI.LogUtils.LogFontsInformation();
        }
    }

    private void Button3_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.Keyboard.HideKeyboard(Editor);
    }

    private void Button2_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.Keyboard.ShowKeyboard(Editor);
    }

    private void editor_SelectionChanged(object sender, EventArgs e)
    {
    }
}