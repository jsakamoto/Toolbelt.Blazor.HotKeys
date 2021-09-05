using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys
{
    internal class JSInvoker : IAsyncDisposable
    {
        public IJSRuntime _JS;

#if ENABLE_JSMODULE
        public IJSObjectReference _JSModule;

        public JSInvoker(IJSRuntime js, IJSObjectReference jsModule)
        {
            this._JS = js;
            this._JSModule = jsModule;
        }
#else
        public JSInvoker(IJSRuntime js)
        {
            this._JS = js;
        }
#endif
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object[] args)
        {
#if ENABLE_JSMODULE
            if (this._JSModule != null) return this._JSModule.InvokeAsync<TValue>(identifier, args);
#endif
            return this._JS.InvokeAsync<TValue>(identifier, args);
        }

        public async ValueTask DisposeAsync()
        {
#if ENABLE_JSMODULE
            if (this._JSModule != null)
            {
                await this._JSModule.DisposeAsync();
            }
#else
            await Task.CompletedTask;
#endif
        }
    }
}
