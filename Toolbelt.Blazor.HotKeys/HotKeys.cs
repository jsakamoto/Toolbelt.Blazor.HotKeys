using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

#if ENABLE_JSMODULE
using IJSInterface = Microsoft.JSInterop.IJSObjectReference;
#else
using IJSInterface = Microsoft.JSInterop.IJSRuntime;
#endif

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// A service that provides Hotkey feature.
    /// </summary>
    public class HotKeys : IAsyncDisposable
    {
        private volatile bool _Attached = false;

        private readonly IJSRuntime _JSRuntime;

        private IJSInterface _JSModule = null;

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
            this._JSRuntime = jSRuntime;
        }

        /// <summary>
        /// Attach this HotKeys service instance to JavaScript DOM event handler.
        /// </summary>
        private async Task<IJSInterface> Attach()
        {
            if (this._Attached) return this._JSModule;
            await this.Syncer.WaitAsync();
            try
            {
                if (this._Attached) return this._JSModule;

                var version = this.GetType().Assembly.GetName().Version;
                var scriptPath = $"./_content/Toolbelt.Blazor.HotKeys/script.min.js?v={version}";
#if ENABLE_JSMODULE
                this._JSModule = await this._JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
#else
                this._JSModule = this._JSRuntime;
                await this._JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s)=>(h=>h.querySelector(t+`[src=\"${{s}}\"]`)?r():(e=>(e.src=s,e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "'))");
#endif
                await this._JSModule.InvokeVoidAsync("Toolbelt.Blazor.HotKeys.attach", DotNetObjectReference.Create(this), this.IsWasm);

                this._Attached = true;
                return this._JSModule;
            }
            finally { this.Syncer.Release(); }
        }

        /// <summary>
        /// Create hotkey entries context, and activate it.
        /// </summary>
        /// <returns></returns>
        public HotKeysContext CreateContext()
        {
            var attachTask = this.Attach();
            return new HotKeysContext(attachTask);
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
            var args = new HotKeyDownEventArgs(modKeys, keyCode, srcElementTagName, srcElementTypeName, this.IsWasm, nativeKey, nativeCode);
            KeyDown?.Invoke(null, args);
            return args.PreventDefault;
        }

        public async ValueTask DisposeAsync()
        {
#if ENABLE_JSMODULE
            if (this._JSModule != null) await this._JSModule.DisposeAsync();
#else
            await Task.CompletedTask;
#endif
        }
    }
}
