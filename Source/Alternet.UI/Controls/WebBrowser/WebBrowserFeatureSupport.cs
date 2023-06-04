using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WebBrowserFeatureSupport
    {
        public bool? F01CanAddScriptMessageHandlerSingle;
        public bool? F02CanAddScriptMessageHandlerMultiple;
        public bool? F03CanAddUserScript;
        public bool? F04CanAddUserScriptOnlyDocStart;
        public bool? F05CanClearHistory;
        public bool? F06CanAccessDevTools;
        public bool? F07CanEnableHistory;
        public bool? F08RegisterHandlerBeforeCreate;
        public bool? F09ReloadFlagsIgnored;
        public bool? F10CanSetEditable;
        public bool? F11BaseUrlInSetPage;
        public bool? F12CanSeUserAgent;
        public bool? F13SetUserAgentBeforeCreate;

        internal void InitIE()
        {
            F01CanAddScriptMessageHandlerSingle = false;
            F02CanAddScriptMessageHandlerMultiple = false;
            F03CanAddUserScript = false;
            F04CanAddUserScriptOnlyDocStart = false;
            F05CanClearHistory = true;
            F06CanAccessDevTools = false;
            F07CanEnableHistory = true;
            F08RegisterHandlerBeforeCreate = false;
            F09ReloadFlagsIgnored = false;
            F10CanSetEditable = true;
            F11BaseUrlInSetPage = false;
            F12CanSeUserAgent = false;
            F13SetUserAgentBeforeCreate = false;
        }

        internal void InitEdge()
        {
            F01CanAddScriptMessageHandlerSingle = true;
            F02CanAddScriptMessageHandlerMultiple = false;
            F03CanAddUserScript = true;
            F04CanAddUserScriptOnlyDocStart = true;
            F05CanClearHistory = true;
            F06CanAccessDevTools = true;
            F07CanEnableHistory = true;
            F08RegisterHandlerBeforeCreate = false;
            F09ReloadFlagsIgnored = true;
            F10CanSetEditable = false;
            F11BaseUrlInSetPage = false;
            F12CanSeUserAgent = true;
            F13SetUserAgentBeforeCreate = true;
        }

        internal void InitMacOs()
        {
            F01CanAddScriptMessageHandlerSingle = true;
            F02CanAddScriptMessageHandlerMultiple = true;
            F03CanAddUserScript = true;
            F04CanAddUserScriptOnlyDocStart = false;
            F05CanClearHistory = false;
            F06CanAccessDevTools = true;
            F07CanEnableHistory = false;
            F08RegisterHandlerBeforeCreate = true;
            F09ReloadFlagsIgnored = false;
            F10CanSetEditable = false;
            F11BaseUrlInSetPage = true;
            F12CanSeUserAgent = true;
            F13SetUserAgentBeforeCreate = false;
        }

        internal void InitWebKit2GTK()
        {
            F01CanAddScriptMessageHandlerSingle = true;
            F02CanAddScriptMessageHandlerMultiple = true;
            F03CanAddUserScript = true;
            F04CanAddUserScriptOnlyDocStart = false;
            F05CanClearHistory = false;
            F06CanAccessDevTools = true;
            F07CanEnableHistory = false;
            F08RegisterHandlerBeforeCreate = false;
            F09ReloadFlagsIgnored = false;
            F10CanSetEditable = true;
            F11BaseUrlInSetPage = true;
            F12CanSeUserAgent = true;
            F13SetUserAgentBeforeCreate = false;
        }
    }
}
