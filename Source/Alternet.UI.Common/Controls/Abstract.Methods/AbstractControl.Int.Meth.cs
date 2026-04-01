using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        internal static Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesBgColor(controlType, renderSize);
        }

        internal static Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesFgColor(controlType, renderSize);
        }

        internal static Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesFont(controlType, renderSize);
        }

        internal void RaiseProcessException(ThrowExceptionEventArgs e)
        {
            OnProcessException(e);
            ProcessException?.Invoke(this, e);
        }

        internal void SetParentInternal(AbstractControl? value)
        {
            parent = value;
            LogicalParent = value;
        }
    }
}
