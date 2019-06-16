using System;
using System.ComponentModel;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// A service that provides Hotkey feature.
    /// </summary>
    public class HotKeys
    {
        private bool _Attached = false;

        private readonly IJSRuntime JSRuntime;

        /// <summary>
        /// Occurs when the user enter any keys on the browser.
        /// </summary>
        public event EventHandler<HotKeyDownEventArgs> KeyDown;

        /// <summary>
        /// Initialize a new instance of the HotKeys class.
        /// </summary>
        internal HotKeys(IJSRuntime jSRuntime)
        {
            this.JSRuntime = jSRuntime;
        }

        /// <summary>
        /// Attach this HotKeys service instance to JavaScript DOM event handler.
        /// </summary>
        private void Attach()
        {
            if (_Attached) return;
            JSRuntime.InvokeAsync<object>("Toolbelt.Blazor.HotKeys.attach", DotNetObjectRef.Create(this));
            _Attached = true;
        }

        /// <summary>
        /// Create hotkey entries context, and activate it.
        /// </summary>
        /// <returns></returns>
        public HotKeysContext CreateContext()
        {
            Attach();
            return new HotKeysContext(this);
        }

        /// <summary>
        /// The method that will be invoked from JavaScript keydown event handler.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyCode">The identifier of hotkey.</param>
        /// <param name="srcElementTagName">The tag name of HTML element that is source of the DOM event.</param>
        /// <returns></returns>
        [JSInvokable(nameof(OnKeyDown)), EditorBrowsable(EditorBrowsableState.Never)]
        public bool OnKeyDown(ModKeys modKeys, Keys keyCode, string srcElementTagName)
        {
            var args = new HotKeyDownEventArgs(modKeys, keyCode, srcElementTagName);
            KeyDown?.Invoke(null, args);
            return args.PreventDefault;
        }
    }
}
