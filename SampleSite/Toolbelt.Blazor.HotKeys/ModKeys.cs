using System;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// Constants of modifire keys.
    /// </summary>
    [Flags]
    public enum ModKeys
    {
        None = 0,
        Shift = 0x01,
        Ctrl = 0x02,
        Alt = 0x04
    }
}
