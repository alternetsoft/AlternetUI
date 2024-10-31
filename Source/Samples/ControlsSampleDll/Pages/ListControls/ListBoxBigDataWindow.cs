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
        private static EnumImages<SymbolKind>? images;

        internal bool IsDebugInfoLogged = false;

        private static int globalCounter;

        private readonly int counter;
        private readonly ObjectUniqueId statusPanelId;

        private string? lastReportedText;
        private AbstractControl? statusPanel;
        private VirtualListBox.AddRangeController<MemberInfo>? controller;

        private readonly TextBox textBox = new()
        {
            EmptyTextHint = "Type here to search for classes and members...",
        };

        private readonly VirtualListBox listBox = new()
        {
        };

        private readonly ToolBar statusBar = new()
        {
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        private readonly ProgressBar longOperationProgressBar = new()
        {
        };

        public ListBoxBigDataWindow()
        {
            counter = ++globalCounter;

            Size = (800, 600);
            Title = "VirtualListBox with BigData";
            StartLocation = WindowStartLocation.ScreenTopRight;
            Layout = LayoutStyle.Vertical;
            MinChildMargin = 10;

            textBox.Parent = this;

            listBox.VerticalAlignment = VerticalAlignment.Fill;
            listBox.Parent = this;
            listBox.SelectionUnderImage = false;

            textBox.TextChanged += ComboBox_TextChanged;

            statusBar.Parent = this;

            longOperationProgressBar.Visible = false;
            statusBar.AddControl(longOperationProgressBar);
            statusBar.SetVisibleBorders(false, true, false, false);
            statusBar.MinHeight = 24;
            statusPanelId = statusBar.AddText("Ready");
            statusPanel = statusBar.GetToolControl(statusPanelId);

            LoadImages();
        }

        private static void LoadImages()
        {
            if (images != null)
                return;
            images = new();

            string prefix = "Resources.CodeComletionSymbols.";
            images.SetImageName(SymbolKind.Namespace, $"{prefix}NamespaceAlpha");
            images.SetImageName(SymbolKind.Struct, $"{prefix}StructAlpha");
            images.SetImageName(SymbolKind.Field, $"{prefix}FieldAlpha");
            images.SetImageName(SymbolKind.Constant, $"{prefix}ConstantAlpha");
            images.SetImageName(SymbolKind.Class, $"{prefix}ClassAlpha");
            images.SetImageName(SymbolKind.Delegate, $"{prefix}DelegateAlpha");
            images.SetImageName(SymbolKind.Event, $"{prefix}EventAlpha");
            images.SetImageName(SymbolKind.GenericParameter, $"{prefix}GenericParameterAlpha");
            images.SetImageName(SymbolKind.Interface, $"{prefix}InterfaceAlpha");
            images.SetImageName(SymbolKind.Keyword, $"{prefix}KeywordAlpha");
            images.SetImageName(SymbolKind.LocalOrParameter, $"{prefix}LocalOrParameterAlpha");
            images.SetImageName(SymbolKind.Method, $"{prefix}MethodAlpha");
            images.SetImageName(SymbolKind.Property, $"{prefix}PropertyAlpha");
            images.AssignImageNames(true);
            images.FormatImageNames("{0}_HighDpi.png", false);
            images.FormatImageNames("{0}.png", true);

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
                    statusPanel?.SetText(listBox.Items.Count);
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
            Class,
            Method,
            Property,
            Field,
            Namespace,
            Constant,
            Keyword,
            Struct,
            Interface,
            Delegate,
            LocalOrParameter,
            Event,
            GenericParameter,
        }
    }
}
