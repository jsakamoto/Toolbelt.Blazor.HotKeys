using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// A service that provides Hotkey feature.
    /// </summary>
    public class HotKeys
    {
        private volatile bool _Attached = false;

        private readonly IJSRuntime JSRuntime;

        private readonly SemaphoreSlim Syncer = new(1, 1);

        private readonly bool IsWasm = RuntimeInformation.OSDescription == "web" || RuntimeInformation.OSDescription == "Browser";

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
        private async Task Attach()
        {
            if (_Attached) return;
            await Syncer.WaitAsync();
            try
            {
                if (_Attached) return;
                const string scriptPath = "_content/Toolbelt.Blazor.HotKeys/script.min.js";
                await JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s)=>(h=>h.querySelector(t+`[src=\"${{s}}\"]`)?r():(e=>(e.src=s,e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "'))");
                await JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.HotKeys.attach", DotNetObjectReference.Create(this), IsWasm);
                _Attached = true;
            }
            finally { Syncer.Release(); }
        }

        /// <summary>
        /// Create hotkey entries context, and activate it.
        /// </summary>
        /// <returns></returns>
        public HotKeysContext CreateContext()
        {
            var attachTask = Attach();
            return new HotKeysContext(this.JSRuntime, attachTask);
        }

        /// <summary>
        /// The method that will be invoked from JavaScript keydown event handler.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The identifier of hotkey.</param>
        /// <param name="srcElementTagName">The tag name of HTML element that is source of the DOM event.</param>
        /// <param name="srcElementTypeName">The <code>type</code>attribute, if any, of the HTML element that is source of the DOM event</param>
        /// <param name="nativeKey">The value of the "key" property in the DOM event object</param>
        /// <param name="nativeCode">The value of the "code" property in the DOM event object</param>
        /// <returns></returns>
        [JSInvokable(nameof(OnKeyDown)), EditorBrowsable(EditorBrowsableState.Never)]
        public bool OnKeyDown(ModKeys modKeys, string keyName, string srcElementTagName, string srcElementTypeName, string nativeKey, string nativeCode)
        {
            var keyCode = Enum.TryParse<Keys>(keyName, ignoreCase: true, out var k) ? k : (Keys)0;
            var args = new HotKeyDownEventArgs(modKeys, keyCode, srcElementTagName, srcElementTypeName, IsWasm, nativeKey, nativeCode);
            KeyDown?.Invoke(null, args);
            return args.PreventDefault;
        }
    }
}
