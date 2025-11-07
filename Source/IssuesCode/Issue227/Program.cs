// See https://aka.ms/new-console-template for more information

using System;

using Alternet.UI;

namespace EditorX.Engine
{

public class MainWindow : Window
{
    public MainWindow()
    {
        var menu = new MainMenu();
        var fileMenu = new MenuItem("_File");
        var fileMenuNew = new MenuItem("_New", OnFileNew);
        fileMenu.Items.Add(fileMenuNew);
        menu.Items.Add(fileMenu);
        Menu = menu;
    }

    private void OnFileNew(object? sender, EventArgs e)
    {
        
    }

        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }

}


}