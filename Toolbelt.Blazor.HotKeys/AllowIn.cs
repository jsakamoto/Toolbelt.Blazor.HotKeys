using System;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// The flags of HTML element that will be allowed hotkey works.
    /// </summary>
    [Flags]
    public enum AllowIn
    {
        /// <summary>
        /// Default. The hot key feature will not be work when the INPUT or TEXTAREA element has focus.
        /// </summary>
        None = 0b0000,

        /// <summary>
        /// The hot key feature also works when the INPUT element has focus.
        /// </summary>
        Input = 0b0001,

        /// <summary>
        /// The hot key feature also works when the TEXTAREA element has focus.
        /// </summary>
        TextArea = 0b0010
    }
}
