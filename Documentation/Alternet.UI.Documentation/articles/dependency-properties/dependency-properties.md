# Dependency Properties

AlterNET UI provides a set of services that can be
used to extend the functionality of a type's
property.
Collectively, these services are referred to as the AlterNET UI property system. A
property that's backed by the AlterNET UI property system is known as a dependency
property. This overview describes the AlterNET UI property system and the capabilities
of a dependency property, including how to use existing dependency properties in
UIXML and in code. This overview also introduces specialized aspects of
dependency properties, such as dependency property metadata, and how to create
your own dependency property in a custom class.

## Prerequisites

This article assumes basic knowledge of the .NET type system and object-oriented
programming. To follow the examples in this article, it helps to understand UIXML
and know how to write AlterNET UI applications.

## Dependency properties and CLR properties

AlterNET UI properties are typically exposed as standard .NET
properties. You
might interact with these properties at a basic level and never know that
they're implemented as a dependency property. However, familiarity with some or
all of the features of the AlterNET UI property system, will help you take advantage of
those features.

The purpose of dependency properties is to provide a way to compute the value of
a property based on the value of other inputs, such as:

- System properties, such as themes and user preference.
- Just-in-time property determination mechanisms, such as data binding and
  animations/storyboards.
- Multiple-use templates, such as resources and styles.
- Values known through parent-child relationships with other elements in the
  element tree.

Also, a dependency property can provide:

- Self-contained validation.
- Default values.
- Callbacks that monitor changes to other properties.
- A system that can coerce property values based on runtime information.

Derived classes can change some characteristics of an existing property by
overriding the metadata of a dependency property, rather than overriding the
actual implementation of existing properties or creating new properties.

In the SDK reference, you can identify a dependency property by the presence of
a Dependency Property Information section on the managed reference page for that
property. The Dependency Property Information section includes a link to the
<xref:Alternet.UI.DependencyProperty> identifier field for that dependency
property. It also includes the list of metadata options for that property,
per-class override information, and other details.

## Dependency properties back CLR properties

Dependency properties and the AlterNET UI property system extend property functionality
by providing a type that backs a property, as an alternative to the standard
pattern of backing a property with a private field. The name of this type is
<xref:Alternet.UI.DependencyProperty>. The other important type that defines
the AlterNET UI property system is <xref:Alternet.UI.DependencyObject>, which defines
the base class that can register and own a dependency property.

Here's some commonly used terminology:

- **Dependency property**, which is a property that's backed by a
  <xref:Alternet.UI.DependencyProperty>.

- **Dependency property identifier**, which is a `DependencyProperty` instance
  obtained as a return value when registering a dependency property, and then
  stored as a static member of a class. Many of the APIs that interact with the
  AlterNET UI property system use the dependency property identifier as a parameter.

- **CLR "wrapper"**, which is the `get` and `set` implementations for the
  property. These implementations incorporate the dependency property identifier
  by using it in the <xref:Alternet.UI.DependencyObject.GetValue%2A> and
  <xref:Alternet.UI.DependencyObject.SetValue%2A> calls. In this way, the AlterNET UI
  property system provides the backing for the property.

The following example defines the `IsSpinning` dependency property to show the
relationship of the `DependencyProperty` identifier to the property that it
backs.

[!code-csharp[](./snippets/DefineDependencyProperty.cs)]

The naming convention of the property and its backing
<xref:Alternet.UI.DependencyProperty> field is important. The name of the
field is always the name of the property, with the suffix `Property` appended.

## Setting property values

You can set properties either in code or in UIXML.

### Setting property values in UIXML

The following UIXML example sets the background color of a button to red. The
string value for the UIXML attribute is type-converted by the AlterNET UI UIXML parser
into a AlterNET UI type. In the generated code, the AlterNET UI type is a
<xref:Alternet.Drawing.Color>, by way of a
<xref:Alternet.Drawing.SolidBrush>.

[!code-xml[](./snippets/BasicPropertySyntax.uixml)]

UIXML supports several syntax forms for setting properties. Which syntax to use
for a particular property depends on the value type that a property uses, and
other factors, such as the presence of a type converter.

The following UIXML example shows another button background that uses property
element syntax instead of attribute syntax. Rather than setting a simple solid
color, the UIXML sets the button `Background` property to an image. An element
represents that image, and an attribute of the nested element specifies the
source of the image.

[!code-xml[](./snippets/PropertyElementSyntax.uixml)]

### Setting properties in code

Setting dependency property values in code is typically just a call to the `set`
implementation exposed by the CLR "wrapper":

[!code-csharp[](./snippets/ProceduralPropertySet.cs)]

Getting a property value is essentially a call to the `get` "wrapper"
implementation:

[!code-csharp[](./snippets/ProceduralPropertyGet.cs)]

You can also call the property system APIs
<xref:Alternet.UI.DependencyObject.GetValue%2A> and
<xref:Alternet.UI.DependencyObject.SetValue%2A> directly. Calling the APIs
directly is appropriate for some scenarios, but usually not when you're using
existing properties. Typically, wrappers are more convenient, and provide better
exposure of the property for developer tools.

Properties can be also set in UIXML and then accessed later in code, through
code-behind.

## Property functionality provided by a dependency property

Unlike a property that's backed by a field, a dependency property extends the
functionality of a property. Often, the added functionality represents or
supports one of the features described below.

### Data binding

A dependency property can reference a value through data binding. Data binding
works through a specific markup extension syntax in UIXML, or the
<xref:Alternet.UI.Binding> object in code. With data binding,
determination of the final property value is deferred until run time, at which
time the value is obtained from a data source.

The following example sets the
<xref:Alternet.UI.ButtonBase.Text> property for a
<xref:Alternet.UI.Button>, by using a binding declared in UIXML.


[!code-xml[](./snippets/BasicInlineBinding.uixml)]

> [!NOTE]
> Bindings are treated as a local value, which means that if you set
> another local value, you'll eliminate the binding.

Dependency properties, or the <xref:Alternet.UI.DependencyObject> class,
don't natively support <xref:System.ComponentModel.INotifyPropertyChanged> for
notification of changes in `DependencyObject` source property value for data
binding operations. For more about how to create properties for use in data
binding that can report changes to a data binding target, see [Data binding](../data-binding/data-binding.md).

### Property value inheritance

An element can inherit the value of a dependency property from its parent in the
object tree.

> [!NOTE]
> Property value inheritance behavior isn't globally enabled for all
> dependency properties, because the calculation time for inheritance affects
> performance. Property value inheritance is typically only enabled in scenarios
> that suggest applicability. You can check whether a dependency property
> inherits by looking at the documenatation for
> that dependency property in the API reference.

The following example shows a binding that includes the
<xref:Alternet.UI.FrameworkElement.DataContext%2A> property to specify the
source of the binding. So, bindings in child objects don't need to specify the
source and can use the inherited value from `DataContext` in the parent
<xref:Alternet.UI.StackPanel> object. Or, a child object can
directly specify its own `DataContext` or a
<xref:Alternet.UI.Binding.Source%2A> in the
<xref:Alternet.UI.Binding>, and not use the inherited value.

[!code-xml[](./snippets/InheritanceBindingContext.uixml)]