using System;
using System.ComponentModel;
using static System.ComponentModel.EditorBrowsableState;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// The flags of HTML element that will be allowed hotkey works.
    /// </summary>
    [Flags]
    [Obsolete("Use \"Exclude\" instead."), EditorBrowsable(Never)]
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
        TextArea = 0b0010,

        /// <summary>
        /// The hot key feature also works when a non-text INPUT element, such as a checkbox or toggle, has focus.
        /// </summary>
        NonTextInput = 0b0100,
    }
}
