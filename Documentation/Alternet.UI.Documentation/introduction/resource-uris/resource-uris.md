# Using Resource URIs

In AlterNET UI, uniform resource identifiers (URIs) are used to identify and load files in the following scenarios:

- Loading images.

- Loading data files.

- Anoy other scenario when a read-only access to a resource file is required.

## Using `embres:` Scheme

`embres:` scheme is used to load an embedded resource from an assembly. The URIs in this scheme have the following format:

```text
embres:Manifest.Resource.Name[?assembly=assembly-name]
```

The following is the example of using an image from a resource embedded into the current assembly :

```xml
<PictureBox Image="embres:EmployeeFormSample.Resources.EmployeePhoto.jpg" />
```

The resource the example above is embedded to the assembly in the following way (an excerpt from the `.csproj` file):

```xml
<ItemGroup>
    <EmbeddedResource Include="Resources\EmployeePhoto.jpg" />
</ItemGroup>
```

The `EmployeeFormSample` part of the manifest resource name comes from the assembly *root namespace*, which is the same
as the assembly name by default.