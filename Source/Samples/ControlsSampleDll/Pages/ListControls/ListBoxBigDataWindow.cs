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

            listBox.Parent = this;
            listBox.VerticalAlignment = VerticalAlignment.Fill;

            textBox.TextChanged += ComboBox_TextChanged;

            statusBar.Parent = this;

            longOperationProgressBar.Visible = false;
            statusBar.AddControl(longOperationProgressBar);
            statusBar.SetVisibleBorders(false, true, false, false);
            statusBar.MinHeight = 24;
            statusPanelId = statusBar.AddText("Ready");
            statusPanel = statusBar.GetToolControl(statusPanelId);

        }

        public VirtualListBox.AddRangeController<MemberInfo> CreateController()
        {
            var containsText = lastReportedText ?? string.Empty;

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

        ListControlItem? ConvertItem(MemberInfo member)
        {
            var fullName = $"{member.DeclaringType.FullName}.{member.Name}";
            return new ListControlItem(fullName);
        }

        private void StartThread()
        {
            SafeDisposeObject(ref controller);
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
    }
}
