"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var HotKeys;
        (function (HotKeys) {
            var HotkeyEntry = (function () {
                function HotkeyEntry(hotKeyEntryWrpper, modKeys, keyName, allowIn) {
                    this.hotKeyEntryWrpper = hotKeyEntryWrpper;
                    this.modKeys = modKeys;
                    this.keyName = keyName;
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
            var fixingKeyNameTypoMap = {
                "BackQuart": "BackQuote",
                "SingleQuart": "SingleQuote",
            };
            function register(hotKeyEntryWrpper, modKeys, keyName, allowIn) {
                var id = idSeq++;
                var hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper, modKeys, fixingKeyNameTypoMap[keyName] || keyName, allowIn);
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
                    if (entry.keyName.toLocaleLowerCase() !== e.keyName.toLocaleLowerCase())
                        continue;
                    var modKeys = entry.modKeys;
                    if (entry.keyName === 'Shift')
                        modKeys |= 1;
                    if (entry.keyName === 'Ctrl')
                        modKeys |= 2;
                    if (entry.keyName === 'Alt')
                        modKeys |= 4;
                    if (e.modKeys !== modKeys)
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
                    var key = ev.key;
                    var code = ev.code;
                    var keyName = convertToKeyName(ev);
                    var tagName = ev.srcElement.tagName;
                    var type = ev.srcElement.getAttribute('type');
                    var preventDefault1 = onKeyDown({ modKeys: modKeys, keyName: keyName, tagName: tagName, type: type });
                    var preventDefault2 = isWasm === true ? hotKeysWrpper.invokeMethod('OnKeyDown', modKeys, keyName, tagName, type, key, code) : false;
                    if (preventDefault1 || preventDefault2)
                        ev.preventDefault();
                    if (isWasm === false)
                        hotKeysWrpper.invokeMethodAsync('OnKeyDown', modKeys, keyName, tagName, type, key, code);
                });
            }
            HotKeys.attach = attach;
            var convertToKeyNameMap = {
                "Escape": "ESC",
                "Control": "Ctrl",
                "OS": "Meta",
                "Minus": "Hyphen",
                "Quote": "SingleQuote",
                "Decimal": "Period",
            };
            function convertToKeyName(ev) {
                return /^[a-z]$/i.test(ev.key) ? ev.key.toUpperCase() :
                    /^\d$/.test(ev.key) ? 'Num' + ev.key :
                        convertToKeyNameLevel2(ev.code || ev.key);
            }
            function convertToKeyNameLevel2(keyName) {
                var converted = /^Digit\d$/.test(keyName) ? 'Num' + keyName.charAt(5) :
                    /^Numpad\d$/.test(keyName) ? 'Num' + keyName.charAt(6) :
                        /^Volume.+$/.test(keyName) ? 'Audio' + keyName :
                            /^Arrow.+$/.test(keyName) ? keyName.substr(5) :
                                /^[SACOM].+(Left|Right)$/.test(keyName) ? keyName.replace(/(Left|Right)$/, '') :
                                    keyName.replace(/^Bracket/, 'Blace').replace(/^Page/, 'Pg').replace(/^Numpad/, '');
                return convertToKeyNameMap[converted] || converted;
            }
        })(HotKeys = Blazor.HotKeys || (Blazor.HotKeys = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
