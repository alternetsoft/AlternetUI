using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.Diagnostics;

namespace PropertyGridSample
{
    public partial class MainWindow
    {
        private void InitToolBox()
        {
            void Fn()
            {

                ControlListBoxItem item = new(typeof(WelcomeControl))
                {
                    Text = "Welcome Page",
                };
                panel.LeftTreeView.Add(item);

                Type[] badTypes = new Type[]
                {
              typeof(ContextMenu), // added using other style
              typeof(NonVisualControl), // has no sense to add
              typeof(StatusBarPanel), // part of other control
              typeof(MenuItem), // part of other control
              typeof(TabPage), // part of other control
              typeof(ToolbarItem), // part of other control

              typeof(Toolbar), // can create some modal window? or add child window 
              typeof(WebBrowser),
              typeof(Popup), // button with popup like ContextMenu
              typeof(AuiNotebook),
              typeof(AuiToolbar),
              typeof(UIDialogWindow),
              typeof(SplitterPanel),// know how
              typeof(Grid),// know how
              typeof(ScrollViewer),
              typeof(LayoutPanel),// know how
              typeof(UserPaintControl),// know how
              typeof(MainMenu),// can create some modal window? or add child window 
              typeof(StatusBar),// can create some modal window? or add child window 
              typeof(PropertyGrid),
              typeof(TabControl), // pages are not shown. Why?
              typeof(Window),
                };

                IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));
                foreach (Type type in result)
                {
                    if (Array.IndexOf(badTypes, type) >= 0)
                        continue;
                    if (type.Assembly != typeof(Control).Assembly)
                        continue;
                    item = new(type);
                    panel.LeftTreeView.Add(item);
                }

                item = new(typeof(SettingsControl))
                {
                    PropInstance = PropertyGridSettings.Default,
                    EventInstance = new object(),
                    Text = "Demo Options",
                };
                panel.LeftTreeView.Add(item);

                AddDialog<ColorDialog>();
                AddDialog<OpenFileDialog>();
                AddDialog<SaveFileDialog>();
                AddDialog<SelectDirectoryDialog>();
                AddDialog<FontDialog>();
                /*AddContextMenu<ContextMenu>();*/
            }

            panel.LeftTreeView.DoInsideUpdate(() => { Fn(); });
        }

        internal void AddMainWindow()
        {
            panel.LeftTreeView.Add(new ControlListBoxItem(typeof(Window), this.ParentWindow));
        }

        private void AddDialog<T>()
            where T : CommonDialog
        {
            var dialog = (T)ControlListBoxItem.CreateInstance(typeof(T))!;
            var button = new ShowDialogButton
            {
                Dialog = dialog,
            };
            var item = new ControlListBoxItem(typeof(T), button)
            {
                PropInstance = dialog,
            };
            panel.LeftTreeView.Add(item);
        }

        internal void AddContextMenu<T>()
            where T : ContextMenu
        {
            var menu = (T)ControlListBoxItem.CreateInstance(typeof(T))!;

            var button = new ShowContextMenuButton
            {
                Menu = menu,
            };
            var item = new ControlListBoxItem(typeof(T), button)
            {
                PropInstance = menu,
            };
            panel.LeftTreeView.Add(item);
        }

        private void ApplicationIdle(object? sender, EventArgs e)
        {
            if (updatePropertyGrid)
            {
                updatePropertyGrid = false;
                try
                {
                    UpdatePropertyGrid();
                }
                catch
                {
                }
            }
        }
    }
}