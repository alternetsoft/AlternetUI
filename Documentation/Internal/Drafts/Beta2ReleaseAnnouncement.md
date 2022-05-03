# AlterNET UI Beta 2

AlterNET UI Beta 2 represents a significant milestone towards releasing a production-ready cross-platform
.NET UI framework for developing desktop applications on Windows, macOS and Linux.

In this release we have implemented some major features, primarily adopted from WPF framework.

## Layout Engine

In this version we refined the ideas of how layout system functions in AlterNET UI. We kept the API simple, similar to Windows Forms approach.
However, we adopted powerful layout containers such as Grid and StackPanel from WPF. The layout system automatically detects relevant property changes
and repositions all the affected controls. Together with performance optimizations, the layout system is capable
of implementing rich user user interfaces.

<Employee Form Demo Screenshots/videos here>

## Dependency Properties

We ported the dependency properties system quite closely from WPF, which allows to follow the same idioms regarding data.
Dependency properties give a higher order abstraction over regular CLR properties, and allow for automatic change tracking,
value inheritance based on controls parent-child relationships, and services for convenient access from UIXML.
Attached dependency properties allow for setting extra properties on any DependencyObject from UIXML.

To give a glimpse of what the dependency properties look like, here is a code that defines dependency properties:

```
public static readonly DependencyProperty IsSpinningProperty = DependencyProperty.Register(
    "IsSpinning", typeof(bool),
    typeof(MyObject)
    );

public bool IsSpinning
{
    get => (bool)GetValue(IsSpinningProperty);
    set => SetValue(IsSpinningProperty, value);
}
```

And the UIXML below consumes such a property:

```
<MyObject IsSpinning="True" />
```

## Data Binding

Data binding is also a concept taken from WPF world and is built on top of dependency properties.
Data binding is the process that establishes a connection between the app UI and the data it displays.
If the binding has the correct settings and the data provides the proper notifications, when the data changes its value,
the elements that are bound to the data reflect changes automatically. Data binding can also mean that if an outer representation
of the data in an element changes, then the underlying data can be automatically updated to reflect the change.

The following pair of C# and UIXML code snippets illustrate a typical scenario of data binding usage:

```
public MainWindow()
{
    InitializeComponent();

    DataContext = new Employee
    {
        FirstName = "Alice",
        LastName = "Jameson"
    };
}
```

```
<Window>
    <TextBox Text="{Binding FirstName}" />
    <TextBox Text="{Binding LastName}" />
</Window>
```

In the sample above, the text boxes defined in UIXML get filled from the `Employee` object automatically.
Also, when the user edits value of one of the text boxes, the corresponding `Employee` object properties get updated automatically.

## Keyboard and Mouse Input, Routed Events

Keyboard and mouse input API provides functionality for input events related to key presses, mouse buttons, mouse wheel, mouse movement, and mouse capture.

Related to the input support is concept of routed events. Routed events is another piece of technology adopted from WPF and allows for cascade-style handing, when an event
can "travel" up and down the controls hierarchy. The following example illustrates how the same mouse event can be handled on different levels in a window:

```
<StackPanel Orientation="Vertical" PreviewMouseDown="StackPanel_PreviewMouseDown" MouseDown="StackPanel_MouseDown">
	<Border MouseDown="Border_MouseDown">
		<Label Text="Click me." />
	</Border>
</StackPanel>
```

## And more

In Beta 2, we also added many other improvements big and small, such as new components and controls like `Timer` and `PictureBox`.
Many infrastractural imrovements were made and additional `DrawingContext` functionality support was added. Beta 2 also adds several new code samples.