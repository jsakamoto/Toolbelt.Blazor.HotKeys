using System;
using NUnit.Framework;

namespace Toolbelt.Blazor.HotKeys.Test
{
    public class HotKeyDownEventArgsTest
    {
        [Test]
        public void SetPreventDefault_to_True_Throws_Exception_on_BlazorServer()
        {
            var args = new HotKeyDownEventArgs(ModKeys.None, Keys.A, "INPUT", "text", isWasm: false, nativeKey: "a", nativeCode: "KeyA");
            Assert.Throws<InvalidOperationException>(() =>
            {
                args.PreventDefault = true;
            });
        }

        [Test]
        public void SetPreventDefault_to_False_on_BlazorServer()
        {
            var args = new HotKeyDownEventArgs(ModKeys.None, Keys.A, "INPUT", "text", isWasm: false, nativeKey: "a", nativeCode: "KeyA");
            args.PreventDefault.IsFalse();
            args.PreventDefault = false;
            args.PreventDefault.IsFalse();
        }

        [Test]
        public void SetPreventDefault_Success_on_BlazorWebAssembly()
        {
            var args = new HotKeyDownEventArgs(ModKeys.None, Keys.A, "INPUT", "text", isWasm: true, nativeKey: "a", nativeCode: "KeyA");
            args.PreventDefault.IsFalse();
            args.PreventDefault = true;
            args.PreventDefault.IsTrue();
        }
    }
}