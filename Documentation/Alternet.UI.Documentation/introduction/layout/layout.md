# Layout

This topic describes the AlterNET UI layout system. Understanding how and when layout calculations occur is essential for creating user interfaces in AlterNET UI.

## Control Bounding Boxes

When thinking about layout in AlterNET UI, it is important to understand the
bounding box that surrounds all controls. Each
<xref:Alternet.UI.Control> consumed by the layout system can be
thought of as a rectangle that is slotted into the layout. The size of the rectangle
is determined by calculating the available screen space, the size of any
constraints, layout-specific properties (such as margin and padding), and the
individual behavior of the parent control.
By processing this data, the layout system can calculate the position of all
the children of a particular <xref:Alternet.UI.Control>. It is
important to remember that sizing characteristics, defined on the parent control, such as a <xref:Alternet.UI.Border>, affect its children.

The following illustration shows a simple layout.

![Screenshot that shows a typical grid, no bounding box
superimposed.](./images/BoundingBox1.png)

This layout can be achieved by using the following UIXML.

[!code-xml[](./snippets/BoundingBox1.uixml)]

A <xref:Alternet.UI.Label> control is hosted within a
<xref:Alternet.UI.Grid>. While the text fills only the upper-left
corner of the first column, the allocated space for the
containing <xref:Alternet.UI.Border> is actually much larger. The bounding
box of any <xref:Alternet.UI.Control> can be retrieved by using the
<xref:Alternet.UI.Control.Bounds>
method. The following illustration shows the bounding box for the
<xref:Alternet.UI.Label> control.

![Screenshot that shows that the Border bounding box is now
visible.](./images/BoundingBox2.png)

As shown by the orange rectangle, the allocated space for the
<xref:Alternet.UI.Label> control is actually much larger than it
appears. As additional controls are added to the
<xref:Alternet.UI.Grid>, this allocation could shrink or expand,
depending on the type and size of controls that are added.

The layout bounds of the <xref:Alternet.UI.Border> are highlighted
by setting the <xref:Alternet.UI.Control.BorderBrush> property.

[!code-csharp[](./snippets/BoundingBox.cs)]

## The Layout System

At its simplest, the layout is a recursive system that leads to control being
sized, positioned, and drawn. More specifically, layout describes the process of
measuring and arranging the members of a <xref:Alternet.UI.Control>'s
<xref:Alternet.UI.Control.Children%2A> collection. The layout is
an intensive process. The larger the
<xref:Alternet.UI.Control.Children%2A> collection, the greater the
number of calculations that must be made. Complexity can also be introduced
based on the layout behavior defined by the <xref:Alternet.UI.Control>
control that owns the collection. A relatively simple
layout <xref:Alternet.UI.Control>, such as
<xref:Alternet.UI.Border>, can have significantly better performance
than a more complex <xref:Alternet.UI.Control>, such as
<xref:Alternet.UI.Grid>.

Each time that a child <xref:Alternet.UI.Control> changes its position, it
has the potential to trigger a new pass by the layout system. Therefore, it is
important to understand the events that can invoke the layout system, as
unnecessary invocation can lead to poor application performance. The following
describes the process that occurs when the layout system is invoked.

1. A child <xref:Alternet.UI.Control> generally begins the layout process by first measuring itself
   by having its core sizing properties
   evaluated, such as <xref:Alternet.UI.Control.Width%2A>,
   <xref:Alternet.UI.Control.Height%2A>, and
   <xref:Alternet.UI.Control.Margin%2A>.

2. After that, a custom <xref:Alternet.UI.Control.GetPreferredSize%2A> implementation may change the desired control's size.

3. Layout <xref:Alternet.UI.Control>-specific logic is applied, such as
   <xref:Alternet.UI.StackPanel>'s <xref:Alternet.UI.Control.OnLayout%2A> logic and its related properties, such as <xref:Alternet.UI.StackPanel.Orientation%2A>.

4. The control bounds are set after all children have been measured and laid out.

5. The process is invoked again if additional
   <xref:Alternet.UI.Control.Children%2A> are added to the collection, or
   the <xref:Alternet.UI.Control.PerformLayout%2A> method is called.

## Measuring and Positioning Children

The layout system typically performs two operations for each member of the
<xref:Alternet.UI.Control.Children%2A> collection, a measure and
a layout. Each child <xref:Alternet.UI.Control> provides its
own <xref:Alternet.UI.Control.GetPreferredSize%2A> and
<xref:Alternet.UI.Control.OnLayout%2A> methods to achieve its
own specific layout behavior.

By default, a control provides a base measure and layout logic.
It considers several base control inputs to perform its operation.

First, native size properties of the <xref:Alternet.UI.Control> are
evaluated, such as
<xref:Alternet.UI.Control.Visible%2A>.
Secondly, the properties which affect the value of the control's preferred size are processed. These properties
generally describe the sizing characteristics of the underlying
<xref:Alternet.UI.Control>, such as its
<xref:Alternet.UI.Control.Height%2A>,
<xref:Alternet.UI.Control.Width%2A>,
<xref:Alternet.UI.Control.Margin%2A>,
<xref:Alternet.UI.Control.Padding%2A>,
<xref:Alternet.UI.Control.HorizontalAlignment%2A>, and
<xref:Alternet.UI.Control.VerticalAlignment%2A>. Each of these properties can
change the space that is necessary to display the control.

The ultimate goal of the measurement process is for the child to determine its preferred size, which occurs during the
<xref:Alternet.UI.Control.GetPreferredSize%2A> call.

During the layout process, the
parent <xref:Alternet.UI.Control> control generates a rectangle that
represents the bounds of the child. This value is set to the
<xref:Alternet.UI.Control.Bounds%2A> property.

The layout logic evaluates the
preferred size of the child and evaluates any
additional properties that may affect the actual size of the control, such as margin and alignment, and puts the
child within its layout slot. The child does not have to (and frequently does
not) fill the entire allocated space. After that, the layout process is complete.

## Standard Layout Controls

AlterNET UI includes a group of controls that enable complex layouts. For example, stacking controls can easily
be achieved by using the <xref:Alternet.UI.StackPanel> control,
while more complex layouts are possible by using a
<xref:Alternet.UI.Grid>.

The following table summarizes the available layout controls.

|Control name|Description|
|----------------|-----------------|
|<xref:Alternet.UI.Grid>|Defines a flexible grid area that consists of columns and rows.|
|<xref:Alternet.UI.StackPanel>|Arranges child controls into a single line that can be oriented horizontally or vertically.|
|<xref:Alternet.UI.VerticalStackPanel>|Arranges child controls into a single line that can be oriented vertically.|
|<xref:Alternet.UI.HorizontalStackPanel>|Arranges child controls into a single line that can be oriented horizontally.|
|<xref:Alternet.UI.SplitterPanel>|Manages up to two subcontrols which are aligned vertically or horizontally with splitter control between them.|
|<xref:Alternet.UI.LayoutPanel>|Arranges child controls using different methods.|
|<xref:Alternet.UI.AuiManager>|Implement advanced docking and floating toolbars and panes framework.|

## Custom Layout Behaviors

For applications that require a layout that is not possible by using any of the
predefined <xref:Alternet.UI.Control> controls, custom layout
behaviors can be achieved by inheriting from
<xref:Alternet.UI.Control> and overriding the
<xref:Alternet.UI.Control.GetPreferredSize%2A> and
<xref:Alternet.UI.Control.OnLayout%2A> methods.