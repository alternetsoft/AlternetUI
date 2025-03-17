using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

using Alternet.Editor;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Alternet.Maui;
using Alternet.Scripter.Python;

namespace EditorMAUI;

public partial class MainPage : Alternet.UI.DisposableContentPage, EditorUI.IDocumentContainer
{
    public bool LogToWindowTitle = false;

    internal string NewFileNameNoExt = "embres:EditorMAUI.Content.newfile";

    private readonly EditorUI.PythonDocument documentPy;
    private readonly EditorUI.CSharpDocument documentCs;
    private readonly ObservableCollection<LogItem> logItems = new();

    private readonly SimpleToolBarView.IToolBarItem buttonNew;
    private readonly SimpleToolBarView.IToolBarItem buttonRun;
    private readonly SimpleToolBarView.IToolBarItem buttonRunNoDebug;
    private readonly SimpleToolBarView.IToolBarItem buttonStepInto;
    private readonly SimpleToolBarView.IToolBarItem buttonStepOver;
    private readonly SimpleToolBarView.IToolBarItem buttonStepOut;
    private readonly SimpleToolBarView.IToolBarItem buttonStop;
    private readonly SimpleToolBarView.IToolBarItem buttonStatus;

    private ExecutionPosition? executionPosition;
    private EditorUI.CustomDocument documentCurrent;

    static MainPage()
    {
        SyntaxEditView.SyntaxEditTypeOverride = typeof(EditorUI.PoweredSyntaxEdit);

        Alternet.UI.DebugUtils.RegisterExceptionsLogger((e) =>
        {
            /*
            if (global::System.Diagnostics.Debugger.IsAttached)
                global::System.Diagnostics.Debugger.Break();
            */
        });

        ScripterDemoUtils.Initialize();
        PythonDemoUtils.Initialize();

        if (Alternet.UI.App.IsWindowsOS && Alternet.UI.DebugUtils.IsDebugDefined)
        {
            Alternet.UI.App.LogFileIsEnabled = true;
        }
    }

    public MainPage()
    {
        if (App.MainWindow is not null)
        {
            App.MainWindow.Destroying += (s, e) =>
            {
                ActiveDocument.Stop();
            };
        }

        Alternet.UI.PlessMouse.ShowTestMouseInControl = false;

        InitializeComponent();

        EditorUI.MainToolBar.ResPrefix = "embres:EditorUI.Dll.Resources.Svg.";

        var svgRestart = EditorUI.MainToolBar.CreateRestartSvgImage();
        var svgStepOver = EditorUI.MainToolBar.CreateStepOverSvgImage();
        var svgStop = EditorUI.MainToolBar.CreateStopSvgImage();
        var svgStepOut = EditorUI.MainToolBar.CreateStepOutSvgImage();
        var svgStartNoDebug = EditorUI.MainToolBar.CreateStartNoDebugSvgImage();
        var svgStart = EditorUI.MainToolBar.CreateStartSvgImage();
        var svgStepInto = EditorUI.MainToolBar.CreateStepIntoSvgImage();

        buttonNew = toolbar.AddDialogButton(null, "New", Alternet.UI.KnownSvgImages.ImgFileNew, () =>
        {
            if (IsPython)
            {
                LoadFile(NewFileNameNoExt, "cs");
            }
            else
            {
                LoadFile(NewFileNameNoExt, "py");
            }
        });

        buttonRun = toolbar.AddDialogButton(null, "Run", svgStart, () =>
        {
            ActiveDocument.Run();
        });

        buttonRunNoDebug = toolbar.AddDialogButton(null, "Run No Debug", svgStartNoDebug, () =>
        {
            ActiveDocument.RunWithoutDebug();
        });

        buttonStepInto = toolbar.AddDialogButton(null, "Step Into", svgStepInto, () =>
        {
            ActiveDocument.StepInto();
        });

        buttonStepOver = toolbar.AddDialogButton(null, "Step Over", svgStepOver, () =>
        {
            ActiveDocument.StepOver();
        });

        buttonStepOut = toolbar.AddDialogButton(null, "Step Out", svgStepOut, () =>
        {
            ActiveDocument.StepOver();
        });

        buttonStop = toolbar.AddDialogButton(null, "Stop", svgStop, () =>
        {
            ActiveDocument.Stop();
        });

        var innerForm = CreateInnerForm();

        var showDialog = toolbar.AddDialogButton(
            null,
            "Test Dialog",
            Alternet.UI.KnownSvgImages.ImgDiamondFilled,
            () =>
            {
                innerForm.IsVisible = !innerForm.IsVisible;
            });

        buttonStatus = toolbar.AddLabel("Ready");

        logListBox.SelectionMode = SelectionMode.Single;
        logListBox.ItemsSource = logItems;
        logListBox.ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepLastItemInView;
        logListBox.VerticalScrollBarVisibility = ScrollBarVisibility.Always;
        logListBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Always;

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

        Alternet.UI.App.LogMessage += App_LogMessage;

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

        ScripterDemoUtils.RunningProcessDisposed += ScripterUtils_RunningProcessDisposed;
        ScripterDemoUtils.RunningProcessExited += ScripterUtils_RunningProcessDisposed;

        documentCs = new(this);
        documentCs.StateChanged += Document_StateChanged;
        documentCurrent = documentCs;

        documentPy = new(this);
        documentPy.StateChanged += Document_StateChanged;

        LoadFile(NewFileNameNoExt, "cs");

        toolbar.IsBottomBorderVisible = true;

        Alternet.UI.ConsoleUtils.BindConsoleOutput();
        Alternet.UI.ConsoleUtils.BindConsoleError();

        UpdateToolBar(documentCs);

        clearLogItem.Clicked += (s, e) =>
        {
            logItems.Clear();
        };
    }

    public View CreateInnerForm()
    {
        var dialogTitle = new SimpleDialogTitleView();
        dialogTitle.Title = "Title";

        Color backColor;
        Color textColor;

        if (Alternet.UI.MauiUtils.IsDarkTheme)
        {
            backColor = Color.FromRgb(30, 30, 30);
            textColor = Color.FromRgb(220, 220, 220);
        }
        else
        {
            backColor = Colors.White;
            textColor = Colors.Black;
        }

        var borderColor = dialogTitle.GetPressedBorderColor();
        var placeHolderColor = textColor;

        var innerForm = new Border
        {
            IsVisible = false,
            BackgroundColor = backColor,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Stroke = borderColor,
            StrokeThickness = 1,
            MinimumWidthRequest = 300,
        };

        dialogTitle.CloseClicked += (s, e) =>
        {
            innerForm.IsVisible = false;
        };

        var formContent = new VerticalStackLayout();
        formContent.VerticalOptions = LayoutOptions.Start;

        formContent.Children.Add(dialogTitle);

        var verticalStack = new VerticalStackLayout();
        verticalStack.Padding = 10;

        var label = new Label
        {
            Text = "Enter your text:",
            Margin = new Thickness(5, 5, 5, 5),
            TextColor = textColor,
        };

        verticalStack.Children.Add(label);

        var entryBorder = new Border
        {
            Stroke = borderColor,
            StrokeThickness = 1,
            BackgroundColor = backColor,
            Margin = new Thickness(5),
            Padding = new Thickness(0),
            StrokeShape = new RoundRectangle { CornerRadius = 5 }
        };

        var entry = new Entry
        {
            Placeholder = "Type here",
            TextColor = textColor,
            PlaceholderColor = placeHolderColor,
        };

        entryBorder.Content = entry;
        verticalStack.Children.Add(entryBorder);

        var buttons = new SimpleToolBarView();
        buttons.Margin = new(0, 5, 0, 0);

        buttons.AddExpandingSpace();
        buttons.AddButtonOk(() =>
        {
            innerForm.IsVisible = false;
        });
        buttons.AddButtonCancel(() =>
        {
            innerForm.IsVisible = false;
        });

        verticalStack.Children.Add(buttons);

        formContent.Children.Add(verticalStack);

        innerForm.Content = formContent;

        formContent.SizeChanged += (s, e) =>
        {
            editorPanel.SetLayoutBounds(
                innerForm,
                new Rect(650, 100, -1, -1));
        };

        editorPanel.SetLayoutBounds(
            innerForm,
            new Rect(650, 100, -1, -1));

        editorPanel.Add(innerForm);

        return innerForm;
    }

    public EditorUI.IDocumentContainer Container => this;

    public EditorUI.CustomDocument ActiveDocument => documentCurrent;

    internal void UpdateToolBar(EditorUI.CustomDocument document)
    {
        /*
        SetToolEnabled(ButtonIdBreakAll, document.CanPause);
        SetToolEnabled(ButtonIdRestart, document.CanRestart);
        */

        buttonNew.IsEnabled = document.State == EditorUI.CustomDocument.RunningState.NotRunning;
        buttonRun.IsEnabled = document.CanRun;
        buttonRunNoDebug.IsEnabled = document.CanRunWithoutDebug;
        buttonStepInto.IsEnabled = document.CanStepInto;
        buttonStepOver.IsEnabled = document.CanStepOver;
        buttonStepOut.IsEnabled = document.CanStepOut;
        buttonStop.IsEnabled = document.CanStop;
    }

    private void Document_StateChanged(object? sender, EventArgs e)
    {
        if (sender != documentCurrent)
            return;

        Alternet.UI.App.Invoke(Fn);

        void Fn()
        {
            var notRunning = documentCurrent.State == EditorUI.CustomDocument.RunningState.NotRunning;

            Editor.ReadOnly = !notRunning;

            UpdateToolBar(documentCurrent);

            void UpdateStatus(string s)
            {
                Alternet.UI.App.Invoke(() =>
                {
                    buttonStatus.Text = s;
                });
            }

            switch (documentCurrent.State)
            {
                case EditorUI.CustomDocument.RunningState.NotRunning:
                    Container.ExecutionPosition = null;
                    Editor.SwitchStackFrame(null, null);
                    UpdateStatus("Execution stopped");
                    break;
                case EditorUI.CustomDocument.RunningState.RunWithDebug:
                    UpdateStatus("Run with debug");
                    break;
                case EditorUI.CustomDocument.RunningState.RunWithoutDebug:
                    UpdateStatus("Run without debug");
                    break;
                case EditorUI.CustomDocument.RunningState.Paused:
                    UpdateStatus("Debug paused");
                    break;
            }
        }
    }

    private void ScripterUtils_RunningProcessDisposed(object? sender, EventArgs e)
    {
        documentCurrent.State = EditorUI.CustomDocument.RunningState.NotRunning;
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

    void EditorUI.IDocumentContainer.ClearErrors()
    {
    }

    void EditorUI.IDocumentContainer.SelectErrorPanel()
    {
    }

    void EditorUI.IDocumentContainer.AddError(
        ScriptCompilationDiagnostic error)
    {
        Alternet.UI.App.Log($"{error}", EditorUI.ErrorListPanel.GetErrorKind(error));
    }

    string EditorUI.IDocumentContainer.DocumentText
    {
        get => Editor.Text;
    }

    string? EditorUI.IDocumentContainer.CommandLineArgs { get; set; }

    public EditorUI.PoweredSyntaxEdit Editor => (EditorUI.PoweredSyntaxEdit)(editorPanel.Editor);

    public SyntaxEditView EditorView => editorPanel.EditorView;

    public ExecutionPosition? ExecutionPosition
    {
        get => executionPosition;

        set
        {
            Editor.ClearDebugStyles(executionPosition);
            executionPosition = value;
            Editor.ExecutionStopped(value);
        }
    }

    private void InitEdit()
    {
        var ed = Editor;
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

    public bool IsPython
    {
        get
        {
            return documentCurrent == documentPy;
        }
    }

    public void LoadFile(string urlPrefix, string extension)
    {
        Container.ExecutionPosition = null;
        Editor.SwitchStackFrame(null, null);
        Editor.Text = string.Empty;
        Editor.Lexer = null;
        Editor.Source.BookMarks.Clear();
        Editor.Source.LineStyles.Clear();
        documentCs.ClearBreakPoints();
        documentPy.ClearBreakPoints();
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
                documentCs.InitEditor(Editor);
                documentCurrent = documentCs;
                UpdateToolBar(documentCurrent);
                break;
            case "py":
                documentPy.InitEditor(Editor);
                documentCurrent = documentPy;
                UpdateToolBar(documentCurrent);
                break;
            default:
                throw new Exception("Unknown script type");
        }

        Editor.Source.FileName = documentCurrent.FileName;
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

    private void App_LogMessage(object? sender, Alternet.UI.LogMessageEventArgs e)
    {
        try
        {
            if (e.Message is null)
                return;

            var s = e.Message;

            Debug.WriteLine(s);

            Alternet.UI.App.AddBackgroundInvokeAction(() =>
            {
                logItems.Add(new(s));
                logListBox.SelectedItem = logItems[logItems.Count - 1];
            });
        }
        catch
        {
        }
    }

    private void editor_SelectionChanged(object sender, EventArgs e)
    {
    }

    private class LogItem
    {
        public string Message = string.Empty;

        public LogItem(string message)
        {
            Message = message;
        } 

        public override string ToString()
        {
            return Message;
        }
    }
}