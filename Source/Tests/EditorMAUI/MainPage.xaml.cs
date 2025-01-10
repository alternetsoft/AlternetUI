using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Alternet.Editor;
using Alternet.Drawing;

namespace EditorMAUI;

public partial class MainPage : Alternet.UI.DisposableContentPage
{
    public bool LogToWindowTitle = false;

    private static int counter = 0;

    internal string NewFileNameNoExt = "embres:EditorMAUI.Content.newfile";

    private readonly Alternet.Syntax.Parsers.Advanced.CsParser? parserCs;
    private readonly Alternet.Syntax.Parsers.Roslyn.CsParser? roslynParser;

    private readonly Button button = new();

    static MainPage()
    {
    }

    public MainPage()
    {
        Alternet.UI.DebugUtils.RegisterExceptionsLogger();

        Alternet.UI.PlessMouse.ShowTestMouseInControl = false;

        InitializeComponent();

        InitEdit();

        if (Alternet.UI.App.IsWindowsOS)
        {
            roslynParser = new();
            editor.Editor.Lexer = roslynParser;
        }
        else
        {
            parserCs = new();
            editor.Editor.Lexer = parserCs;
        }

        LoadFile(NewFileNameNoExt + ".cs");

        button.Text = "Hello";

        editor.Interior.HasBorder = false;

        if (!Alternet.UI.App.IsDesktopDevice)
        {
            editor.Editor.RunAfterGotFocus = Alternet.UI.GenericControlAction.ShowKeyboardIfUnknown;

            editor.Editor.Selection.Options |= SelectionOptions.DisableSelectionByMouse;
            Alternet.UI.BaseObject.UseNamesInToString = false;
        }
        else
        {
            panel.Padding = new(0);
        }

        BindingContext = this;

        Alternet.UI.App.LogMessage += App_LogMessage;

        var ho = editor.HorizontalOptions;
        ho.Expands = true;
        ho.Alignment = LayoutAlignment.Fill;
        editor.HorizontalOptions = ho;

        var vo = editor.VerticalOptions;
        vo.Expands = true;
        vo.Alignment = LayoutAlignment.Fill;
        editor.VerticalOptions = vo;

        button1.Clicked += Button1_Clicked;

        Alternet.UI.Control.FocusedControlChanged += Control_FocusedControlChanged;

        Alternet.UI.PlessMouse.LastMousePositionChanged += PlessMouse_LastMousePositionChanged;

        editor.Interior.CornerClick += Interior_CornerClick;
        editor.Interior.Scroll += Interior_Scroll;
        editor.Editor.LongTap += Editor_LongTap;

        editor.Editor.CanLongTap = true;

        extraControls.IsVisible = false;

        setHeight.Clicked += SetHeight_Clicked;

        if(Alternet.UI.App.IsMacOS)
        {
            editor.Editor.Font = editor.Editor.RealFont.IncSize(2);    
        }

        editor.Editor.SizeChanged+=(s,e)=>
        {
        };
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
        LogToEntry("LongTap", e, true);
        var caret = editor.Editor.CaretInfo;
        if(caret is not null)
        {
            caret.OverlayColor = LightDarkColors.Blue;
            caret.TopAndBottomOverlayVisible = true;
            editor.Editor.InvalidateCaret();
        }
    }

    private void Interior_Scroll(object? sender, Alternet.UI.ScrollEventArgs e)
    {
        LogToEntry("Scroll", e, false);
    }

    private void LogToEntry(string prefix, object obj, bool condition)
    {
        Alternet.UI.DebugUtils.DebugCallIf(condition, () =>
        {
            entry1.Text = $"({Alternet.UI.LogUtils.GenNewId()}){prefix}: {obj}";
        });
    }

    private void Interior_CornerClick(object? sender, EventArgs e)
    {
        Alternet.UI.Keyboard.ToggleKeyboardVisibility(editor.Editor);

        /*
        editor.Editor.Font = editor.Editor.Font.Larger();
        */
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

    private void InitEdit()
    {
        var ed = editor.Editor;
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

    public void LoadFile(string url)
    {
        var ed = editor.Editor;
        ed.Text = string.Empty;
        ed.Source.BookMarks.Clear();
        ed.Source.LineStyles.Clear();

        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

        if (stream is null || !ed.LoadStream(stream))
        {
            ed.Text = $"Error loading text: {url}";
            return;
        }
    }

    internal void InsertText(string s)
    {
        var ed = editor.Editor;
        var position = ed.Position;
        ed.Lines.Insert(0, s);
        ed.Position = position;
    }

    void AppendText(string s)
    {
        var ed = editor.Editor;
        var position = ed.Position;
        ed.Lines.Add(s);
        ed.Position = position;
    }

    void LogToEditor(Action action)
    {
        var ed = editor.Editor;
        ed.Lexer = null;
        ed.BeginUpdate();
        try
        {
            ed.Lines.Clear();
            AppendText(Alternet.UI.LogUtils.GetLogVersionText());
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

        /*
        var page = new Alternet.MAUI.SelectDevToolsActionPage();
        await Navigation.PushModalAsync(page);
        */
    }

    private void Button3_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.Keyboard.HideKeyboard(editor.Editor);
    }

    private void Button2_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.Keyboard.ShowKeyboard(editor.Editor);
    }

    public ObservableCollection<SimpleItem> MyItems { get; set; } = [];

    private void LogToTitle(string? s)
    {
        if (s is null)
            return;

        if (Window is not null)
            Window.Title = $"{counter++} {s}";
    }

    private void App_LogMessage(object? sender, Alternet.UI.LogMessageEventArgs e)
    {
        try
        {
            if (!LogToWindowTitle)
                return;

            LogToTitle(e.Message);
        }
        catch
        {
        }
    }

    public class SimpleItem
    {
        public string Text { get; set; } = "";
    }

    private void editor_SelectionChanged(object sender, EventArgs e)
    {
    }
}