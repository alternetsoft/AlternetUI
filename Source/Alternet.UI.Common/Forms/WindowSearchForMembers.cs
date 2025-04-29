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
        private string? lastReportedText;
        private VirtualListBox.AddRangeController<MemberInfo>? controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSearchForMembers"/> class.
        /// </summary>
        public WindowSearchForMembers()
        {
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

            textBox.TextChanged += ComboBox_TextChanged;

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

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            App.LogIf($"{GetType()}{counter} Closed", false);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            App.LogIf($"{GetType()}{counter} Disposed", false);
        }

        /// <inheritdoc/>
        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
            App.LogIf($"{GetType()}{counter} Closing", false);
            SafeDisposeObject(ref controller);
        }

        /// <inheritdoc/>
        protected override void OnIdle(EventArgs e)
        {
            if (lastReportedText == textBox.Text)
                return;
            lastReportedText = textBox.Text;

            ResetListBox();
            if (lastReportedText.Length > 0)
                StartThread();
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

        private void ResetListBox()
        {
            SafeDisposeObject(ref controller);
            listBox.RemoveAll();
            statusPanel?.SetText(0);
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            ResetListBox();
        }

        private void StartThread()
        {
            ResetListBox();
            controller = CreateController();
            controller.Start();
        }

        private Image? GetImage(MemberTypes memberType)
        {
            var kind = GetKind(memberType);
            if (kind == SymbolKind.Other)
                return KnownSvgImages.ImgEmpty.AsImage(HasScaleFactor ? 32 : 16);
            var result = images?.GetImage(kind, !HasScaleFactor);
            return result;
        }

        private void LoadImages(bool isDark)
        {
            if (images != null)
                return;
            images = new();

            string prefix = "Resources.Svg.CodeCompletionSymbols.";

            images.SetImageName(SymbolKind.Field, $"{prefix}Field.svg");
            images.SetImageName(SymbolKind.Event, $"{prefix}Event.svg");
            images.SetImageName(SymbolKind.Method, $"{prefix}Method1.svg");
            images.SetImageName(SymbolKind.Property, $"{prefix}Property.svg");

            images.SetSvgColor(SymbolKind.Field, LightDarkColors.Green.LightOrDark(isDark));
            images.SetSvgColor(SymbolKind.Event, LightDarkColors.Yellow.LightOrDark(isDark));
            images.SetSvgColor(SymbolKind.Method, LightDarkColors.Blue.LightOrDark(isDark));

            images.SetSvgColor(
                SymbolKind.Property,
                new LightDarkColor(KnownSvgColor.Normal).LightOrDark(isDark));

            images.AssignImageNames(true);

            images.LoadImagesFromResource(typeof(WindowSearchForMembers).Assembly, true);
            images.LoadImagesFromResource(typeof(WindowSearchForMembers).Assembly, false);
        }

        private VirtualListBox.AddRangeController<MemberInfo> CreateController()
        {
            var containsText = lastReportedText ?? string.Empty;

            ListControlItem? ConvertItem(MemberInfo member)
            {
                if (member.Name.EndsWith("_"))
                    return null;

                var fullName = $"{member.DeclaringType.Name}.{member.Name}";

                var replaced = StringUtils.InsertBoldTags(fullName, containsText);

                Action? doubleClickAction = null;

                if (member is MethodInfo method)
                {
                    var retParam = method.ReturnParameter;
                    var resultIsVoid = retParam.ParameterType == typeof(void);
                    var methodParameters = method.GetParameters();

                    if (methodParameters.Length == 0 && method.IsStatic)
                    {
                        replaced += " " + CharUtils.RightArrow;

                        doubleClickAction = () =>
                        {
                            AssemblyUtils.InvokeMethodAndLogResult(null, method);
                        };
                    }
                }

                var item = new MemberInfoItem();
                item.Text = replaced;
                item.LabelFlags = DrawLabelFlags.TextHasBold;
                item.Image = GetImage(member.MemberType);
                item.MemberInfo = member;
                item.DoubleClickAction = doubleClickAction;

                return item;
            }

            var controller = new VirtualListBox.AddRangeController<MemberInfo>(
                listBox,
                () => AssemblyUtils.GetAllPublicMembers(containsText),
                ConvertItem,
                () =>
                {
                    if (IsDisposed || containsText != lastReportedText)
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

        private class MemberInfoItem : TreeControlItem
        {
            public MemberInfoItem()
            {
            }

            public MemberInfo? MemberInfo { get; set; }
        }
    }
}
