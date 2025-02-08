using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    internal class ListBoxBigDataWindow : Window
    {
        internal bool IsDebugInfoLogged = false;

        private static int globalCounter;

        private readonly int counter;
        private readonly AbstractControl? statusPanel;

        private EnumImages<SymbolKind>? images;
        private string? lastReportedText;
        private VirtualListBox.AddRangeController<MemberInfo>? controller;

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

        public ListBoxBigDataWindow()
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

        private void LoadImages(bool isDark)
        {
            if (images != null)
                return;
            images = new();

            string prefix = "Resources.CodeComletionSymbols.";

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

            images.LoadImagesFromResource(typeof(ListBoxBigDataWindow).Assembly, true);
            images.LoadImagesFromResource(typeof(ListBoxBigDataWindow).Assembly, false);
        }

        public VirtualListBox.AddRangeController<MemberInfo> CreateController()
        {
            var containsText = lastReportedText ?? string.Empty;

            ListControlItem? ConvertItem(MemberInfo member)
            {
                var fullName = $"{member.DeclaringType.Name}.{member.Name}";

                var replaced = StringUtils.InsertBoldTags(fullName, containsText);

                var item = new ListControlItem(replaced);
                item.LabelFlags = DrawLabelFlags.TextHasBold;

                item.Image = GetImage(member.MemberType);

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            App.IdleLog($"{GetType()}{counter} Closed");
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            App.IdleLog($"{GetType()}{counter} Disposed");
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
            App.IdleLog($"{GetType()}{counter} Closing");
            SafeDisposeObject(ref controller);
        }

        void ResetListBox()
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

        protected override void OnIdle(EventArgs e)
        {
            if (lastReportedText == textBox.Text)
                return;
            lastReportedText = textBox.Text;

            var prefix = "Idle: ComboBox.TextChanged";
            App.LogReplace($"{prefix}: {textBox.Text}", prefix);
            ResetListBox();
            if (lastReportedText.Length > 0)
                StartThread();
        }

        public Image? GetImage(MemberTypes mtype)
        {
            var kind = GetKind(mtype);
            if (kind == SymbolKind.Other)
                return KnownSvgImages.ImgEmpty.AsImage(HasScaleFactor ? 32 : 16);
            var result = images?.GetImage(kind, !HasScaleFactor);
            return result;
        }

        public static SymbolKind GetKind(MemberTypes mtype)
        {
            if (mtype.HasFlag(MemberTypes.Field))
                return SymbolKind.Field;
            if (mtype.HasFlag(MemberTypes.Event))
                return SymbolKind.Event;
            if (mtype.HasFlag(MemberTypes.Property))
                return SymbolKind.Property;
            if (mtype.HasFlag(MemberTypes.Method))
                return SymbolKind.Method;
            return SymbolKind.Other;
        }

        public enum SymbolKind
        {
            Other,

            Field,
            Property,
            Method,
            Event,

            /*
            Class,
            Namespace,
            Constant,
            Keyword,
            Struct,
            Interface,
            Delegate,
            LocalOrParameter,
            GenericParameter,
            */
        }
    }
}
