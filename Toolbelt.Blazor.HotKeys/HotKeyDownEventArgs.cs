using System;
using System.ComponentModel;

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
        /// Get the value of the "key" property in the HTML Keyboard event object, that taking into consideration the state of modifier keys such as Shift as well as the keyboard locale and layout.<br/>
        /// (https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/key/Key_Values)
        /// </summary>
        public string NativeKey { get; }

        /// <summary>
        /// Get the value of the "code" property in the HTML Keyboard event object, taht represents a physical key on the keyboard.<br/>
        /// (https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/code/code_values)
        /// </summary>
        public string NativeCode { get; }

        /// <summary>
        /// Get the tag name of HTML element that is source of the DOM event.
        /// </summary>
        public string SrcElementTagName { get; }

        /// <summary>
        /// Get the type attribute, if any, of the HTML element that is source of the DOM event.
        /// </summary>
        public string SrcElementTypeAttribute { get; }

        /// <summary>
        /// Get the runtime environment is on a WebAssembly (Web browser), or not.
        /// </summary>
        public bool IsWasm { get; }

        private bool _PreventDefault;

        /// <summary>
        /// Get or set the flag to determine prevent default behavior or not.<br/>
        /// [IMPORTANT NOTICE] When set this property to true on a Blazor Server App (not a WebAssembly app), <see cref="InvalidOperationException"/> will be thrown. <br/>
        /// ("PreventDefault" behavior is not supported on Blazor Server App.)
        /// </summary>
        public bool PreventDefault
        {
            get => _PreventDefault;
            set { _PreventDefault = value; if (value == true && IsWasm == false) throw new InvalidOperationException("Setting the PreventDefault to true doesn't work on Blazor Server."); }
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyDownEventArgs class.
        /// </summary>
        [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
        public HotKeyDownEventArgs(ModKeys modKeys, Keys keyCode, string srcElementTagName, string srcElementTypeAttribute)
            : this(modKeys, keyCode, srcElementTagName, srcElementTypeAttribute, isWasm: false, nativeCode: "", nativeKey: "")
        {
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyDownEventArgs class.
        /// </summary>
        [Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
        public HotKeyDownEventArgs(ModKeys modKeys, Keys keyCode, string srcElementTagName, string srcElementTypeAttribute, bool isWasm)
            : this(modKeys, keyCode, srcElementTagName, srcElementTypeAttribute, isWasm, nativeCode: "", nativeKey: "")
        {
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyDownEventArgs class.
        /// </summary>
        public HotKeyDownEventArgs(ModKeys modKeys, Keys keyCode, string srcElementTagName, string srcElementTypeAttribute, bool isWasm, string nativeKey, string nativeCode)
        {
            ModKeys = modKeys;
            Key = keyCode;
            SrcElementTagName = srcElementTagName;
            SrcElementTypeAttribute = srcElementTypeAttribute;
            IsWasm = isWasm;
            NativeKey = nativeKey ?? "";
            NativeCode = nativeCode ?? "";
        }
    }
}
