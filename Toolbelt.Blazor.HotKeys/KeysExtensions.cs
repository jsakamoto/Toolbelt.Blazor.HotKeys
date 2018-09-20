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
            switch (value)
            {
                case Keys.Num0: return "0";
                case Keys.Num1: return "1";
                case Keys.Num2: return "2";
                case Keys.Num3: return "3";
                case Keys.Num4: return "4";
                case Keys.Num5: return "5";
                case Keys.Num6: return "6";
                case Keys.Num7: return "7";
                case Keys.Num8: return "8";
                case Keys.Num9: return "9";
                case Keys.SemiColon: return ";";
                case Keys.Equal: return "=";
                case Keys.Hyphen: return "-";
                case Keys.Comma: return ",";
                case Keys.Period: return ".";
                case Keys.Slash: return "/";
                case Keys.BackQuart: return "`";
                case Keys.BlaceLeft: return "[";
                case Keys.BackSlash: return "]";
                case Keys.BlaceRight: return "\\";
                case Keys.SingleQuart: return "'";
                default:
                    return value.ToString();
            }
        }
    }
}
