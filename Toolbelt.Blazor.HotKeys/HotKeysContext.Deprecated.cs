using System;
using System.ComponentModel;
using System.Threading.Tasks;
using static System.ComponentModel.EditorBrowsableState;

namespace Toolbelt.Blazor.HotKeys
{
    public partial class HotKeysContext
    {
        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<HotKeyEntry, Task> action, AllowIn allowIn) => this.Add(modKeys, key, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<HotKeyEntry, Task> action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
            return this;
        }

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<Task> action, AllowIn allowIn) => this.Add(modKeys, key, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Func<Task> action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
            return this;
        }

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Action<HotKeyEntry> action, AllowIn allowIn) => this.Add(modKeys, key, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Action<HotKeyEntry> action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
            return this;
        }

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Action action, AllowIn allowIn) => this.Add(modKeys, key, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use the other overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="key">The identifier of hotkey.</param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, Keys key, Action action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, key, allowIn, description, action)));
            return this;
        }

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Func<HotKeyEntry, Task> action, AllowIn allowIn) => this.Add(modKeys, keyName, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Func<HotKeyEntry, Task> action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, keyName, allowIn, description, action)));
            return this;
        }

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Func<Task> action, AllowIn allowIn) => this.Add(modKeys, keyName, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Func<Task> action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, keyName, allowIn, description, action)));
            return this;
        }

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Action<HotKeyEntry> action, AllowIn allowIn) => this.Add(modKeys, keyName, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Action<HotKeyEntry> action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, keyName, allowIn, description, action)));
            return this;
        }

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Action action, AllowIn allowIn) => this.Add(modKeys, keyName, action, "", allowIn);

        /// <summary>
        /// Add a new hotkey entry to this context.<br/>
        /// if the key that you want to hook is not covered by the Keys enum values, use this overload version that accepts key name as a string.
        /// </summary>
        /// <param name="modKeys">The combination of modifier keys flags.</param>
        /// <param name="keyName">The name of the identifier of hotkey.<para>The "key name" is a bit different from the "key" and "code" properties of the DOM event object.<br/> The "key name" comes from "key" and "code", but it is tried to converting to one of the Keys enum values names.<br/>if the keyboard event is not covered by Keys enum values, the "key name" will be the value of "code" or "key".</para></param>
        /// <param name="action">The callback action that will be invoked when user enter modKeys + key combination on the browser.</param>
        /// <param name="description">The description of the meaning of this hot key entry.</param>
        /// <param name="allowIn">The combination of HTML element flags that will be allowed hotkey works.</param>
        /// <returns>This context.</returns>
        [Obsolete("Use the other overload version that has an \"Exclude exclude\" argument isntead."), EditorBrowsable(Never)]
        public HotKeysContext Add(ModKeys modKeys, string keyName, Action action, string description, AllowIn allowIn)
        {
            this.Keys.Add(this.Register(new HotKeyEntry(modKeys, keyName, allowIn, description, action)));
            return this;
        }
    }
}
