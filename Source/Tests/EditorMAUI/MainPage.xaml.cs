using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Alternet.Editor;

namespace EditorMAUI;

public partial class MainPage : ContentPage
{
    private static int counter = 0;

    internal string NewFileNameNoExt = "embres:EditorMAUI.Content.newfile";

    private readonly Alternet.Syntax.Parsers.Roslyn.CodeCompletion.CsSolution solution;
    private readonly Alternet.Syntax.Parsers.Roslyn.CsParser parserCs;

    private readonly Button button = new();

    static MainPage()
    {
    }

    public MainPage()
    {
        Alternet.UI.PlessMouse.ShowTestMouseInControl = true;

        InitializeComponent();

        solution = new(Microsoft.CodeAnalysis.SourceCodeKind.Regular);
        parserCs = new(solution);

        InitEdit();
        editor.Editor.Lexer = parserCs;

        LoadFile(NewFileNameNoExt + "-small.cs");

        editor.BackgroundColor = Colors.White;

        button.Text = "Hello";

        panel.BackgroundColor = Colors.White;
        panel.Padding = new(10);

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
        Alternet.UI.App.LogIf($"FocusedControlChanged: {sender?.GetType()}", true);
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

        Alternet.UI.DebugUtils.DebugCallIf(true, () =>
        {
            void InsertText(string s)
            {
                var position = ed.Position;
                ed.Lines.Insert(0, s);
                ed.Position = position;
            }

            InsertText("// " + Alternet.UI.LogUtils.GetLogVersionText() + Environment.NewLine);
        });
    }

    private async void Button1_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.Display.Log();

        var page = new Alternet.MAUI.SelectDevToolsActionPage();
        await Navigation.PushModalAsync(page);
    }

    public ObservableCollection<SimpleItem> MyItems { get; set; } = [];

    private void App_LogMessage(object? sender, Alternet.UI.LogMessageEventArgs e)
    {
        try
        {
            var s = e.Message;

            if (s is null)
                return;

            if (Window is not null)
                Window.Title = $"{counter++} {s}" ?? string.Empty;
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