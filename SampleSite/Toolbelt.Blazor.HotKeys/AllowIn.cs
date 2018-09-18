using System;

namespace Toolbelt.Blazor.HotKeys
{
    [Flags]
    public enum AllowIn
    {
        None = 0b0000,
        Input = 0b0001,
        TextArea = 0b0010
    }
}
