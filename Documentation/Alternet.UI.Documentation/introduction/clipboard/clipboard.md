# Clipboard Support

You can implement user cut/copy/paste support and user data transfer to the Clipboard within your AlterNET UI
applications by using simple method calls.

The <xref:Alternet.UI.Clipboard> class provides methods that you can use to interact with the operating system
Clipboard feature. Many applications use the Clipboard as a temporary repository for data. For example, word processors
use the Clipboard during cut-and-paste operations. The Clipboard is also useful for transferring data from one
application to another.

## Add Data to the Clipboard

When you add data to the Clipboard, you can indicate the data format so that other applications can recognize the data
if they can use that format. You can also add data to the Clipboard in multiple different formats to increase the number
of other applications that can potentially use the data.

A Clipboard format is a string that identifies the format so that an application that uses that format can retrieve the
associated data. The <xref:Alternet.UI.DataFormats> class provides predefined format names for your use. You can also
use your own format names or use the type of an object as its format.

To add data to the Clipboard in one or multiple formats, use the <xref:Alternet.UI.Clipboard.SetDataObject%2A> method.
You can pass any object to this method, but to add data in multiple formats, you must first add the data to a separate
object designed to work with multiple formats. Typically, you will add your data to a <xref:Alternet.UI.DataObject>, but
you can use any type that implements the <xref:Alternet.UI.IDataObject> interface.

> [!NOTE]
> All applications in an OS environment share the Clipboard. Therefore, the contents are subject to change when you switch to another application.

The following sample illustrates how to add a textual data to the Clipboard:

```csharp
private void CopyButton_Click(object sender, System.EventArgs e)
{
    Clipboard.SetText("my string");
}
```

The sample below illustrates how to add data in multiple formats to the Clipboard:

```csharp
private void CopyButton_Click(object sender, System.EventArgs e)
{
    var data = new DataObject();
    data.SetData(DataFormats.Text, "my text string");
    data.SetData(DataFormats.Files, new[] { "c:\\myfile.txt" });

    Clipboard.SetDataObject(data);
}
```

## Retrieve Data from the Clipboard

Some applications store data on the Clipboard in multiple formats to increase the number of other applications that can
potentially use the data. A Clipboard format is a string that identifies the format. An application that uses the
identified format can retrieve the associated data on the Clipboard. The <xref:Alternet.UI.DataFormats> class
provides predefined format names for your use. You can also use your own format names or use an object's type as its
format.

To determine whether the Clipboard contains data in a particular format, use one of the `Contains`*Format* methods or
the <xref:Alternet.UI.Clipboard.GetData%2A> method. To retrieve data from the Clipboard, use one of the
`Get`*Format* methods or the <xref:Alternet.UI.Clipboard.GetData%2A> method.

The following sample illustrates how to retrieve data from the Clipboard:

```csharp
private void PasteButton_Click(object sender, System.EventArgs e)
{
    if (Clipboard.ContainsText)
        MessageBox.Show("Text from the clipboard: " + Clipboard.GetText());

    if (Clipboard.ContainsData(DataFormats.Files))
    {
        string[]? fileNames = Clipboard.GetFiles();
        // Process the file names here.
    }
}
```