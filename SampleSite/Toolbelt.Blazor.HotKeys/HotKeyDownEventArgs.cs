using System;

namespace Toolbelt.Blazor.HotKeys
{
    public class HotKeyDownEventArgs : EventArgs
    {
        public ModKeys ModKeys { get; }

        public Keys Key { get; }

        public string SrcElementTagName { get; }

        public bool PreventDefault { get; set; }

        public HotKeyDownEventArgs(ModKeys modKeys, Keys keyCode, string srcElementTagName)
        {
            ModKeys = modKeys;
            Key = keyCode;
            SrcElementTagName = srcElementTagName;
        }
    }
}
