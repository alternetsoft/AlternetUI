﻿using System;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window
    {
        private readonly StatusBar statusbar = new();
        private readonly bool disableResize = false;
        private readonly PanelTreeAndCards mainPanel;

        static MainTestWindow()
        {
            AuiNotebook.DefaultCreateStyle = AuiNotebookCreateStyle.Top;
            Test.DoTests();
        }

        public MainTestWindow()
        {
            mainPanel = new(InitPanel);

            if (disableResize)
            {
                this.Resizable = false;
                this.MaximizeEnabled = false;
                this.MinimizeEnabled = false;
                mainPanel.RightPane.Resizable(false).DockFixed(true).Fixed();
                mainPanel.BottomPane.Resizable(false);
                mainPanel.CenterPane.DockFixed(true).Fixed();
                mainPanel.LeftPane.Resizable(false).DockFixed(true)
                    .MaxSizeDip(mainPanel.DefaultRightPaneBestSize).Fixed();
            }

            Icon = new("embres:ControlsTest.Sample.ico");

            InitializeComponent();

            this.StatusBar = statusbar;
            mainPanel.Parent = this;
            mainPanel.BindApplicationLog();

            int CreateWebBrowserPages()
            {
                int result = -1;

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.Edge))
                {
                    PanelWebBrowser.UseBackend = WebBrowserBackend.Edge;
                    result = AddWebBrowserPage("Browser Edge");
                }

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.WebKit))
                {
                    PanelWebBrowser.UseBackend = WebBrowserBackend.WebKit;
                    result = AddWebBrowserPage("Browser WebKit");
                }

                if (WebBrowser.IsBackendAvailable(WebBrowserBackend.IELatest))
                {
                    PanelWebBrowser.UseBackend = WebBrowserBackend.IELatest;
                    result = AddWebBrowserPage("Browser IE");
                }

                return result;
            }

            mainPanel.Add("Custom Draw Test", new CustomDrawTestPage());
            mainPanel.Add("Sizer Test", new SizerTestPage());
            int webBrowserPageIndex = CreateWebBrowserPages();

            mainPanel.LeftTreeView.SelectedItem = mainPanel.LeftTreeView.FirstItem;
            mainPanel.Manager.Update();

            if (!disableResize)
            {
                mainPanel.SizeChanged += Log_SizeChanged;
                mainPanel.CardPanel.SizeChanged += Log_SizeChanged;
            }

            mainPanel.Name = "mainPanel";
            Name = "MainTestWindow";
            mainPanel.CardPanel.Name = "cardPanel";
        }

        internal PanelTreeAndCards MainPanel => mainPanel;

        private void InitPanel(PanelAuiManager panel)
        {
            panel.LeftTreeViewAsListBox = true;
            panel.DefaultRightPaneMinSize = new(150, 150);
            panel.DefaultRightPaneBestSize = new(150, 150);
        }

        private void LogSizeEvent(object? sender, string evName)
        {
            var control = sender as Control;
            var name = control?.Name ?? control?.GetType().Name;
            Application.Log($"{evName}: {name}, Bounds: {control!.Bounds}");
        }

        private void Log_SizeChanged(object? sender, EventArgs e)
        {
            LogSizeEvent(sender, "SizeChanged");
        }

        private int AddWebBrowserPage(string title)
        {
            Control Fn()
            {
                var result = new WebBrowserTestPage
                {
                    PageName = title,
                };

                result.Name = result.GetType().Name;

                result.SizeChanged += Log_SizeChanged;

                if (disableResize)
                {
                    result.PanelWebBrowser.RightPane.Resizable(false);
                }

                return result;
            }

            return mainPanel.Add(title, Fn);
        }
    }
}