namespace Toolbelt.Blazor.HotKeys
{
    /// <summary>
    /// The extension methods for "Keys" enum type.
    /// </summary>
    public static class KeysExtensions
    {
        /// <summary>
        /// Returns a String that represent readable key name of this Keys enum value, like "Top", "Left", "Enter", "A", "B", "C", etc.
        /// </summary>
        public static string ToKeyString(this Keys value)
        {
            return value switch
            {
                0 => null,
                Keys.Num0 => "0",
                Keys.Num1 => "1",
                Keys.Num2 => "2",
                Keys.Num3 => "3",
                Keys.Num4 => "4",
                Keys.Num5 => "5",
                Keys.Num6 => "6",
                Keys.Num7 => "7",
                Keys.Num8 => "8",
                Keys.Num9 => "9",
                Keys.SemiColon => ";",
                Keys.Equal => "=",
                Keys.Hyphen => "-",
                Keys.Comma => ",",
                Keys.Period => ".",
                Keys.Slash => "/",
                Keys.BackQuote => "`",
                Keys.BlaceLeft => "[",
                Keys.BackSlash => "]",
                Keys.BlaceRight => "\\",
                Keys.SingleQuote => "'",
                _ => value.ToString(),
            };
        }
    }
}
