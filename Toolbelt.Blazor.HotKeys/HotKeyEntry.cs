#nullable enable
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// Association of key combination and callback action.
    /// </summary>
    public class HotKeyEntry
    {
        /// <summary>
        /// Get the combination of modifier keys flags.
        /// </summary>
        public ModKeys ModKeys { get; }

        /// <summary>
        /// Get the identifier of hotkey.
        /// </summary>
        public Keys Key { get; }

        /// <summary>
        /// Get the name if the identifier of hotkey.
        /// <para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para>
        /// </summary>
        public string KeyName { get; }

        /// <summary>
        /// Get the description of the meaning of this hot key entry.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Get the combination of HTML element flags that will be allowed hotkey works.
        /// </summary>
        public AllowIn AllowIn { get; }

        /// <summary>
        /// Get the callback action that will be invoked when user enter modKeys + key combination on the browser.
        /// </summary>
        public Func<HotKeyEntry, Task> Action { get; }

        internal int Id = -1;

        internal DotNetObjectReference<HotKeyEntry>? ObjectReference;

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Func<HotKeyEntry, Task> action)
        {
            ModKeys = modKeys;
            Key = key;
            KeyName = key.ToString();
            AllowIn = allowIn;
            Description = description;
            Action = action;
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Func<Task> action)
            : this(modKeys, key, allowIn, description, _ => action())
        {
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Action<HotKeyEntry> action)
            : this(modKeys, key, allowIn, description, e => { action(e); return Task.CompletedTask; })
        {
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Action action)
            : this(modKeys, key, allowIn, description, _ => { action(); return Task.CompletedTask; })
        {
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, string keyName, AllowIn allowIn, string description, Func<HotKeyEntry, Task> action)
        {
            ModKeys = modKeys;
            Key = Enum.TryParse<Keys>(keyName, ignoreCase: true, out var v) ? v : (Keys)0;
            KeyName = keyName;
            AllowIn = allowIn;
            Description = description;
            Action = action;
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, string keyName, AllowIn allowIn, string description, Func<Task> action)
            : this(modKeys, keyName, allowIn, description, _ => action())
        {
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, string keyName, AllowIn allowIn, string description, Action<HotKeyEntry> action)
            : this(modKeys, keyName, allowIn, description, e => { action(e); return Task.CompletedTask; })
        {
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        public HotKeyEntry(ModKeys modKeys, string keyName, AllowIn allowIn, string description, Action action)
            : this(modKeys, keyName, allowIn, description, _ => { action(); return Task.CompletedTask; })
        {
        }

        [JSInvokable(nameof(InvokeAction)), EditorBrowsable(EditorBrowsableState.Never)]
        public void InvokeAction()
        {
            this.Action?.Invoke(this);
        }

        /// <summary>
        /// Returns a String that combined key combination and description of this entry, like "Ctrl+A: Select All."
        /// </summary>
        public override string ToString()
        {
            return this.ToString("{0}: {1}");
        }

        /// <summary>
        /// Returns a String formatted with specified format string.
        /// </summary>
        /// <param name="format">{0} will be replaced with key combination text, and {1} will be replaced with description of this hotkey entry object.</param>
        public string ToString(string format)
        {
            var keyComboText =
                (this.ModKeys == ModKeys.None ? "" : this.ModKeys.ToString().Replace(", ", "+") + "+") +
                (this.Key.ToKeyString() ?? this.KeyName);
            return string.Format(format, keyComboText, this.Description);
        }
    }
}
