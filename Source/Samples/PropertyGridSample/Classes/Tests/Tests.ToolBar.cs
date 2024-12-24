using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        void InitTestsToolBar()
        {
            PropertyGrid.AddSimpleAction<ToolBar>("Test Visible", TestGenericToolBarVisible);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Enabled", TestGenericToolBarEnabled);

            PropertyGrid.AddSimpleAction<ToolBar>(
                "Test Delete (Without Dispose)",
                ()=> { TestGenericToolBarDelete(); });

            PropertyGrid.AddSimpleAction<ToolBar>(
                "Test Delete (With Dispose)",
                () => { TestGenericToolBarDelete(true); });

            AddControlAction<ToolBar>("Test Dispose 1st item", (c) =>
            {
                var child = c.GetToolControlAt(0);

                if (child is not null)
                {
                    child.Disposed += (s, e) =>
                    {
                        App.Log($"ToolBar item '{c.GetType()}' disposed");
                    };

                    child.Dispose();
                }
            });


            PropertyGrid.AddSimpleAction<ToolBar>("Test Sticky", TestGenericToolBarSticky);
            PropertyGrid.AddSimpleAction<ToolBar>(
                "Test Foreground Color",
                TestGenericToolBarForegroundColor);
            PropertyGrid.AddSimpleAction<ToolBar>(
                "Test Background Color",
                TestGenericToolBarBackgroundColor);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Font", TestGenericToolBarFont);
            PropertyGrid.AddSimpleAction<ToolBar>("Test Background", TestGenericToolBarBackground);
            PropertyGrid.AddSimpleAction<ToolBar>(
                "Reset Background",
                TestGenericToolBarResetBackground);
            PropertyGrid.AddSimpleAction<ToolBar>("Clear", TestGenericToolBarClear);
            PropertyGrid.AddSimpleAction<ToolBar>("Add OK button", TestGenericToolBarAddOk);
            PropertyGrid.AddSimpleAction<ToolBar>("Add Cancel button", TestGenericToolBarAddCancel);
            PropertyGrid.AddSimpleAction<ToolBar>("ReInit", TestGenericToolBarReInit);
        }

        void TestGenericToolBarFont()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Font = AbstractControl.DefaultFont.Scaled(2);
        }

        void TestGenericToolBarForegroundColor()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.ForegroundColor = Color.Red;
        }

        void TestGenericToolBarBackgroundColor()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Background = null;
            control.BackgroundColor = Color.DarkOliveGreen;
        }

        void TestGenericToolBarBackground()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Background = Color.RebeccaPurple.AsBrush;
            control.BackgroundColor = null;
        }

        void TestGenericToolBarClear()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.DeleteAll();
        }

        void TestGenericToolBarAddOk()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.AddSpeedBtn(KnownButton.OK);
        }

        void TestGenericToolBarAddCancel()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.AddSpeedBtn(KnownButton.Cancel);
        }

        void TestGenericToolBarReInit()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.DeleteAll(false);
            ObjectInit.InitGenericToolBar(control);
        }


        void TestGenericToolBarResetBackground()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            control.Background = null;
            control.BackgroundColor = null;
        }

        void TestGenericToolBarVisible()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var value = control.GetToolVisible(childId);
            value = !value;
            control.SetToolVisible(childId, value);
        }

        void TestGenericToolBarEnabled()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var enabled = control.GetToolEnabled(childId);
            enabled = !enabled;
            control.SetToolEnabled(childId, enabled);
        }

        void TestGenericToolBarSticky()
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;
            var childId = control.GetToolId(1);
            var value = control.GetToolSticky(childId);
            value = !value;
            control.SetToolSticky(childId, value);
        }

        void TestGenericToolBarDelete(bool dispose = false)
        {
            var control = GetSelectedControl<ToolBar>();
            if (control is null)
                return;

            var countBefore = control.ChildCount;

            var childId = control.GetToolId(0);
            var child = control.GetToolControl(childId);

            if (child is not null && dispose)
            {
                child.Disposed += (s, e) =>
                {
                    App.Log($"ToolBar item '{control.GetType()}' disposed");
                };
            }

            control.DeleteTool(childId, dispose);

            App.Log($"ToolBar.Delete 1st item. Count before: {countBefore} after {control.ChildCount}");
        }
    }
}