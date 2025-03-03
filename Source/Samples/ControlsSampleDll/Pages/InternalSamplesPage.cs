using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using System.IO;
using System.Diagnostics;

namespace ControlsSample
{
    public partial class InternalSamplesPage : CustomInternalSamplesPage
    {
        protected override void AddDefaultItems()
        {
            /*
                Add("NinePatch Drawing Sample", () => new NinePatchDrawingWindow());
            */


            if (App.IsWindowsOS)
            {
                InternalSamplesPage.Add("Action Simulator Sample", () => new ActionSimulatorPage());
            }

            Add("Threading Sample", () => new ThreadingSample.ThreadingMainWindow());
            
            AddIfDebug("Draw Test Page: Custom", () => new CustomDrawTestPage());
            AddIfDebug("Draw Test Page: Skia", () => new SkiaDrawingWindow());
            
            AddIfDebug("Controls Test Window", () => new ControlsTestWindow());           

            Add("Documentation Samples", () => new ApiDoc.MainWindowSimple());
            Add("Preview File Sample", () => new PreviewSample.PreviewSampleWindow());
            Add("Explorer UI Sample", () => new ExplorerUISample.ExplorerMainWindow());
            Add("Printing Sample", () => new PrintingSample.PrintingMainWindow());
            Add("Menu and ToolBar Sample", () => new MenuSample.MenuMainWindow());
            Add("Mouse Input", () => new InputSample.MouseInputWindow());
            Add("Keyboard Input", () => new InputSample.KeyboardInputWindow());
            Add("Drag and Drop", () => new DragAndDropSample.DragAndDropWindow());
            Add("SkiaSharp MegaDemo", () => new SkiaSharpExamplesWindow());
            Add("Paint Sample", () => new PaintSample.PaintMainWindow());
            Add("Custom Controls", () => new CustomControlsSample.CustomControlsWindow());
            Add("Window Properties", () => new WindowPropertiesSample.WindowPropertiesWindow());
            Add("Common Dialogs", () => new CommonDialogsWindow());
            Add("Employee Form", () => new EmployeeFormSample.EmployeeWindow());
            Add("Drawing Sample", () => new DrawingSample.DrawingMainWindow());
            Add("Property Grid", () => new PropertyGridSample.MainWindow());
        }
    }
}