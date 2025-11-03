using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window for searching and displaying class members.
    /// Inherits from <see cref="Window"/>.
    /// </summary>
    public class WindowSearchForMembers : Window
    {
        private static WindowSearchForMembers? defaultWindow;
        private static int globalCounter;

        private readonly int counter;
        private readonly AbstractControl? statusPanel;

        private readonly TextBoxAndButton textBox = new()
        {
        };

        private readonly VirtualListBox listBox = new()
        {
        };

        private readonly ToolBar statusBar = new()
        {
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        private EnumImages<SymbolKind>? images;
        private VirtualListBox.RangeAdditionController<MemberInfo>? controller;
        private bool closeRequested;
        private Graphics.DrawElementParams runImageElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSearchForMembers"/> class.
        /// </summary>
        public WindowSearchForMembers()
        {
            runImageElement = Graphics.DrawElementParams.CreateImageElement(
                    this,
                    KnownSvgImages.ImgDebugRun,
                    null,
                    LightDarkColors.Green);

            textBox.InitSearchEdit();
            textBox.TextBox.EmptyTextHint = "Type here to search for classes and members...";

            counter = ++globalCounter;

            Size = (800, 600);
            Title = "Search for classes and members";
            StartLocation = WindowStartLocation.ScreenTopRight;
            Layout = LayoutStyle.Vertical;

            textBox.Margin = 10;
            textBox.Parent = this;

            listBox.Margin = 10;
            listBox.VerticalAlignment = VerticalAlignment.Fill;
            listBox.Parent = this;
            listBox.SelectionUnderImage = false;

            textBox.DelayedTextChanged += ComboBox_TextChanged;

            statusBar.Parent = this;

            statusBar.SetBorderAndMargin(AnchorStyles.Top);

            statusBar.MinHeight = 24;
            statusPanel = new Label("Ready");
            statusBar.AddControl(statusPanel);

            LoadImages(this.IsDarkBackground);

            ActiveControl = textBox;
        }

        private enum SymbolKind
        {
            Other,
            Field,
            Property,
            Method,
            Event,
        }

        /// <summary>
        /// Gets default instance of <see cref="WindowSearchForMembers"/>.
        /// </summary>
        public static new WindowSearchForMembers Default
        {
            get
            {
                if(defaultWindow is null)
                {
                    defaultWindow ??= new();

                    defaultWindow.Disposed += (s, e) =>
                    {
                        defaultWindow = null;
                    };
                }

                return defaultWindow;
            }
        }

        /// <summary>
        /// Loads and initializes the images used for code completion symbols.
        /// </summary>
        /// <remarks>This method initializes the image collection for various symbol kinds (e.g., fields,
        /// events,  methods, and properties) by assigning appropriate image names and colors.
        /// It also loads the  images
        /// from embedded resources in the assembly. If the images have already been initialized,
        /// the method returns
        /// without performing any actions.</remarks>
        /// <param name="isDark">A value indicating whether the dark theme is enabled.
        /// If <see langword="true"/>, the images will be adjusted
        /// for the dark theme; otherwise, the default theme is used.</param>
        public virtual void LoadImages(bool isDark)
        {
            images = new();

            string prefix = "Resources.Svg.CodeCompletionSymbols.";

            images.SetImageName(SymbolKind.Field, $"{prefix}Field.svg");
            images.SetImageName(SymbolKind.Event, $"{prefix}Event.svg");
            images.SetImageName(SymbolKind.Method, $"{prefix}Method1.svg");
            images.SetImageName(SymbolKind.Property, $"{prefix}Property.svg");

            images.SetSvgColor(SymbolKind.Field, LightDarkColors.Green);
            images.SetSvgColor(SymbolKind.Event, LightDarkColors.Yellow);
            images.SetSvgColor(SymbolKind.Method, LightDarkColors.Blue);
            images.SetSvgColor(SymbolKind.Property, DefaultColors.SvgNormalColor);

            images.AssignImageNames(true);

            images.LoadSvgFromResource(typeof(WindowSearchForMembers).Assembly);
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            images?.ResetCachedImages();
        }

        /// <summary>
        /// Retrieves an image representation for the specified member type.
        /// </summary>
        /// <remarks>If the member type corresponds to an unknown or unsupported kind, a default
        /// placeholder image  is returned. The size of the placeholder image depends
        /// on whether a scale factor is
        /// applied.</remarks>
        /// <param name="memberType">The type of the member for which to retrieve the image.</param>
        /// <returns>An <see cref="Image"/> object representing the specified
        /// member type, or <see langword="null"/>  if no image
        /// is available.</returns>
        protected virtual SvgImage? GetImage(MemberTypes memberType)
        {
            var kind = GetKind(memberType);
            if (kind == SymbolKind.Other)
                return KnownSvgImages.ImgEmpty;
            var result = images?.GetSvgImage(kind);
            return result;
        }

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            StopThread();
            base.OnClosed(e);
            App.LogIf($"{GetType()}{counter} Closed", false);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            StopThread();
            base.DisposeManaged();
            App.LogIf($"{GetType()}{counter} Disposed", false);
        }

        /// <inheritdoc/>
        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);

            if(controller is not null)
            {
                e.Cancel = true;

                closeRequested = true;

                StopThread();
            }

            App.LogIf($"{GetType()}{counter} Closing", false);
        }

        private static SymbolKind GetKind(MemberTypes memberType)
        {
            if (memberType.HasFlag(MemberTypes.Field))
                return SymbolKind.Field;
            if (memberType.HasFlag(MemberTypes.Event))
                return SymbolKind.Event;
            if (memberType.HasFlag(MemberTypes.Property))
                return SymbolKind.Property;
            if (memberType.HasFlag(MemberTypes.Method))
                return SymbolKind.Method;
            return SymbolKind.Other;
        }

        private void StopThread()
        {
            controller?.Stop();
            controller = null;
        }

        private void ResetListBox(Action? afterThreadStopped)
        {
            if (controller is not null)
            {
                if (!controller.IsStopped)
                {
                    controller.ThreadActionFinished = MyAction;
                    StopThread();
                    return;
                }
                else
                {
                    MyAction();
                }
            }

            MyAction();

            void MyAction()
            {
                listBox.RemoveAll();
                statusPanel?.SetText(0);
                afterThreadStopped?.Invoke();
            }
        }

        private void ComboBox_TextChanged(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            App.DebugLogIf("ComboBox_TextChanged", false);
            StartThread();
        }

        private void StartThread()
        {
            ResetListBox(Internal);

            void Internal()
            {
                controller = CreateController();
                controller?.Start(() =>
                {
                    if (DisposingOrDisposed)
                        return;
                    if (closeRequested)
                    {
                        closeRequested = false;
                        Close(WindowCloseAction.Dispose);
                    }
                });
            }
        }

        private VirtualListBox.RangeAdditionController<MemberInfo>? CreateController()
        {
            var containsText = textBox.Text;

            if (string.IsNullOrEmpty(containsText))
            {
                ResetListBox(null);
                return null;
            }

            ListControlItem? ConvertItem(MemberInfo member)
            {
                if (member.Name.EndsWith("_"))
                    return null;

                var fullName = $"{member.DeclaringType?.Name}.{member.Name}";

                var replaced = StringUtils.InsertBoldTags(fullName, containsText);

                Action? doubleClickAction = null;

                if (member is MethodInfo method)
                {
                    var retParam = method.ReturnParameter;
                    var resultIsVoid = retParam.ParameterType == typeof(void);
                    var methodParameters = method.GetParameters();

                    if (methodParameters.Length == 0 && method.IsStatic)
                    {
                        doubleClickAction = () =>
                        {
                            AssemblyUtils.InvokeMethodAndLogResult(null, method);
                        };
                    }
                }

                var item = new MemberInfoItem();
                item.Text = replaced;
                item.LabelFlags = DrawLabelFlags.TextHasBold;
                item.SvgImageSize = SvgUtils.GetSvgSize(ScaleFactor);
                item.SvgImage = GetImage(member.MemberType);
                item.MemberInfo = member;
                item.DoubleClickAction = doubleClickAction;

                if(doubleClickAction != null)
                {
                    item.SuffixElements = [runImageElement];
                }

                return item;
            }

            var controller = new VirtualListBox.RangeAdditionController<MemberInfo>(
                listBox,
                () => AssemblyUtils.GetAllPublicMembers(containsText),
                ConvertItem,
                () =>
                {
                    if (IsDisposed || containsText == string.Empty)
                        return false;
                    Invoke(() =>
                    {
                        statusPanel?.SetText(listBox.Items.Count);
                        statusPanel?.Refresh();
                    });
                    return true;
                });

            return controller;
        }

        private class MemberInfoItem : TreeViewItem
        {
            public MemberInfoItem()
            {
            }

            public MemberInfo? MemberInfo { get; set; }
        }
    }
}
