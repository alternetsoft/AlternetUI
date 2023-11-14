using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    public static class EventLogManager
    {
        private static ICustomFlags flags = Factory.CreateCustomFlags();

        public static bool IsEventLogged(Type? type, EventInfo? evt)
        {
            var key = GetKey(type, evt);
            if(key is null)
                return false;
            var result = flags.HasFlag(key);
            return result;            
        }

        public static void SetEventLogged(Type? type, EventInfo? evt, bool logged)
        {
            var key = GetKey(type, evt);
            if (key is null)
                return;
            flags.SetFlag(key, logged);
        }

        private static string? GetKey(Type? type, EventInfo? evt)
        {
            if (type is null || evt is null)
                return null;
            var key = type.Name + "." + evt.Name;
            return key;
        }
    }
}
