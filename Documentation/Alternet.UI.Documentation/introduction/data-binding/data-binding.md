# Data Binding

Data binding in AlterNET UI provides a simple and
consistent way for apps to present and interact with data. Elements can be bound
to data from different kinds of data sources in the form of .NET objects.

The data binding in AlterNET UI has several advantages over traditional models,
including inherent support for data binding by a broad range of properties and clean separation of business logic from
UI.

This article first discusses concepts fundamental to AlterNET UI data binding and then
covers the usage of the <xref:Alternet.UI.Binding> class and other
features of data binding.

## What is data binding?

Data binding is the process that establishes a connection between the app UI and
the data it displays. If the binding has the correct settings and the data
provides the proper notifications, when the data changes its value, the elements
that are bound to the data reflect changes automatically. Data binding can also
mean that if an outer representation of the data in an element changes, then the
underlying data can be automatically updated to reflect the change. For example,
if the user edits the value in a `TextBox` element, the underlying data value is
automatically updated to reflect that change.

A typical use of data binding is to place server or local configuration data
into forms or other UI controls. In AlterNET UI, this concept is expanded to include
binding a broad range of properties to different kinds of data sources.

## Basic data binding concepts

Data binding is essentially the bridge between your binding
target and your binding source.

- Typically, each binding has four components:

  - A binding target object.
  - A target property.
  - A binding source.
  - A path to the value in the binding source to use.
  
  For example, if you bound the content of a `TextBox` to the `Employee.Name`
  property, you would set up your binding like the following table:

  | Setting                  | Value                                          |
  |--------------------------|------------------------------------------------|
  | Target                   | <xref:Alternet.UI.TextBox>         |
  | Target property          | <xref:Alternet.UI.TextBox.Text%2A> |
  | Source object            | `Employee`                                     |
  | Source object value path | `Name`                                         |

- The target property must be a dependency property.

  Most <xref:Alternet.UI.UIElement> properties are dependency properties, and
  most dependency properties, except read-only ones, support data binding by
  default. Only types derived from <xref:Alternet.UI.DependencyObject> can
  define dependency properties. All <xref:Alternet.UI.UIElement> types derive
  from `DependencyObject`.

It's important to remember that when you're establishing a binding, you're
binding a binding target *to* a binding source. For example, if you're
displaying some data in a <xref:Alternet.UI.ListBox>
using data binding, you're binding your `ListBox` to the data.

To establish a binding, you use the <xref:Alternet.UI.Binding> object.
The rest of this article discusses many of the concepts associated with and some
of the properties and usage of the `Binding` object.

### Data context

When data binding is declared on UIXML elements, they resolve data binding by
looking at their immediate <xref:Alternet.UI.FrameworkElement.DataContext%2A>
property. The data context is typically the **binding source object** for the
**binding source value path** evaluation. You can override this behavior in the
binding and set a specific **binding source object** value. If the `DataContext`
property for the object hosting the binding isn't set, the parent element's
`DataContext` property is checked, and so on, up until the root of the UIXML
object tree. In short, the data context used to resolve binding is inherited
from the parent unless explicitly set on the object.

Bindings can be configured to resolve with a specific object, as opposed to
using the data context for binding resolution. Specifying a source object
directly is used when, for example, you bind the foreground color of an object
to the background color of another object. Data context isn't needed since the
binding is resolved between those two objects. Inversely, bindings that aren't
bound to specific source objects use data-context resolution.

When the `DataContext` property changes, all bindings that could be affected by
the data context are reevaluated.

### Direction of the data flow

As indicated by the arrow in the previous figure, the data flow of a binding can
go from the binding target to the binding source (for example, the source value
changes when a user edits the value of a `TextBox`) and/or from the binding
source to the binding target (for example, your `TextBox` content is updated
with changes in the binding source) if the binding source provides the proper
notifications.

You may want your app to enable users to change the data and propagate it back
to the source object. Or you may not want to enable users to update the source
data. You can control the flow of data by setting the
<xref:Alternet.UI.Binding.Mode?displayProperty=nameWithType>.

- <xref:Alternet.UI.BindingMode.OneWay> binding causes changes to the
  source property to automatically update the target property, but changes to
  the target property are not propagated back to the source property. This type
  of binding is appropriate if the control being bound is implicitly read-only.
  For instance, you may bind to a source such as a stock ticker, or perhaps your
  target property has no control interface provided for making changes, such as
  a data-bound background color of a table. If there's no need to monitor the
  changes of the target property, using the
  <xref:Alternet.UI.BindingMode.OneWay> binding mode avoids the overhead
  of the <xref:Alternet.UI.BindingMode.TwoWay> binding mode.

- <xref:Alternet.UI.BindingMode.TwoWay> binding causes changes to either
  the source property or the target property to automatically update the other.
  This type of binding is appropriate for editable forms or other fully
  interactive UI scenarios. Most properties default to
  <xref:Alternet.UI.BindingMode.OneWay> binding, but some dependency
  properties (typically properties of user-editable controls such as the
  <xref:Alternet.UI.TextBox.Text?displayProperty=nameWithType> and
  <xref:Alternet.UI.CheckBox.IsChecked?displayProperty=nameWithType>
  default to <xref:Alternet.UI.BindingMode.TwoWay> binding. A
  programmatic way to determine whether a dependency property binds one-way or
  two-way by default is to get the property metadata with
  <xref:Alternet.UI.DependencyProperty.GetMetadata%2A?displayProperty=nameWithType>
  and then check the Boolean value of the
  <xref:Alternet.UI.FrameworkPropertyMetadata.BindsTwoWayByDefault%2A?displayProperty=nameWithType>
  property.

- <xref:Alternet.UI.BindingMode.OneWayToSource> is the reverse of
  <xref:Alternet.UI.BindingMode.OneWay> binding; it updates the source
  property when the target property changes. One example scenario is if you only
  need to reevaluate the source value from the UI.

- Not illustrated in the figure is
  <xref:Alternet.UI.BindingMode.OneTime> binding, which causes the
  source property to initialize the target property but doesn't propagate
  subsequent changes. If the data context changes or the object in the data
  context changes, the change is *not* reflected in the target property. This
  type of binding is appropriate if either a snapshot of the current state is
  appropriate or the data is truly static. This type of binding is also useful
  if you want to initialize your target property with some value from a source
  property and the data context isn't known in advance. This mode is essentially
  a simpler form of <xref:Alternet.UI.BindingMode.OneWay> binding that
  provides better performance in cases where the source value doesn't change.

To detect source changes (applicable to
<xref:Alternet.UI.BindingMode.OneWay> and
<xref:Alternet.UI.BindingMode.TwoWay> bindings), the source must
implement a suitable property change notification mechanism such as
<xref:System.ComponentModel.INotifyPropertyChanged>.

### What triggers source updates

Bindings that are <xref:Alternet.UI.BindingMode.TwoWay> or
<xref:Alternet.UI.BindingMode.OneWayToSource> listen for changes in the
target property and propagate them back to the source, known as updating the
source. For example, you may edit the text of a TextBox to change the underlying
source value.

However, is your source value updated while you're editing the text or after you
finish editing the text and the control loses focus? The
<xref:Alternet.UI.Binding.UpdateSourceTrigger?displayProperty=nameWithType>
property determines what triggers the update of the source.

If the `UpdateSourceTrigger` value is
<xref:Alternet.UI.UpdateSourceTrigger.PropertyChanged?displayProperty=nameWithType>,
then the value pointed to by the right arrow of
<xref:Alternet.UI.BindingMode.TwoWay> or the
<xref:Alternet.UI.BindingMode.OneWayToSource> bindings is updated as
soon as the target property changes. However, if the `UpdateSourceTrigger` value
is <xref:Alternet.UI.UpdateSourceTrigger.LostFocus>, then that value
only is updated with the new value when the target property loses focus.

Similar to the <xref:Alternet.UI.Binding.Mode%2A> property, different
dependency properties have different default
<xref:Alternet.UI.Binding.UpdateSourceTrigger%2A> values. The default
value for most dependency properties is
<xref:Alternet.UI.UpdateSourceTrigger.PropertyChanged>, which causes the
source property's value to instantly change when the target property value is
changed. Instant changes are fine for <xref:Alternet.UI.CheckBox>
and other simple controls. However, for text fields, updating after every
keystroke can diminish performance and denies the user the usual opportunity to
backspace and fix typing errors before committing to the new value. For example,
the `TextBox.Text` property defaults to the `UpdateSourceTrigger` value of
<xref:Alternet.UI.UpdateSourceTrigger.LostFocus>, which causes the
source value to change only when the control element loses focus, not when the
`TextBox.Text` property is changed. See the
<xref:Alternet.UI.Binding.UpdateSourceTrigger%2A> property page for
information about how to find the default value of a dependency property.

The following table provides an example scenario for each
<xref:Alternet.UI.Binding.UpdateSourceTrigger%2A> value using the
<xref:Alternet.UI.TextBox> as an example.

| UpdateSourceTrigger value | When the source value is updated | Example scenario for TextBox |
| ------------------------- | ---------------------------------- | ---------------------------- |
| `LostFocus` (default for <xref:Alternet.UI.TextBox.Text%2A?displayProperty=nameWithType>) | When the TextBox control loses focus. | A TextBox that is associated with validation logic. |
| `PropertyChanged` | As you type into the <xref:Alternet.UI.TextBox>. | TextBox controls in a chat room window. |
| `Explicit` | When the app calls <xref:Alternet.UI.BindingExpression.UpdateSource%2A>. | TextBox controls in an editable form (updates the source values only when the user presses the submit button). |