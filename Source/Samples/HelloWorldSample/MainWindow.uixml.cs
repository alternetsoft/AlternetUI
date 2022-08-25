using System;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace HelloWorldSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HelloButton_Click(object? sender, EventArgs e)
        {
            //MessageBox.Show("Hello, world!");

            var control = new Window();

            Window window = null;

            if (control is Window w)
            {
                window = w;
                //window.ShowInTaskbar = false;
                //window.Resizable = false;
                window.IsToolWindow = true;
            }

            //currentWindow = window;
            window.Show();

            var timer = new Timer(TimeSpan.FromMilliseconds(10));

            timer.Tick += (o, e) =>
            {
                timer.Stop();
                timer.Dispose();

                //var targetDirectoryPath = Path.Combine(Path.GetTempPath(), "AlterNET.UIXMLPreviewer", "UIImages");
                //if (!Directory.Exists(targetDirectoryPath))
                //    Directory.CreateDirectory(targetDirectoryPath);

                //var targetFilePath = Path.Combine(targetDirectoryPath, Guid.NewGuid().ToString("N") + ".png");
                //SaveScreenshot(window, targetFilePath);

                window.Close();
                window.Dispose();
            };

            timer.Start();
        }
    }
}