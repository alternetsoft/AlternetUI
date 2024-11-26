using System.Collections.ObjectModel;
using System.Diagnostics;

using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;

using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;

namespace RoslynSyntaxParsing;

public partial class MainPage : ContentPage
{
    private readonly Alternet.Editor.TextSource.TextSource csharpSource = new();
    private readonly Alternet.Editor.TextSource.TextSource vbSource = new();
    private readonly CsParser csParser1 = new();
    private readonly VbParser vbParser1 = new();

    internal string NewFileNameNoExt = "embres:RoslynSyntaxParsing.Content.newfile";

    static MainPage()
    {
    }

    public MainPage()
    {
        InitializeComponent();

        if (!Alternet.UI.App.IsDesktopDevice)
        {
            var imageSource = Alternet.UI.MauiUtils.ImageSourceFromSvg(
                Alternet.UI.KnownSvgImages.ImgKeyboard,
                32,
                Alternet.UI.MauiUtils.IsDarkTheme ?? false);
            keyboardButton.Source = imageSource;
        }
        else
        {
            keyboardButton.IsVisible = false;
        }

        InitEdit();

        csharpSource.Lexer = csParser1;
        LoadFile(csharpSource, NewFileNameNoExt + ".cs");
        csharpSource.HighlightReferences = true;

        vbSource.Lexer = vbParser1;
        LoadFile(vbSource, NewFileNameNoExt + ".vb");
        vbSource.HighlightReferences = true;

        syntaxEdit1.Editor.Source = csharpSource;

        BindingContext = this;

        LanguagesPicker.SelectedIndex = 0;

        if (!Alternet.UI.App.IsDesktopOs)
        {
            laDescription.IsVisible = false;
        }

        if (!Alternet.UI.App.IsDesktopDevice)
        {
            syntaxEdit1.Interior.HasBorder = false;
            syntaxEdit1.Margin = new(0);
        }
        else
        {
            syntaxEdit1.Interior.HasBorder = true;
            syntaxEdit1.Margin = new(10);
        }

        if (!Alternet.UI.App.IsWindowsOS)
        {
            // Currently loadButton only supports loading on Windows
            // For other platforms, see 
            // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0&tabs=macios
            loadButton.IsVisible = false;
        }
    }

    private bool IsVisualBasicSelected => syntaxEdit1.Source == vbSource;

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Source = LanguagesPicker.SelectedIndex switch
        {
            0 => csharpSource,
            1 => vbSource,
            _ => csharpSource,
        };
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit1.Selection.Options = syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol;
        syntaxEdit1.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
    }

    public void LoadFile(Alternet.Editor.TextSource.TextSource source, string url)
    {
        source.Text = string.Empty;
        source.BookMarks.Clear();
        source.LineStyles.Clear();

        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

        if (stream is null || !source.LoadStream(stream))
        {
            source.Text = $"Error loading text: {url}";
            return;
        }
    }

    private void KeyboardButton_Clicked(object? sender, EventArgs e)
    {
        syntaxEdit1.ToggleKeyboard();
    }

    private async void LoadButton_Clicked(object? sender, EventArgs e)
    {
        var customFileTypeCs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".cs" } },
                });

        var customFileTypeVb = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".vb" } },
                });

        PickOptions options = new PickOptions()
        {
            PickerTitle = IsVisualBasicSelected
                ? "Please select a C# file" : "Please select a Visual Basic file",
            FileTypes = IsVisualBasicSelected ? customFileTypeVb : customFileTypeCs,
        };

        var files = await FilePicker.Default.PickAsync(options);

        if (files == null)
            return;

        var source = IsVisualBasicSelected ? vbSource : csharpSource;

        LoadFile(source, files.FullPath);
    }
}