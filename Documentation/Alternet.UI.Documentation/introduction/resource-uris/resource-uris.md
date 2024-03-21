# Using Resource URIs

In AlterNET UI, uniform resource identifiers (URIs) are used to identify and load files in the following scenarios:

- Loading images.

- Loading data files.

- Any other scenario when read-only access to a resource file is required.

## Using `embres:` Scheme

`embres:` scheme is used to load an embedded resource from an assembly. The URIs in this scheme have the following format:

```text
embres:Manifest.Resource.Name[?assembly=assembly-name]
```

The following is an example of using an image from a resource embedded into the current assembly :

```xml
<PictureBox Image="embres:EmployeeFormSample.Resources.EmployeePhoto.jpg" />
```

The resource in the example above is embedded into the assembly in the following way (an excerpt from the `.csproj` file):

```xml
<ItemGroup>
    <EmbeddedResource Include="Resources\EmployeePhoto.jpg" />
</ItemGroup>
```

The `EmployeeFormSample` part of the manifest resource name comes from the assembly *root namespace*, which is the same
as the assembly name by default.

## Using `file:` Scheme

`file:` scheme is used to load a file. The URIs in this scheme have the following format:

```text
file://<host>/<path>
```

### Linux:

These urls point to the same file <b>/etc/fstab</b>:

```text
file://localhost/etc/fstab
file:///etc/fstab
file:///etc/./fstab
file:///etc/../etc/fstab
```

### Mac OS:
These urls point to the same file <b>/var/log/system.log</b>:

```text
file://localhost/var/log/system.log
file:///var/log/system.log
```

### Windows:

These urls point to the same file <b>c:\WINDOWS\clock.avi</b>:

```text
file://localhost/c|/WINDOWS/clock.avi
file:///c|/WINDOWS/clock.avi
file://localhost/c:/WINDOWS/clock.avi
file:///c:/WINDOWS/clock.avi
```