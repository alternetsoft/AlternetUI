using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    internal class VComboBoxWindow : Window
    {
        private static int globalCounter;

        private readonly int counter;

        private string? lastReportedText;

        private Thread? thread;

        private readonly ComboBox comboBox = new()
        {
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

        private readonly ObjectUniqueId statusPanelId;

        public VComboBoxWindow()
        {
            counter = ++globalCounter;

            Size = (800, 600);
            Title = "ComboBox and VirtualListBox with BigData";
            StartLocation = WindowStartLocation.ScreenTopRight;
            Layout = LayoutStyle.Vertical;
            MinChildMargin = 10;

            comboBox.Parent = this;

            listBox.Parent = this;
            listBox.VerticalAlignment = VerticalAlignment.Fill;

            comboBox.TextChanged += ComboBox_TextChanged;

            statusBar.Parent = this;

            longOperationProgressBar.Visible = false;
            statusBar.AddControl(longOperationProgressBar);
            statusBar.SetVisibleBorders(false, true, false, false);
            statusBar.MinHeight = 24;
            statusPanelId = statusBar.AddText("Ready");
        }

        private void EndThread(ref Thread? thread)
        {
            thread?.Interrupt();
            thread = null;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            App.IdleLog($"VComboBoxWindow{counter} Closed");
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            App.IdleLog($"VComboBoxWindow{counter} Disposed");
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
            App.IdleLog($"VComboBoxWindow{counter} Closing");
            EndThread(ref thread);
        }

        void ResetListBox()
        {
            EndThread(ref thread);
            listBox.RemoveAll();
            statusBar.SetToolText(statusPanelId, listBox.Items.Count);
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            ResetListBox();
        }

        private void CallThreadAction(Action action)
        {
            try
            {
                action();
            }
            catch (ThreadInterruptedException)
            {
                App.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' awoken.");
            }
            catch (ThreadAbortException)
            {
                App.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' aborted.");
            }
            finally
            {
                App.IdleLog($"Thread '{counter}.{Thread.CurrentThread.Name}' executing finally block.");
            }
        }

        private void ThreadAction()
        {
            void Fn()
            {
                var containsText = lastReportedText ?? string.Empty;

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                List<ListControlItem> items = new(50);
                SortedList<string, int> addedMethods = new();

                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetExportedTypes();


                    foreach (var type in types)
                    {
                        var typeName = type.FullName;
                        var props = type.GetProperties();

                        foreach (var prop in props)
                        {
                            if (!AddMember(prop.Name))
                                return;
                        }

                        var methods = type.GetMethods();
                        addedMethods.Clear();

                        foreach (var method in methods)
                        {
                            if (method.IsSpecialName)
                                continue;
                            if (addedMethods.ContainsKey(method.Name))
                                continue;
                            addedMethods.Add(method.Name, 0);
                            if (!AddMember(method.Name))
                                return;
                        }

                        bool AddMember(string memberName)
                        {
                            var result = true;
                            var s = $"{typeName}.{memberName}";

                            if (s.IndexOf(
                                containsText,
                                StringComparison.CurrentCultureIgnoreCase) >= 0)
                            {
                                ListControlItem item = new(s);
                                items.Add(item);
                            }

                            if (items.Count > 10)
                            {
                                Invoke(() =>
                                {
                                    if (IsDisposed || listBox.IsDisposed
                                    || containsText != lastReportedText)
                                    {
                                        result = false;
                                        return;
                                    }
                                    listBox.Items.AddRange(items);
                                    statusBar.SetToolText(statusPanelId, listBox.Items.Count);
                                });
                                items.Clear();
                                Thread.Sleep(200);
                            }

                            return result;
                        }
                    }
                }
            }

            CallThreadAction(Fn);
        }

        private void ThreadAction1()
        {
            void Fn()
            {
                for (int i = 0; ; i++)
                {
                    BeginInvoke(() =>
                    {
                        if (IsDisposed)
                        {
                            App.IdleLog($"VComboBoxWindow{counter} is already disposed");
                        }
                        else
                        {
                            var prefix = "Thread counter:";
                            App.LogReplace($"{prefix} {i}", prefix);
                        }
                    });
                    Thread.Sleep(1000);
                }
            }

            CallThreadAction(Fn);
        }

        private void StartThread()
        {
            EndThread(ref thread);
            thread = new Thread(ThreadAction)
            {
                Name = "1",
                IsBackground = true,
            };

            thread.Start();
        }

        protected override void OnIdle(EventArgs e)
        {
            if (lastReportedText == comboBox.Text)
                return;
            lastReportedText = comboBox.Text;

            var prefix = "Idle: ComboBox.TextChanged";
            App.LogReplace($"{prefix}: {comboBox.Text}", prefix);
            ResetListBox();
            if (lastReportedText.Length > 0)
                StartThread();
        }
    }
}
