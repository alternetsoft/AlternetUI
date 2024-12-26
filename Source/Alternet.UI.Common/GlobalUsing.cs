global using EnumArrayStateImages
    = Alternet.UI.EnumArray<Alternet.UI.VisualControlState, Alternet.Drawing.Image?>;

global using ObjectToStringEventArgs = Alternet.UI.ValueConvertEventArgs<object?, string?>;
global using StringToObjectEventArgs = Alternet.UI.ValueConvertEventArgs<string?, object?>;

[assembly: Alternet.UI.XmlnsDefinition("http://schemas.alternetsoft.com/ui/2021", "Alternet.UI")]

namespace Alternet.UI.Internal
{
}

#pragma warning disable
namespace Alternet.UI.Threading
{
}
#pragma warning restore