using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// Current active hotkeys set.
    /// </summary>
    public class HotKeysContext : IDisposable
    {
        /// <summary>
        /// The collection of Hotkey entries.
        /// </summary>
        public List<HotKeyEntry> Keys { get; } = new List<HotKeyEntry>();

        private readonly IJSRuntime JSRuntime;

        private readonly Task AttachTask;

        /// <summary>
        /// Initialize a new instance of the HotKeysContext class.
        /// </summary>
        /// <param name="jSRuntime"></param>
        /// <param name="attachTask"></param>
        internal HotKeysContext(IJSRuntime jSRuntime, Task attachTask)
        {
            this.JSRuntime = jSRuntime;
            this.AttachTask = attachTask;
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
            this.Keys.Add(Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
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
            this.Keys.Add(Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
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
            this.Keys.Add(Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
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
            this.Keys.Add(Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
            return this;
        }

        private HotKeyEntry Register(HotKeyEntry hotKeyEntry)
        {
            hotKeyEntry.ObjectReference = DotNetObjectReference.Create(hotKeyEntry);
            this.AttachTask.ContinueWith(t =>
            {
                return this.JSRuntime.InvokeAsync<int>(
                    "Toolbelt.Blazor.HotKeys.register",
                    hotKeyEntry.ObjectReference, hotKeyEntry.ModKeys, hotKeyEntry.Key, hotKeyEntry.AllowIn).AsTask();
            })
            .Unwrap()
            .ContinueWith(t =>
            {
                if (!t.IsCanceled && !t.IsFaulted) { hotKeyEntry.Id = t.Result; }
            });
            return hotKeyEntry;
        }

        private void Unregister(HotKeyEntry hotKeyEntry)
        {
            if (hotKeyEntry.Id == -1) return;

            this.JSRuntime.InvokeVoidAsync("Toolbelt.Blazor.HotKeys.unregister", hotKeyEntry.Id)
                .AsTask()
                .ContinueWith(t =>
                {
                    hotKeyEntry.Id = -1;
                    hotKeyEntry.ObjectReference?.Dispose();
                    hotKeyEntry.ObjectReference = null;
                });
        }

        /// <summary>
        /// Deactivate the hot key entry contained in this context.
        /// </summary>
        public void Dispose()
        {
            foreach (var entry in this.Keys)
            {
                this.Unregister(entry);
            }
            this.Keys.Clear();
        }
    }
}