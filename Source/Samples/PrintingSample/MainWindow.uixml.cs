using System;
using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI;

namespace PrintingSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Grid_Paint(object? sender, PaintEventArgs e)
        {
            var dc = e.DrawingContext;
            Draw(dc);
        }

        private static void Draw(DrawingContext dc)
        {
            dc.DrawRectangle(Pens.Blue, new Rect(10, 10, 100, 100));
        }

        private void PrintImmediatelyMenuItem_Click(object sender, System.EventArgs e)
        {
            var document = new PrintDocument();
            document.PrintPage += Document_PrintPage;
            document.Print();
        }

        private void Document_PrintPage(object? sender, PrintPageEventArgs e)
        {
            Draw(e.DrawingContext);
        }

        private void AboutMenuItem_Click(object sender, System.EventArgs e) => MessageBox.Show("AlterNET UI Printing Sample Application.", "About");

        private void ExitMenuItem_Click(object sender, System.EventArgs e) => Close();
    }
}