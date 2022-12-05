# Printing Overview

Printing in AlterNET UI consists primarily of using the <xref:Alternet.Drawing.Printing.PrintDocument> component to
enable the user to print. The <xref:Alternet.UI.PrintPreviewDialog> control,
<xref:Alternet.UI.PrintDialog> and <xref:Alternet.UI.PageSetupDialog> components provide a familiar
graphical interface to users.

The `PrintDialog` component is a pre-configured dialog box used to select a printer, choose the pages to print, and
determine other print-related settings in UI applications. It's a simple solution for printer and
print-related settings instead of configuring your own dialog box. You can enable users to print many parts of their
documents: print all, print a selected page range, or print a selection. By relying on standard dialog boxes,
you create applications whose basic functionality is immediately familiar to users. The
<xref:Alternet.UI.PrintDialog> component inherits from the <xref:Alternet.UI.CommonDialog> class.

Typically, you create a new instance of the <xref:Alternet.Drawing.Printing.PrintDocument> component and set the
properties that describe what to print using the <xref:Alternet.Drawing.Printing.PrinterSettings> and
<xref:Alternet.Drawing.Printing.PageSettings> classes. Calling the <xref:Alternet.Drawing.Printing.PrintDocument.Print%2A>
method prints the document.

## Working with the component

Use the [PrintDialog.ShowModal](xref:Alternet.UI.CommonDialog.ShowModal%2A) method to display the dialog at
run time. This component has properties that relate to either a single print job
(<xref:Alternet.Drawing.Printing.PrintDocument> class) or the settings of an individual printer
(<xref:Alternet.Drawing.Printing.PrinterSettings> class). One of the two, in turn, may be shared by multiple printers.

## How to capture user input from a PrintDialog at run time

You can set options related to printing at design time. Sometimes you may want to change these options at run time, most
likely because of choices made by the user. You can capture user input for printing a document using the
<xref:Alternet.UI.PrintDialog> and the <xref:Alternet.Drawing.Printing.PrintDocument> components. The following
steps demonstrate displaying the print dialog for a document:

01. Add a <xref:Alternet.UI.PrintDialog> and a <xref:Alternet.Drawing.Printing.PrintDocument> component to your
    form.

01. Set the <xref:Alternet.UI.PrintDialog.Document%2A> property of the <xref:Alternet.UI.PrintDialog>
    to the <xref:Alternet.Drawing.Printing.PrintDocument> added to the form.

    ```csharp
    printDialog1.Document = printDocument1;
    ```

01. Display the <xref:Alternet.UI.PrintDialog> component by using the
    <xref:Alternet.UI.CommonDialog.ShowModal%2A> method.

    ```csharp
    // display show dialog, and if the user selects "Ok" the document is printed
    if (printDialog1.ShowDialog() == DialogResult.OK)
        printDocument1.Print();
    ```

01. The user's printing choices from the dialog will be copied to the <xref:Alternet.Drawing.Printing.PrinterSettings>
    property of the <xref:Alternet.Drawing.Printing.PrintDocument> component.

## How to create print jobs

The foundation of printing in AlterNET UI is the <xref:Alternet.Drawing.Printing.PrintDocument> component, more
specifically, the <xref:Alternet.Drawing.Printing.PrintDocument.PrintPage> event. By writing code to handle the
<xref:Alternet.Drawing.Printing.PrintDocument.PrintPage> event, you can specify what to print and how to print it. The
following steps demonstrate creating a print job:

01. Add a <xref:Alternet.Drawing.Printing.PrintDocument> component to your form.

01. Write code to handle the <xref:Alternet.Drawing.Printing.PrintDocument.PrintPage> event.

    You'll have to code your own printing logic. Additionally, you'll have to specify the material to be printed.

    As a material to print, in the following code example, a sample graphic in the shape of a red rectangle is created
    in the <xref:Alternet.Drawing.Printing.PrintDocument.PrintPage> event handler.

    ```csharp
    private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) =>
        e.Graphics.FillRectangle(Brushes.Red, new Rectangle(100, 100, 100, 100));
    ```

You can also write code for the <xref:Alternet.Drawing.Printing.PrintDocument.BeginPrint> and
<xref:Alternet.Drawing.Printing.PrintDocument.EndPrint> events. It will help to include an integer representing the total
number of pages to print that is decremented as each page prints.

For more information about the specifics of AlterNET UI print jobs, including how to create a print job
programmatically, see <xref:Alternet.Drawing.Printing.PrintPageEventArgs>.