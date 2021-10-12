# Rendering Graphics with DrawingContext

In this tutorial you will learn how to create a custom <xref:Alternet.UI.Control> which draws itself on screen using <xref:Alternet.Drawing.DrawingContext> class.

1. Create a new AlterNET UI Application project, name it `DrawingContextTutorial`. For a step-by-step guideance on how to create a new AlterNET UI project,
    see [Quick Start Tutorial](../quick-start/quick-start.md).
1. Add a new empty class named `DrawingControl` to the project. Make the class `public`, and derive it from <xref:Alternet.UI.Control>:
   [!code-csharp[](examples/DrawingContextTutorial/DrawingControl-Step1.cs?highlight=1,5)]
1. Open `MainWindow.uixml`. Add the reference to the local namespace, and add a `DrawingControl` to the window:
   [!code-xml[](examples/DrawingContextTutorial/MainWindow.uixml?highlight=5,6)]
1. Compile and run the application. An empty window will appear. This is because `DrawingControl` does not paint itself.
1. In the `DrawingControl` class, add a default constructor. In its body, set <xref:Alternet.UI.Control.UserPaint> property to `true`:
   [!code-csharp[](examples/DrawingContextTutorial/DrawingControl-Step2.cs?highlight=10)]
1. In the `DrawingControl` class, override the <xref:Alternet.UI.Control.OnPaint*> method:
   [!code-csharp[](examples/DrawingContextTutorial/DrawingControl-Step3.cs?highlight=13-15)]
1. In the overriden `OnPaint` method add the following
   <xref:Alternet.Drawing.DrawingContext.FillRectangle*?text=DrawingContext.FillRectangle> call to
   paint the control's background <xref:Alternet.Drawing.Brushes.LightBlue>:
   [!code-csharp[](examples/DrawingContextTutorial/DrawingControl-Step4.cs?highlight=15)]