using System;
using System.Collections.Generic;
using Toolbelt.Blazor.HotKeys;

namespace SampleSite.Components.Shared
{
    public class EnteredKey : IEquatable<EnteredKey>
    {
        public readonly Guid Id = Guid.NewGuid();
        public readonly ModKeys ModKeys;
        public readonly string KeyName;
        public readonly string Key;
        public readonly string Code;
        public int RepeatCount = 1;

        public EnteredKey(ModKeys modKeys, string keyName, string key = "", string code = "")
        {
            ModKeys = modKeys;
            KeyName = keyName;
            Key = key;
            Code = code;
        }

        public override bool Equals(object obj) => Equals(obj as EnteredKey);

        public bool Equals(EnteredKey other)
        {
            return other != null &&
                   ModKeys == other.ModKeys &&
                   KeyName == other.KeyName &&
                   Key == other.Key &&
                   Code == other.Code;
        }

        public override int GetHashCode() => HashCode.Combine(ModKeys, KeyName, Key, Code);

        public static bool operator ==(EnteredKey left, EnteredKey right) => EqualityComparer<EnteredKey>.Default.Equals(left, right);

        public static bool operator !=(EnteredKey left, EnteredKey right) => !(left == right);
    }
}
