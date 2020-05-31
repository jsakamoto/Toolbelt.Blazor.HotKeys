using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// Current active hotkeys set.
    /// </summary>
    public class HotKeysContext : IDisposable
    {
        private HotKeys _HotKeys;

        /// <summary>
        /// The collection of Hotkey entries.
        /// </summary>
        public List<HotKeyEntry> Keys { get; } = new List<HotKeyEntry>();

        /// <summary>
        /// Initialize a new instance of the HotKeysContext class.
        /// </summary>
        /// <param name="hotKeys">HotKeys service</param>
        internal HotKeysContext(HotKeys hotKeys)
        {
            hotKeys.KeyDown += HotKeys_KeyDown;
            _HotKeys = hotKeys;
        }

        /// <summary>
        /// The method that will be invoked when KeyDown event of HotKeys service occurred.
        /// </summary>
        private async void HotKeys_KeyDown(object sender, HotKeyDownEventArgs e)
        {
            foreach (var entry in this.Keys)
            {
                if (entry.Key != e.Key) continue;

                var modKeys = entry.ModKeys;
                if (entry.Key == Blazor.HotKeys.Keys.Shift) modKeys |= ModKeys.Shift;
                if (entry.Key == Blazor.HotKeys.Keys.Ctrl) modKeys |= ModKeys.Ctrl;
                if (entry.Key == Blazor.HotKeys.Keys.Alt) modKeys |= ModKeys.Alt;
                if (modKeys != e.ModKeys) continue;

                if (e.SrcElementTagName == "INPUT" && (entry.AllowIn & AllowIn.Input) != AllowIn.Input) continue;
                if (e.SrcElementTagName == "TEXTAREA" && (entry.AllowIn & AllowIn.TextArea) != AllowIn.TextArea) continue;

                e.PreventDefault = true;

                await entry.Action?.Invoke(entry);
            }
        }

        /// <summary>
        /// Add new hotkey entry to this context.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<HotKeyEntry, Task> action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        /// <summary>
        /// Add new hotkey entry to this context.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<Task> action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        /// <summary>
        /// Add new hotkey entry to this context.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        public HotKeysContext Add(ModKeys modKeys, Keys key, Action<HotKeyEntry> action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        /// <summary>
        /// Add new hotkey entry to this context.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        public HotKeysContext Add(ModKeys modKeys, Keys key, Action action, string description = "", AllowIn allowIn = AllowIn.None)
        {
            this.Keys.Add(new HotKeyEntry(modKeys, key, allowIn, description, action));
            return this;
        }

        /// <summary>
        /// Deactivate the hot key entry contained in this context.
        /// </summary>
        public void Dispose()
        {
            _HotKeys.KeyDown -= HotKeys_KeyDown;
        }
    }
}