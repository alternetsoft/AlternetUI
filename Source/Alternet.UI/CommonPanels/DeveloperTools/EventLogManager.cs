using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal static class EventLogManager
    {
        public static void UpdateEventsPropertyGrid(PropertyGrid eventGrid, Type? type)
        {
            eventGrid.DoInsideUpdate(() =>
            {
                eventGrid.Clear();
                if (type == null)
                    return;
                var events = AssemblyUtils.EnumEvents(type, true);

                foreach (var item in events)
                {
                    if (item.DeclaringType != type)
                        continue;
                    var isBinded = LogUtils.IsEventLogged(type, item);
                    var prop = eventGrid.CreateBoolItem(item.Name, null, isBinded);
                    prop.FlagsAndAttributes.Attr["InstanceType"] = type;
                    prop.FlagsAndAttributes.Attr["EventInfo"] = item;
                    prop.PropertyChanged += Event_PropertyChanged;
                    eventGrid.Add(prop);
                }
            });
        }

        private static void Event_PropertyChanged(object? sender, EventArgs e)
        {
            if (sender is not IPropertyGridItem item)
                return;
            var type = item.FlagsAndAttributes.Attr.GetAttribute<Type?>("InstanceType");
            var eventInfo = item.FlagsAndAttributes.Attr.GetAttribute<EventInfo?>("EventInfo");
            var value = item.Owner.GetPropertyValueAsBool(item);
            LogUtils.SetEventLogged(type, eventInfo, value);
        }
    }
}
