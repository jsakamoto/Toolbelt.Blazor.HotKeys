var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var HotKeys;
        (function (HotKeys) {
            document.addEventListener('keydown', function (ev) {
                var keyCode = ev.keyCode;
                if (0x10 <= keyCode && keyCode <= 0x12)
                    return;
                var modKeys = (ev.shiftKey ? 0x01 : 0) +
                    (ev.ctrlKey ? 0x02 : 0) +
                    (ev.altKey ? 0x04 : 0) +
                    (ev.metaKey ? 0x08 : 0);
                var preventDefault = DotNet.invokeMethod('SampleSite', 'OnKeyDown', modKeys, keyCode, ev.srcElement.tagName);
                console.log("preventDefault is: [" + preventDefault + "]");
                if (preventDefault) {
                    ev.preventDefault();
                }
            });
        })(HotKeys = Blazor.HotKeys || (Blazor.HotKeys = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
//# sourceMappingURL=script.js.map