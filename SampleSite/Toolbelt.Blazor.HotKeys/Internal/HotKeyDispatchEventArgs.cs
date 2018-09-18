using System;

namespace Toolbelt.Blazor.HotKeys.Internal
{
    public class HotKeyDispatchEventArgs : EventArgs
    {
        public ModKeys ModKeys { get; }

        public Keys Key { get; }

        public string SrcElementTagName { get; }

        public bool PreventDefault { get; set; }

        public HotKeyDispatchEventArgs(ModKeys modKeys, Keys keyCode, string srcElementTagName)
        {
            ModKeys = modKeys;
            Key = keyCode;
            SrcElementTagName = srcElementTagName;
        }
    }
}
