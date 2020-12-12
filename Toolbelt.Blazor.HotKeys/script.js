"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var HotKeys;
        (function (HotKeys) {
            var HotkeyEntry = (function () {
                function HotkeyEntry(hotKeyEntryWrpper, modKeys, key, allowIn) {
                    this.hotKeyEntryWrpper = hotKeyEntryWrpper;
                    this.modKeys = modKeys;
                    this.key = key;
                    this.allowIn = allowIn;
                }
                HotkeyEntry.prototype.action = function () {
                    this.hotKeyEntryWrpper.invokeMethodAsync('InvokeAction');
                };
                return HotkeyEntry;
            }());
            var NonTextInputTypes = ["button", "checkbox", "color", "file", "image", "radio", "range", "reset", "submit",];
            var idSeq = 0;
            var hotKeyEntries = {};
            function register(hotKeyEntryWrpper, modKeys, key, allowIn) {
                var id = idSeq++;
                var hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper, modKeys, key, allowIn);
                hotKeyEntries[id] = hotKeyEntry;
                return id;
            }
            HotKeys.register = register;
            function unregister(id) {
                delete hotKeyEntries[id];
            }
            HotKeys.unregister = unregister;
            function onKeyDown(e) {
                var preventDefault = false;
                for (var key in hotKeyEntries) {
                    if (!hotKeyEntries.hasOwnProperty(key))
                        continue;
                    var entry = hotKeyEntries[key];
                    if (entry.key !== e.keyCode)
                        continue;
                    var modKeys = entry.modKeys;
                    if (entry.key == 16)
                        modKeys |= 1;
                    if (entry.key == 17)
                        modKeys |= 2;
                    if (entry.key == 18)
                        modKeys |= 4;
                    if (e.modKeys != modKeys)
                        continue;
                    if (!isAllowedIn(entry, e.tagName, e.type))
                        continue;
                    preventDefault = true;
                    entry.action();
                }
                return preventDefault;
            }
            function isAllowedIn(entry, tagName, type) {
                if (tagName === "TEXTAREA") {
                    return (entry.allowIn & 2) === 2;
                }
                if (tagName == "INPUT") {
                    if ((type !== null) &&
                        (NonTextInputTypes.indexOf(type) !== -1) &&
                        (entry.allowIn & 4) === 4)
                        return true;
                    return (entry.allowIn & 1) === 1;
                }
                return true;
            }
            function attach(hotKeysWrpper, isWasm) {
                document.addEventListener('keydown', function (ev) {
                    var modKeys = (ev.shiftKey ? 1 : 0) +
                        (ev.ctrlKey ? 2 : 0) +
                        (ev.altKey ? 4 : 0) +
                        (ev.metaKey ? 8 : 0);
                    var keyCode = ev.keyCode;
                    var tagName = ev.srcElement.tagName;
                    var type = ev.srcElement.getAttribute('type');
                    var preventDefault1 = onKeyDown({ modKeys: modKeys, keyCode: keyCode, tagName: tagName, type: type });
                    var preventDefault2 = isWasm === true ? hotKeysWrpper.invokeMethod('OnKeyDown', modKeys, keyCode, tagName, type) : false;
                    if (preventDefault1 || preventDefault2)
                        ev.preventDefault();
                    if (isWasm === false)
                        hotKeysWrpper.invokeMethodAsync('OnKeyDown', modKeys, keyCode, tagName, type);
                });
            }
            HotKeys.attach = attach;
        })(HotKeys = Blazor.HotKeys || (Blazor.HotKeys = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
