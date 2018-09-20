using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Toolbelt.Blazor.HotKeys
{
    public class HotKeysContext : IDisposable
    {
        private HotKeys _HotKeys;

        public List<HotKeyEntry> Keys { get; } = new List<HotKeyEntry>();

        internal HotKeysContext(HotKeys hotKeys)
        {
            hotKeys.KeyDown += HotKeys_KeyDown;
            _HotKeys = hotKeys;
        }

        private async void HotKeys_KeyDown(object sender, HotKeyDownEventArgs e)
        {
            foreach (var entry in this.Keys)
            {
                if (entry.ModKeys != e.ModKeys) continue;
                if (entry.Key != e.Key) continue;
                if (e.SrcElementTagName == "INPUT" && (entry.AllowIn & AllowIn.Input) != AllowIn.Input) continue;
                if (e.SrcElementTagName == "TEXTAREA" && (entry.AllowIn & AllowIn.TextArea) != AllowIn.TextArea) continue;

                e.PreventDefault = true;

                await entry.Action?.Invoke(entry);
            }
        }

        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<HotKeyEntry, Task> action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<Task> action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        public HotKeysContext Add(ModKeys modKeys, Keys key, Action<HotKeyEntry> action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        public HotKeysContext Add(ModKeys modKeys, Keys key, Action action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        public void Dispose()
        {
            _HotKeys.KeyDown -= HotKeys_KeyDown;
        }
    }
}