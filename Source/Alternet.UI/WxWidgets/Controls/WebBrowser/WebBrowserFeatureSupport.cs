using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WebBrowserFeatureSupport
    {
        public bool? CanAddScriptMessageHandlerSingle { get; set; }

        public bool? CanAddScriptMessageHandlerMultiple { get; set; }

        public bool? CanAddUserScript { get; set; }

        public bool? CanAddUserScriptOnlyDocStart { get; set; }

        public bool? CanClearHistory { get; set; }

        public bool? CanAccessDevTools { get; set; }

        public bool? CanEnableHistory { get; set; }

        public bool? RegisterHandlerBeforeCreate { get; set; }

        public bool? ReloadFlagsIgnored { get; set; }

        public bool? CanSetEditable { get; set; }

        public bool? BaseUrlInSetPage { get; set; }

        public bool? CanSeUserAgent { get; set; }

        public bool? SetUserAgentBeforeCreate { get; set; }

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
