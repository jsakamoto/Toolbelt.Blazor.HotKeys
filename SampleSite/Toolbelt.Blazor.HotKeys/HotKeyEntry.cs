using System;
using System.Threading.Tasks;

namespace Toolbelt.Blazor.HotKeys
{
    public class HotKeyEntry
    {
        public ModKeys ModKeys { get; }

        public Keys Key { get; }

        public string Description { get; }

        public AllowIn AllowIn { get; }

        public Func<HotKeyEntry, Task> Action { get; }

        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Func<HotKeyEntry, Task> action)
        {
            ModKeys = modKeys;
            Key = key;
            AllowIn = allowIn;
            Description = description;
            Action = action;
        }

        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Func<Task> action)
            : this(modKeys, key, allowIn, description, _ => action())
        {
        }

        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Action<HotKeyEntry> action)
            : this(modKeys, key, allowIn, description, e => { action(e); return Task.CompletedTask; })
        {
        }

        public HotKeyEntry(ModKeys modKeys, Keys key, AllowIn allowIn, string description, Action action)
            : this(modKeys, key, allowIn, description, _ => { action(); return Task.CompletedTask; })
        {
        }

        public override string ToString()
        {
            return this.ToString("{0}: {1}");
        }

        public string ToString(string format)
        {
            var keyComboText =
                (this.ModKeys == ModKeys.None ? "" : this.ModKeys.ToString().Replace(", ", "+") + "+") +
                (this.Key.ToKeyString());
            return string.Format(format, keyComboText, this.Description);
        }
    }
}
