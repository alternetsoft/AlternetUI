using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class AuiNotebookPage : BaseControlItem, IAuiNotebookPage
    {
        private AuiNotebook? notebook;

        public AuiNotebookPage(AuiNotebook? notebook = null)
        {
            this.notebook = notebook;
        }

        public int? Index { get; internal set; }

        public AuiNotebook? Notebook
        {
            get => notebook;
            internal set => notebook = value;
        }
    }
}
