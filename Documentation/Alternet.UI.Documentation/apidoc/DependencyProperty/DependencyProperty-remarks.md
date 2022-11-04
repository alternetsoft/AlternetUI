---
uid: Alternet.UI.DependencyProperty
remarks: *content
---
 A <xref:Alternet.UI.DependencyProperty> supports the following capabilities in
 Alternet UI:  
  
-   The property can be set through data binding. For more information about
    data binding dependency properties, see [Data
    binding](../../introduction/data-binding/data-binding.md).
  
-   The property can inherit its value automatically from a parent element in
    the element tree.
  
-   The property can report when the previous value of the property has been
    changed and the property value can be coerced.
  
-   The property reports information to Alternet UI, such as whether changing a
    property value should require the layout system to recompose the visuals for
    an element.  
    
 To learn more about dependency properties, see [Dependency
 Properties](../../introduction/dependency-properties/dependency-properties.md). If
 you want properties on your custom types to support the capabilities in the
 preceding list, you should create a dependency property.
  
 An attached property is a property that enables any object to report
 information to the type that defines the attached property. In Alternet UI, any
 type that inherits from <xref:System.Windows.DependencyObject> can use an
 attached property regardless of whether the type inherits from the type that
 defines the property. An attached property is a feature of the UIXML language.
 To set an attached property in UIXML, use the *ownerType*.*propertyName* syntax.
 An example of an attached property is the Grid.Column attached property implemented by the
 <xref:Alternet.UI.Grid.GetColumn*>/<xref:Alternet.UI.Grid.SetColumn*> methods.
 property. If you want to create a property that can be used on all
 <xref:Alternet.UI.DependencyObject> types, then you should create an
 attached property.