using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// A service that provides Hotkey feature.
    /// </summary>
    public class HotKeys : IAsyncDisposable
    {
        private volatile bool _Attached = false;

        private readonly IJSRuntime _JSRuntime;

        private readonly HotKeysOptions _Options;

        private JSInvoker _JSInvoker = null;

        private readonly SemaphoreSlim Syncer = new(1, 1);

        private readonly bool IsWasm = RuntimeInformation.OSDescription == "web" || RuntimeInformation.OSDescription == "Browser";

        /// <summary>
        /// Occurs when the user enter any keys on the browser.
        /// </summary>
        public event EventHandler<HotKeyDownEventArgs> KeyDown;

        /// <summary>
        /// Initialize a new instance of the HotKeys class.
        /// </summary>
        internal HotKeys(IJSRuntime jSRuntime, HotKeysOptions options)
        {
            this._JSRuntime = jSRuntime;
            this._Options = options;
        }

        /// <summary>
        /// Attach this HotKeys service instance to JavaScript DOM event handler.
        /// </summary>
        private async Task<JSInvoker> Attach()
        {
            if (this._Attached) return this._JSInvoker;
            await this.Syncer.WaitAsync();
            try
            {
                if (this._Attached) return this._JSInvoker;

                var assembly = this.GetType().Assembly;
                var version = assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion ?? assembly.GetName().Version.ToString();
#if ENABLE_JSMODULE
                if (!this._Options.DisableClientScriptAutoInjection)
                {
                    var scriptPath = $"./_content/Toolbelt.Blazor.HotKeys/script.module.min.js?v={version}";
                    var jsModule = await this._JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
                    this._JSInvoker = new JSInvoker(this._JSRuntime, jsModule);
                }
                else
                {
                    this._JSInvoker = new JSInvoker(this._JSRuntime, null);
                    try { await this._JSRuntime.InvokeVoidAsync("eval", "Toolbelt.Blazor.HotKeys.ready"); } catch { }
                }
#else
                this._JSInvoker = new JSInvoker(this._JSRuntime);
                if (!this._Options.DisableClientScriptAutoInjection)
                {
                    var scriptPath = "./_content/Toolbelt.Blazor.HotKeys/script.min.js";
                    await this._JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s,v)=>(h=>h.querySelector(t+`[src^=\"${s}\"]`)?r():(e=>(e.src=(s+v),e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "','?v=" + version + "'))");
                }
                try { await this._JSRuntime.InvokeVoidAsync("eval", "Toolbelt.Blazor.HotKeys.ready"); } catch { }
#endif
                await this._JSInvoker.InvokeAsync<object>("Toolbelt.Blazor.HotKeys.attach", DotNetObjectReference.Create(this), this.IsWasm);

                this._Attached = true;
                return this._JSInvoker;
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
            if (this._JSInvoker != null) await this._JSInvoker.DisposeAsync();
        }
    }
}
