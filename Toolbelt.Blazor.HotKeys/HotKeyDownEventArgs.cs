using System;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// Provides data for the KeyDown eventsof HotKeys service.
    /// </summary>
    public class HotKeyDownEventArgs : EventArgs
    {
        /// <summary>
        /// Get the combination of modifier keys flags.
        /// </summary>
        public ModKeys ModKeys { get; }

        /// <summary>
        /// Get the identifier of the key that is pressed.
        /// </summary>
        public Keys Key { get; }

        /// <summary>
        /// Get the tag name of HTML element that is source of the DOM event.
        /// </summary>
        public string SrcElementTagName { get; }
        
        /// <summary>
        /// Get the type attribute, if any, of the HTML element that is source of the DOM event.
        /// </summary>
        public string SrcElementTypeAttribute { get; }

        /// <summary>
        /// Get or set the flag to determine prevent default behavior or not.
        /// </summary>
        public bool PreventDefault { get; set; }

        /// <summary>
        /// Initialize a new instance of the HotKeyDownEventArgs class.
        /// </summary>
        public HotKeyDownEventArgs(ModKeys modKeys, Keys keyCode, string srcElementTagName, string srcElementTypeAttribute)
        {
            ModKeys = modKeys;
            Key = keyCode;
            SrcElementTagName = srcElementTagName;
            SrcElementTypeAttribute = srcElementTypeAttribute;
        }
    }
}
