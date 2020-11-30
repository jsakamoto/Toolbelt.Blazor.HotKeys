"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var HotKeys;
        (function (HotKeys) {
            var HotkeyEntry = (function () {
                function HotkeyEntry(hotKeyEntryWrpper) {
                    this.hotKeyEntryWrpper = hotKeyEntryWrpper;
                }
                return HotkeyEntry;
            }());
            var idSeq = 0;
            var hotKeyEntries = {};
            function register(hotKeyEntryWrpper) {
                var id = idSeq++;
                var hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper);
                hotKeyEntries[idSeq] = hotKeyEntry;
                return id;
            }
            HotKeys.register = register;
            function unregister(id) {
                delete hotKeyEntries[id];
            }
            HotKeys.unregister = unregister;
            function onKeyDown(modKeys, keyCode, tagName, type) {
                for (var key in hotKeyEntries) {
                    if (!hotKeyEntries.hasOwnProperty(key))
                        continue;
                    var entry = hotKeyEntries[key];
                }
            }
            function attach(hotKeysWrpper) {
                document.addEventListener('keydown', function (ev) {
                    var keyCode = ev.keyCode;
                    var modKeys = (ev.shiftKey ? 0x01 : 0) +
                        (ev.ctrlKey ? 0x02 : 0) +
                        (ev.altKey ? 0x04 : 0) +
                        (ev.metaKey ? 0x08 : 0);
                    var preventDefault = hotKeysWrpper.invokeMethod('OnKeyDown', modKeys, keyCode, ev.srcElement.tagName, ev.srcElement.getAttribute('type'));
                    if (preventDefault)
                        ev.preventDefault();
                });
            }
            HotKeys.attach = attach;
        })(HotKeys = Blazor.HotKeys || (Blazor.HotKeys = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
