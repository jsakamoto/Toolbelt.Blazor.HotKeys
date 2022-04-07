using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

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

        private readonly ILogger<HotKeys> _Logger;

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
        internal HotKeys(IJSRuntime jSRuntime, HotKeysOptions options, ILogger<HotKeys> logger)
        {
            this._JSRuntime = jSRuntime;
            this._Options = options;
            this._Logger = logger;
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

                await this.EnsureJSInvokerAsync();
                await this._JSInvoker.InvokeAsync<object>("Toolbelt.Blazor.HotKeys.attach", DotNetObjectReference.Create(this), this.IsWasm);

                this._Attached = true;
                return this._JSInvoker;
            }
            finally { this.Syncer.Release(); }
        }

        private string GetVersionText()
        {
            var assembly = this.GetType().Assembly;
            return assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? assembly.GetName().Version.ToString();
        }

#if ENABLE_JSMODULE
        private async ValueTask EnsureJSInvokerAsync()
        {
            if (!this._Options.DisableClientScriptAutoInjection)
            {
                var scriptPath = "./_content/Toolbelt.Blazor.HotKeys/script.module.min.js";

                // Add version string for refresh token only navigator is online.
                // (If the app runs on the offline mode, the module url with query parameters might cause the "resource not found" error.)
                const string moduleScript = "export function isOnLine(){ return navigator.onLine; }";
                await using var inlineJsModule = await this._JSRuntime.InvokeAsync<IJSObjectReference>("import", "data:text/javascript;charset=utf-8," + Uri.EscapeDataString(moduleScript));
                var isOnLine = await inlineJsModule.InvokeAsync<bool>("isOnLine");

                if (isOnLine) scriptPath += $"?v={this.GetVersionText()}";

                var jsModule = await this._JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
                this._JSInvoker = new JSInvoker(this._JSRuntime, jsModule);
            }
            else
            {
                this._JSInvoker = new JSInvoker(this._JSRuntime, null);
                try
                {
                    const string moduleScript = "export function ready(){ return Toolbelt.Blazor.HotKeys.ready; }";
                    await using var inlineJsModule = await this._JSRuntime.InvokeAsync<IJSObjectReference>("import", "data:text/javascript;charset=utf-8," + Uri.EscapeDataString(moduleScript));

                    await inlineJsModule.InvokeVoidAsync("ready");
                }
                catch { }
            }
        }
#else
        private async ValueTask EnsureJSInvokerAsync()
        {
            this._JSInvoker = new JSInvoker(this._JSRuntime);
            if (!this._Options.DisableClientScriptAutoInjection)
            {
                var scriptPath = "./_content/Toolbelt.Blazor.HotKeys/script.min.js";

                // Add version string for refresh token only navigator is online.
                // (If the app runs on the offline mode, the module url with query parameters might cause the "resource not found" error.)
                var jsInProcRuntime = this._JSRuntime as IJSInProcessRuntime;
                var isOnLine = jsInProcRuntime?.Invoke<bool>("eval", "navigator.onLine") ?? false;
                var versionQuery = isOnLine ? $"?v={this.GetVersionText()}" : "";

                await this._JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s,v)=>(h=>h.querySelector(t+`[src^=\"${s}\"]`)?r():(e=>(e.src=(s+v),e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "','" + versionQuery + "'))");
            }
            try { await this._JSRuntime.InvokeVoidAsync("eval", "Toolbelt.Blazor.HotKeys.ready"); } catch { }
        }
#endif

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
            try { if (this._JSInvoker != null) await this._JSInvoker.DisposeAsync(); }
            catch (Exception ex) when (ex.GetType().FullName == "Microsoft.JSInterop.JSDisconnectedException") { }
            catch (Exception ex) { this._Logger.LogError(ex, ex.Message); }
        }
    }
}
