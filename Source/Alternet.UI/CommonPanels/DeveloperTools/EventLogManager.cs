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
        private static readonly ICustomFlags Flags = Factory.CreateCustomFlags();

        public static bool IsEventLogged(string eventId) => Flags.HasFlag(eventId);

        public static string? GetEventKey(Type? type, EventInfo? evt)
        {
            if (type is null || evt is null)
                return null;
            var key = type.Name + "." + evt.Name;
            return key;
        }

        public static bool IsEventLogged(Type? type, EventInfo? evt)
        {
            var key = GetEventKey(type, evt);
            if(key is null)
                return false;
            var result = Flags.HasFlag(key);
            return result;
        }

        public static void SetEventLogged(Type? type, EventInfo? evt, bool logged)
        {
            var key = GetEventKey(type, evt);
            if (key is null)
                return;
            Flags.SetFlag(key, logged);
        }

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
                    var isBinded = EventLogManager.IsEventLogged(type, item);
                    var prop = eventGrid.CreateBoolItem(item.Name, null, isBinded);
                    prop.FlagsAndAttributes.SetAttribute("InstanceType", type);
                    prop.FlagsAndAttributes.SetAttribute("EventInfo", item);
                    prop.PropertyChanged += Event_PropertyChanged;
                    eventGrid.Add(prop);
                }
            });
        }

        private static void Event_PropertyChanged(object? sender, EventArgs e)
        {
            if (sender is not IPropertyGridItem item)
                return;
            var type = item.FlagsAndAttributes.GetAttribute<Type?>("InstanceType");
            var eventInfo = item.FlagsAndAttributes.GetAttribute<EventInfo?>("EventInfo");
            var value = item.Owner.GetPropertyValueAsBool(item);
            EventLogManager.SetEventLogged(type, eventInfo, value);
        }
    }
}
