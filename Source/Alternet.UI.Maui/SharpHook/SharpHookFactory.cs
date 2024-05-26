using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using SharpHook;

namespace Alternet.UI.Maui
{
    internal class SharpHookFactory
    {
        private readonly TaskPoolGlobalHook? hook;

        public SharpHookFactory()
        {
            hook = new TaskPoolGlobalHook();

            hook.KeyTyped += Hook_KeyTyped;
            hook.KeyPressed += Hook_KeyPressed;
            hook.KeyReleased += Hook_KeyReleased;

            hook.MouseClicked += Hook_MouseClicked;
            hook.MousePressed += Hook_MousePressed;
            hook.MouseReleased += Hook_MouseReleased;
            hook.MouseMoved += Hook_MouseMoved;
            hook.MouseDragged += Hook_MouseDragged;
            hook.MouseWheel += Hook_MouseWheel;

            hook.RunAsync();
        }

        private void Hook_KeyTyped(object? sender, KeyboardHookEventArgs e)
        {
            /*char keyChar = e.Data.KeyChar;*/
        }

        private void Hook_KeyPressed(object? sender, KeyboardHookEventArgs e)
        {
            /*SharpHook.Native.KeyCode keyCode = e.Data.KeyCode;
            SharpHook.Native.ModifierMask mask = e.RawEvent.Mask;*/
        }

        private void Hook_KeyReleased(object? sender, KeyboardHookEventArgs e)
        {
        }

        private void Hook_MouseClicked(object? sender, MouseHookEventArgs e)
        {
        }

        private void Hook_MousePressed(object? sender, MouseHookEventArgs e)
        {
        }

        private void Hook_MouseReleased(object? sender, MouseHookEventArgs e)
        {
        }

        private void Hook_MouseMoved(object? sender, MouseHookEventArgs e)
        {
        }

        private void Hook_MouseDragged(object? sender, MouseHookEventArgs e)
        {
        }

        private void Hook_MouseWheel(object? sender, MouseWheelHookEventArgs e)
        {
        }
    }
}
