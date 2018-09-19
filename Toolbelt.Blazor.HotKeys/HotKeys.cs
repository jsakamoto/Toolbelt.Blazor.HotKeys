using System;
using System.ComponentModel;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys
{
    public class HotKeys
    {
        private bool _Attached = false;

        public event EventHandler<HotKeyDownEventArgs> KeyDown;

        public HotKeys()
        {
        }

        private void Attach()
        {
            if (_Attached) return;
            JSRuntime.Current.InvokeAsync<object>("Toolbelt.Blazor.HotKeys.attach", new DotNetObjectRef(this));
            _Attached = true;
        }

        public HotKeysContext CreateContext()
        {
            Attach();
            return new HotKeysContext(this);
        }

        [JSInvokable(nameof(OnKeyDown)), EditorBrowsable(EditorBrowsableState.Never)]
        public bool OnKeyDown(ModKeys modKeys, Keys keyCode, string srcElementTagName)
        {
            var args = new HotKeyDownEventArgs(modKeys, keyCode, srcElementTagName);
            KeyDown?.Invoke(null, args);
            return args.PreventDefault;
        }
    }
}
