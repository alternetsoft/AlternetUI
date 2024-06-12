using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class Control
    {
        public void BubbleHelpRequested(HelpEventArgs e)
        {
            RaiseHelpRequested(e);

            if (!e.Handled)
                Parent?.BubbleHelpRequested(e);
        }

        public virtual void BubbleKeyPress(KeyPressEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                form.RaiseKeyPress(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                control.RaiseKeyPress(e);

                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        public virtual void BubbleKeyUp(KeyEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null && form.KeyPreview)
            {
                e.CurrentTarget = form;
                form.RaiseKeyUp(e);
                if (e.Handled)
                    return;
            }
            else
                form = null;

            while (control is not null && control != form)
            {
                e.CurrentTarget = control;
                control.RaiseKeyUp(e);
                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }

        public virtual void BubbleKeyDown(KeyEventArgs e)
        {
            var control = this;
            var form = ParentWindow;
            if (form is not null)
            {
                if (form.KeyPreview)
                {
                    e.CurrentTarget = form;
                    form.RaiseKeyDown(e);
                    if (e.Handled)
                        return;
                }
                else
                    form = null;
            }

            while (control is not null && control != form)
            {
                e.CurrentTarget = control;
                control.RaiseKeyDown(e);

                if (e.Handled)
                    return;
                control = control.Parent;
            }
        }
    }
}
