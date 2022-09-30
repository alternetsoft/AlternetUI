# Drag-and-Drop Support

You can enable user drag-and-drop operations within an AlterNET UI application by handling a series of events, most
notably the <xref:Alternet.UI.Control.DragEnter>, <xref:Alternet.UI.Control.DragLeave>, and
<xref:Alternet.UI.Control.DragDrop> events.

## Drag-and-drop events

There are two categories of events in a drag and drop operation: events that occur on the current target of the
drag-and-drop operation, and events that occur on the source of the drag and drop operation. To perform drag-and-drop
operations, you must handle these events. By working with the information available in the event arguments of these
events, you can easily facilitate drag-and-drop operations.

## Events on the current drop target

The following table shows the events that occur on the current target of a drag-and-drop operation.

| Mouse Event                          | Description                                                                                                                                                                                            |
|--------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| <xref:Alternet.UI.Control.DragEnter> | This event occurs when an object is dragged into the control's bounds. The handler for this event receives an argument of type <xref:Alternet.UI.DragEventArgs>.                              |
| <xref:Alternet.UI.Control.DragOver>  | This event occurs when an object is dragged while the mouse pointer is within the control's bounds. The handler for this event receives an argument of type <xref:Alternet.UI.DragEventArgs>. |
| <xref:Alternet.UI.Control.DragDrop>  | This event occurs when a drag-and-drop operation is completed. The handler for this event receives an argument of type <xref:Alternet.UI.DragEventArgs>.                                      |
| <xref:Alternet.UI.Control.DragLeave> | This event occurs when an object is dragged out of the control's bounds. The handler for this event receives an argument of type <xref:System.EventArgs>.                                              |

The <xref:Alternet.UI.DragEventArgs> class provides the location of the mouse pointer, the current state of the
mouse buttons and modifier keys of the keyboard, the data being dragged, and <xref:Alternet.UI.DragDropEffects>
values that specify the operations allowed by the source of the drag event and the target drop effect for the operation.

## Performing drag-and-drop

Drag-and-drop operations always involve two components, the **drag source** and the **drop target**. To start a
drag-and-drop operation, designate a control as the source and handle the <xref:Alternet.UI.UIElement.MouseDown>
event. In the event handler, call the <xref:Alternet.UI.Control.DoDragDrop%2A> method providing the data associated
with the drop and the a <xref:Alternet.UI.DragDropEffects> value.

Set the target control's <xref:Alternet.UI.Control.AllowDrop> property set to `true` to allow that control to
accept a drag-and-drop operation. The target handles two events, first an event in response to the drag being over the
control, such as <xref:Alternet.UI.Control.DragOver>. And a second event which is the drop action itself,
<xref:Alternet.UI.Control.DragDrop>.

The following example demonstrates a drag from a <xref:Alternet.UI.Label> control to a
<xref:Alternet.UI.TextBox>. When the drag is completed, the `TextBox` responds by assigning the label's text to
itself.

```csharp

textBox1.AllowDrop = true;
// ...

// Initiate the drag
private void label1_MouseDown(object sender, MouseEventArgs e) =>
    DoDragDrop(((Label)sender).Text, DragDropEffects.Copy);

// Set the effect filter and allow the drop on this control
private void textBox1_DragOver(object sender, DragEventArgs e) =>
    e.Effect = DragDropEffects.Copy;

// React to the drop on this control
private void textBox1_DragDrop(object sender, DragEventArgs e) =>
    textBox1.Text = (string)e.Data.GetData(DataFormats.Text);
```

## Dragging Data  
 All drag-and-drop operations begin with dragging. The functionality to enable data to be collected when dragging begins
 is implemented in the <xref:Alternet.UI.Control.DoDragDrop%2A> method.  
  
 In the following example, the <xref:Alternet.UI.UIElement.MouseDown> event is used to start the drag operation
 because it is the most intuitive (most drag-and-drop actions begin with the mouse button being depressed). However,
 remember that any event could be used to initiate a drag-and-drop procedure.  
  
### To start a drag operation  
  
1. In the <xref:Alternet.UI.UIElement.MouseDown> event for the control where the drag will begin, use the
   `DoDragDrop` method to set the data to be dragged and the allowed effect dragging will have. For more information,
   see <xref:Alternet.UI.DragEventArgs.Data%2A> and <xref:Alternet.UI.DragEventArgs.Effect%2A>.  
  
The following example shows how to initiate a drag operation. The control where the drag begins is a
<xref:Alternet.UI.Button> control, the data being dragged is the string representing the
<xref:Alternet.UI.ButtonBase.Text%2A> property of the <xref:Alternet.UI.Button> control, and the
allowed effects are either copying or moving.  
  
```csharp  
private void button1_MouseDown(object sender, Alternet.UI.MouseEventArgs e)  
{  
    button1.DoDragDrop(button1.Text, DragDropEffects.Copy |
        DragDropEffects.Move);  
}  
```  
  
## Dropping Data  
 Once you have begun dragging data from a location on a window or control, you will naturally want to drop it
 somewhere. The cursor will change when it crosses an area of a window or control that is correctly configured for
 dropping data. Any area within a window or control can be made to accept dropped data by setting the
 <xref:Alternet.UI.Control.AllowDrop%2A> property and handling the
 <xref:Alternet.UI.Control.DragEnter> and <xref:Alternet.UI.Control.DragDrop> events.  
  
### To perform a drop  
  
1. Set the <xref:Alternet.UI.Control.AllowDrop%2A> property to true.  
  
2. In the `DragEnter` event for the control where the drop will occur, ensure that the data being dragged is of an
   acceptable type (in this case, <xref:Alternet.UI.Label.Text%2A>). The code then sets the effect that will
   happen when the drop occurs to a value in the <xref:Alternet.UI.DragDropEffects> enumeration. For more
   information, see <xref:Alternet.UI.DragEventArgs.Effect%2A>.  
  
```csharp  
private void textBox1_DragEnter(object sender, Alternet.UI.DragEventArgs e)  
{  
    if (e.Data.GetDataPresent(DataFormats.Text))
        e.Effect = DragDropEffects.Copy;  
    else  
        e.Effect = DragDropEffects.None;  
}  
```  
  
3. In the <xref:Alternet.UI.Control.DragDrop> event for the control where the drop will occur, use the
   <xref:Alternet.UI.DataObject.GetData%2A> method to retrieve the data being dragged. 
  
     In the example below, a <xref:Alternet.UI.TextBox> control is the control being dragged to (where the drop
     will occur). The code sets the <xref:Alternet.UI.TextBox.Text%2A> property of the
     <xref:Alternet.UI.TextBox> control equal to the data being dragged.  
  
```csharp  
private void textBox1_DragDrop(object sender, Alternet.UI.DragEventArgs e)  
{  
    textBox1.Text = e.Data.GetData(DataFormats.Text).ToString();  
}  
```