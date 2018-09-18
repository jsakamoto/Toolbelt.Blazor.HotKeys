using System;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys.Internal
{
    public static class HotKeyDispatcher
    {
        internal static event EventHandler<HotKeyDispatchEventArgs> KeyDown;

        [JSInvokable(nameof(OnKeyDown))]
        public static bool OnKeyDown(ModKeys modKeys, Keys keyCode, string srcElementTagName)
        {
            Console.WriteLine($"[ONKEYDOWN] modKeys:{modKeys}, keyCode:{keyCode} (0x{(int)keyCode:X2}), tagName:{srcElementTagName}");
            var args = new HotKeyDispatchEventArgs(modKeys, keyCode, srcElementTagName);
            KeyDown?.Invoke(null, args);
            return args.PreventDefault;
        }
    }
}
