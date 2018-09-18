declare var DotNet: any;

namespace Toolbelt.Blazor.HotKeys {
    document.addEventListener('keydown', ev => {
        const keyCode = ev.keyCode;
        if (0x10 <= keyCode && keyCode <= 0x12) return;
        let modKeys =
            (ev.shiftKey ? 0x01 : 0) +
            (ev.ctrlKey ? 0x02 : 0) +
            (ev.altKey ? 0x04 : 0) +
            (ev.metaKey ? 0x08 : 0);
        let preventDefault = DotNet.invokeMethod('SampleSite', 'OnKeyDown', modKeys, keyCode, ev.srcElement.tagName);
        console.log(`preventDefault is: [${preventDefault}]`);
        if (preventDefault) {
            ev.preventDefault();
        }
    });
}