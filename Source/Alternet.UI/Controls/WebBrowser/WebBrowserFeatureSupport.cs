using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WebBrowserFeatureSupport
    {
        public bool? CanAddScriptMessageHandlerSingle;
        public bool? CanAddScriptMessageHandlerMultiple;
        public bool? CanAddUserScript;
        public bool? CanAddUserScriptOnlyDocStart;
        public bool? CanClearHistory;
        public bool? CanAccessDevTools;
        public bool? CanEnableHistory;
        public bool? RegisterHandlerBeforeCreate;
        public bool? ReloadFlagsIgnored;
        public bool? CanSetEditable;
        public bool? BaseUrlInSetPage;
        public bool? CanSeUserAgent;
        public bool? SetUserAgentBeforeCreate;

        internal void InitIE()
        {
            CanAddScriptMessageHandlerSingle = false;
            CanAddScriptMessageHandlerMultiple = false;
            CanAddUserScript = false;
            CanAddUserScriptOnlyDocStart = false;
            CanClearHistory = true;
            CanAccessDevTools = false;
            CanEnableHistory = true;
            RegisterHandlerBeforeCreate = false;
            ReloadFlagsIgnored = false;
            CanSetEditable = true;
            BaseUrlInSetPage = false;
            CanSeUserAgent = false;
            SetUserAgentBeforeCreate = false;
        }

        internal void InitEdge()
        {
            CanAddScriptMessageHandlerSingle = true;
            CanAddScriptMessageHandlerMultiple = false;
            CanAddUserScript = true;
            CanAddUserScriptOnlyDocStart = true;
            CanClearHistory = true;
            CanAccessDevTools = true;
            CanEnableHistory = true;
            RegisterHandlerBeforeCreate = false;
            ReloadFlagsIgnored = true;
            CanSetEditable = false;
            BaseUrlInSetPage = false;
            CanSeUserAgent = true;
            SetUserAgentBeforeCreate = true;
        }

        internal void InitMacOs()
        {
            CanAddScriptMessageHandlerSingle = true;
            CanAddScriptMessageHandlerMultiple = true;
            CanAddUserScript = true;
            CanAddUserScriptOnlyDocStart = false;
            CanClearHistory = false;
            CanAccessDevTools = true;
            CanEnableHistory = false;
            RegisterHandlerBeforeCreate = true;
            ReloadFlagsIgnored = false;
            CanSetEditable = false;
            BaseUrlInSetPage = true;
            CanSeUserAgent = true;
            SetUserAgentBeforeCreate = false;
        }

        internal void InitWebKit2GTK()
        {
            CanAddScriptMessageHandlerSingle = true;
            CanAddScriptMessageHandlerMultiple = true;
            CanAddUserScript = true;
            CanAddUserScriptOnlyDocStart = false;
            CanClearHistory = false;
            CanAccessDevTools = true;
            CanEnableHistory = false;
            RegisterHandlerBeforeCreate = false;
            ReloadFlagsIgnored = false;
            CanSetEditable = true;
            BaseUrlInSetPage = true;
            CanSeUserAgent = true;
            SetUserAgentBeforeCreate = false;
        }
    }
}
