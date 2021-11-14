using NUnit.Framework;
using static Toolbelt.Blazor.HotKeys.Keys;
using static Toolbelt.Blazor.HotKeys.ModKeys;

namespace Toolbelt.Blazor.HotKeys.Test;

public class HotKeyEntryTest
{
#pragma warning disable CS0618 // Type or member is obsolete
    [Test]
    public void Compatiblity_Of_AllowIn_Property_Test()
    {
        var entry1 = new HotKeyEntry(None, A, Exclude.Default, description: "", action: _ => Task.CompletedTask);
        entry1.AllowIn.Is(AllowIn.None);

        var entry2 = new HotKeyEntry(None, A, Exclude.InputText, description: "", action: _ => Task.CompletedTask);
        entry2.AllowIn.Is(AllowIn.NonTextInput | AllowIn.TextArea);

        var entry3 = new HotKeyEntry(None, A, Exclude.InputText | Exclude.TextArea, description: "", action: _ => Task.CompletedTask);
        entry3.AllowIn.Is(AllowIn.NonTextInput);

        var entry4 = new HotKeyEntry(None, A, Exclude.ContentEditable, description: "", action: _ => Task.CompletedTask);
        entry4.AllowIn.Is(AllowIn.Input | AllowIn.TextArea);
    }
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
    [Test]
    public void Compatibility_AlloIn_in_ctor_Test()
    {
        var entry1 = new HotKeyEntry(None, A, AllowIn.None, description: "", action: _ => Task.CompletedTask);
        entry1.Exclude.Is(Exclude.Default);

        var entry2 = new HotKeyEntry(None, A, AllowIn.Input, description: "", action: _ => Task.CompletedTask);
        entry2.Exclude.Is(Exclude.TextArea);

        var entry3 = new HotKeyEntry(None, A, AllowIn.NonTextInput | AllowIn.TextArea, description: "", action: _ => Task.CompletedTask);
        entry3.Exclude.Is(Exclude.InputText);
    }
#pragma warning restore CS0618 // Type or member is obsolete
}
