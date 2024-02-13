# Using ToolBars
  
The AlterNET UI <xref:Alternet.UI.ToolBar> control is used as a control bar that displays a row of drop-down menus and
bitmapped buttons that activate commands. Thus, clicking a toolbar button is equivalent to choosing a menu command. 
You can configure toolbar buttons to appear and behave as push buttons, drop-down menus, or separators. Typically, a toolbar
contains buttons and menus corresponding to items in an application's menu structure, providing quick access to an
application's most frequently used functions and commands.  

# [Windows](#tab/screenshot-windows)
![ToolBar on Windows](./images/toolbar-windows.png)
# [macOS](#tab/screenshot-macos)
![ToolBar on macOS](./images/toolbar-macos.png)
# [Linux](#tab/screenshot-linux)
![ToolBar on Linux](./images/toolbar-linux.png)
***

## Working with the ToolBar Control  
A <xref:Alternet.UI.ToolBar> control is "docked" along the top of its parent window.
Use the <xref:Alternet.UI.Window.ToolBar> property to specify a toolbar associated with the window.

The <xref:Alternet.UI.ToolBar> control allows you to create toolbar items by adding
<xref:Alternet.UI.ToolBarItem> objects to a <xref:Alternet.UI.ToolBar.Items%2A> collection. Each
<xref:Alternet.UI.ToolBarItem> object should have <xref:Alternet.UI.ToolBarItem.Text> or an 
<xref:Alternet.UI.ToolBarItem.Image> assigned,
although you can assign both. The image is supplied by an associated <xref:Alternet.UI.ImageSet> component. At run time, you can
add or remove buttons from the <xref:Alternet.UI.ToolBar.Items%2A> collection. To program the items of a
<xref:Alternet.UI.ToolBar>, add code to the <xref:Alternet.UI.Control.Click> events of the
<xref:Alternet.UI.ToolBarItem> to determine which toolbar item was clicked.

Using <xref:Alternet.UI.ToolBarItem.IsCheckable> property,
 you can specify whether a <xref:Alternet.UI.ToolBarItem> can be "checked" or "toggled".
Use <xref:Alternet.UI.ToolBarItem.Checked> property to tell the check state of such items.

Use <xref:Alternet.UI.ToolBarItem.DropDownMenu> to specify a drop-down <xref:Alternet.UI.Menu> for a <xref:Alternet.UI.ToolBarItem>.

Set <xref:Alternet.UI.ToolBarItem>.<xref:Alternet.UI.ToolBarItem.Text> property to a *minus* ("-") value to use the item as a toolbar separator.

A toolbar can display tooltips when the user points the mouse pointer at a toolbar
button. A ToolTip is a small pop-up window briefly describing the button or menu's purpose.

The following example shows how to use <xref:Alternet.UI.ToolBar> component:

```xml
<Window>
    <Window.ToolBar>
        <ToolBar>
            <ToolBarItem Text="Calendar" Image="embres:MenuSample.Resources.Icons.Small.Calendar16.png"
                         Click="ToolBarItem_Click" ToolTip="Calendar ToolBar Item" />
            <ToolBarItem Text="-" />
            <ToolBarItem Text="Pencil Toggle" Image="embres:MenuSample.Resources.Icons.Small.Pencil16.png"
                         Click="ToggleToolBarItem_Click"
                         ToolTip="Pencil ToolBar Item" IsCheckable="true" Name="checkableToolBarItem" />
            <ToolBarItem Text="-" />
            <ToolBarItem Text="Graph Drop Down" ToolTip="Graph ToolBar Item"
                         Image="embres:MenuSample.Resources.Icons.Small.LineGraph16.png" Click="ToolBarItem_Click">
                <ToolBarItem.DropDownMenu>
                    <ContextMenu>
                        <MenuItem Text="_Open..." Name="openToolBarMenuItem" Click="ToolBarDropDownMenuItem_Click" />
                        <MenuItem Text="_Save..." Name="saveToolBarMenuItem" Click="ToolBarDropDownMenuItem_Click" />
                        <MenuItem Text="-" />
                        <MenuItem Text="E_xport..." Name="exportToolBarMenuItem" Click="ToolBarDropDownMenuItem_Click" />
                    </ContextMenu>
                </ToolBarItem.DropDownMenu>
            </ToolBarItem>
        </ToolBar>
    </Window.ToolBar>
</Window>
```

```csharp
private void ToolBarItem_Click(object? sender, EventArgs e)
{
    MessageBox.Show("ToolBar item clicked: " + ((ToolBarItem)sender!).Text);
}

private void ToggleToolBarItem_Click(object sender, EventArgs e)
{
    var item = (ToolBarItem)sender;
    MessageBox.Show($"Toggle toolbar item clicked: {item.Text}. Is checked: {item.Checked}");
}

private void ToolBarDropDownMenuItem_Click(object sender, EventArgs e)
{
    var item = (MenuItem)sender;
    MessageBox.Show($"ToolBar drop down menu item clicked: {item.Text.Replace("_", "")}.");
}
```
