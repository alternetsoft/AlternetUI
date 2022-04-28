#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Implements ETW tracing for Alternet UI Managed Code

#if !SILVERLIGHTXAML

using System;
namespace Alternet.UI
{
    #region Trace

    static internal partial class EventTrace
    {
        static readonly internal TraceProvider EventProvider;

        // EasyTraceEvent
        // Checks the keyword and level before emiting the event
        static internal void EasyTraceEvent(Keyword keywords, Event eventID)
        {
            if (IsEnabled(keywords, Level.Info))
            {
                EventProvider.TraceEvent(eventID, keywords, Level.Info);
            }
        }

        // EasyTraceEvent
        // Checks the keyword and level before emiting the event
        static internal void EasyTraceEvent(Keyword keywords, Level level, Event eventID)
        {
            if (IsEnabled(keywords, level))
            {
                EventProvider.TraceEvent(eventID, keywords, level);
            }
        }

        // EasyTraceEvent
        // Checks the keyword and level before emiting the event
        static internal void EasyTraceEvent<T1>(Keyword keywords, Event eventID, T1 param1)
        {
            if (IsEnabled(keywords, Level.Info))
            {
                EventProvider.TraceEvent(eventID, keywords, Level.Info, param1);
            }
        }

        // EasyTraceEvent
        // Checks the keyword and level before emiting the event
        static internal void EasyTraceEvent<T1>(Keyword keywords, Level level, Event eventID, T1 param1)
        {
            if (IsEnabled(keywords, level))
            {
                EventProvider.TraceEvent(eventID, keywords, level, param1);
            }
        }

        // EasyTraceEvent
        // Checks the keyword and level before emiting the event
        static internal void EasyTraceEvent<T1, T2>(Keyword keywords, Event eventID, T1 param1, T2 param2)
        {
            if (IsEnabled(keywords, Level.Info))
            {
                EventProvider.TraceEvent(eventID, keywords, Level.Info, param1, param2);
            }
        }

        static internal void EasyTraceEvent<T1, T2>(Keyword keywords, Level level, Event eventID, T1 param1, T2 param2)
        {
            if (IsEnabled(keywords, Level.Info))
            {
                EventProvider.TraceEvent(eventID, keywords, Level.Info, param1, param2);
            }
        }

        // EasyTraceEvent
        // Checks the keyword and level before emiting the event
        static internal void EasyTraceEvent<T1, T2, T3>(Keyword keywords, Event eventID, T1 param1, T2 param2, T3 param3)
        {
            if (IsEnabled(keywords, Level.Info))
            {
                EventProvider.TraceEvent(eventID, keywords, Level.Info, param1, param2, param3);
            }
        }

        #region Trace related enumerations

        public enum LayoutSource : byte
        {
            LayoutManager,
            HwndSource_SetLayoutSize,
            HwndSource_WMSIZE
        }

        #endregion

        /// <summary>
        /// Callers use this to check if they should be logging.
        /// </summary>
        static internal bool IsEnabled(Keyword flag, Level level)
        {
            return EventProvider.IsEnabled(flag, level);
        }

        /// <summary>
        /// Internal operations associated with initializing the event provider and
        /// monitoring the Dispatcher and input components.
        /// </summary>
        static EventTrace()
        {
            Guid providerGuid = new Guid("E13B77A8-14B6-11DE-8069-001B212B5009");

            //if (Environment.OSVersion.Version.Major < 6 ||
            //    IsClassicETWRegistryEnabled())
            //{
            //    EventProvider = new ClassicTraceProvider();
            //}
            //else
            {
                EventProvider = new ManifestTraceProvider();
            }
            EventProvider.Register(providerGuid);
        }
    }

    #endregion Trace
}
#endif 
