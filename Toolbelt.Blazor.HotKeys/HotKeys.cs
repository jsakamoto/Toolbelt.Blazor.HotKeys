using System;
using System.ComponentModel;
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

        private readonly SemaphoreSlim Syncer = new SemaphoreSlim(1, 1);

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
                await JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.HotKeys.attach", DotNetObjectReference.Create(this));
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
        /// <param name="keyCode">The identifier of hotkey.</param>
        /// <param name="srcElementTagName">The tag name of HTML element that is source of the DOM event.</param>
        /// <param name="srcElementTypeName">The <code>type</code>attribute, if any, of the HTML element that is source of the DOM event</param>
        /// <returns></returns>
        [JSInvokable(nameof(OnKeyDown)), EditorBrowsable(EditorBrowsableState.Never)]
        public bool OnKeyDown(ModKeys modKeys, Keys keyCode, string srcElementTagName, string srcElementTypeName)
        {
            var args = new HotKeyDownEventArgs(modKeys, keyCode, srcElementTagName, srcElementTypeName);
            KeyDown?.Invoke(null, args);
            return args.PreventDefault;
        }
    }
}
