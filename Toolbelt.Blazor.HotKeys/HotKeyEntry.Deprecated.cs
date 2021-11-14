using System;
using System.ComponentModel;
using System.Threading.Tasks;
using static System.ComponentModel.EditorBrowsableState;

namespace Toolbelt.Blazor.HotKeys
{
    public partial class HotKeyEntry
    {
        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        [Obsolete("Use the constructor version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Func<HotKeyEntry, Task> action)
        {
            this.ModKeys = modKeys;
            this.Key = key;
            this.KeyName = key.ToString();
            this.Exclude = AllowInToExclude(allowIn);
            this.Description = description;
            this.Action = action;
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        [Obsolete("Use the constructor version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
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
        [Obsolete("Use the constructor version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
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
        [Obsolete("Use the constructor version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
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
        [Obsolete("Use the constructor version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeyEntry(ModKeys modKeys, string keyName, AllowIn allowIn, string description, Func<HotKeyEntry, Task> action)
        {
            this.ModKeys = modKeys;
            this.Key = Enum.TryParse<Keys>(keyName, ignoreCase: true, out var v) ? v : (Keys)0;
            this.KeyName = keyName;
            this.Exclude = AllowInToExclude(allowIn);
            this.Description = description;
            this.Action = action;
        }

        /// <summary>
        /// Initialize a new instance of the HotKeyEntry class.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        [Obsolete("Use the constructor version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeyEntry(ModKeys modKeys, string keyName, AllowIn allowIn, string description, Func<Task> action)
            : this(modKeys, keyName, allowIn, description, _ => action())
        {
        }
    }
}
