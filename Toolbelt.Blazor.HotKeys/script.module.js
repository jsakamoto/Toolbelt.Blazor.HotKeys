export var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var HotKeys;
        (function (HotKeys) {
            class HotkeyEntry {
                constructor(hotKeyEntryWrpper, modKeys, keyName, exclude) {
                    this.hotKeyEntryWrpper = hotKeyEntryWrpper;
                    this.modKeys = modKeys;
                    this.keyName = keyName;
                    this.exclude = exclude;
                    const excludeSelectors = [];
                    if ((exclude & 1) !== 0)
                        excludeSelectors.push("input[type=text]");
                    if ((exclude & 2) !== 0)
                        excludeSelectors.push("input:not([type=text])");
                    if ((exclude & 4) !== 0)
                        excludeSelectors.push("textarea");
                    this.selector = "*:not(" + excludeSelectors.join(",") + ")";
                }
                action() {
                    this.hotKeyEntryWrpper.invokeMethodAsync('InvokeAction');
                }
            }
            const NonTextInputTypes = ["button", "checkbox", "color", "file", "image", "radio", "range", "reset", "submit",];
            let idSeq = 0;
            const hotKeyEntries = {};
            const fixingKeyNameTypoMap = {
                "BackQuart": "BackQuote",
                "SingleQuart": "SingleQuote",
            };
            function register(hotKeyEntryWrpper, modKeys, keyName, exclude) {
                const id = idSeq++;
                const hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper, modKeys, fixingKeyNameTypoMap[keyName] || keyName, exclude);
                hotKeyEntries[id] = hotKeyEntry;
                return id;
            }
            HotKeys.register = register;
            function unregister(id) {
                delete hotKeyEntries[id];
            }
            HotKeys.unregister = unregister;
            function onKeyDown(e) {
                let preventDefault = false;
                for (const key in hotKeyEntries) {
                    if (!hotKeyEntries.hasOwnProperty(key))
                        continue;
                    const entry = hotKeyEntries[key];
                    if (entry.keyName.toLocaleLowerCase() !== e.keyName.toLocaleLowerCase())
                        continue;
                    let modKeys = entry.modKeys;
                    if (entry.keyName === 'Shift')
                        modKeys |= 1;
                    if (entry.keyName === 'Ctrl')
                        modKeys |= 2;
                    if (entry.keyName === 'Alt')
                        modKeys |= 4;
                    if (e.modKeys !== modKeys)
                        continue;
                    if (!isAllowedIn(entry, e))
                        continue;
                    preventDefault = true;
                    entry.action();
                }
                return preventDefault;
            }
            function isAllowedIn(entry, e) {
                if ((entry.exclude & 1) !== 0) {
                    if (e.tagName === "INPUT" && NonTextInputTypes.indexOf(e.type || '') === -1)
                        return false;
                }
                if ((entry.exclude & 2) !== 0) {
                    if (e.tagName === "INPUT" && NonTextInputTypes.indexOf(e.type || '') !== -1)
                        return false;
                }
                if ((entry.exclude & 4) !== 0) {
                    if (e.tagName === "TEXTAREA")
                        return false;
                }
                if ((entry.exclude & 8) !== 0) {
                    if (e.srcElement.contentEditable === "true")
                        return false;
                }
                return true;
            }
            function attach(hotKeysWrpper, isWasm) {
                document.addEventListener('keydown', ev => {
                    if (typeof (ev["altKey"]) === 'undefined')
                        return;
                    const modKeys = (ev.shiftKey ? 1 : 0) +
                        (ev.ctrlKey ? 2 : 0) +
                        (ev.altKey ? 4 : 0) +
                        (ev.metaKey ? 8 : 0);
                    const key = ev.key;
                    const code = ev.code;
                    const keyName = convertToKeyName(ev);
                    const srcElement = ev.srcElement;
                    const tagName = srcElement.tagName;
                    const type = srcElement.getAttribute('type');
                    const preventDefault1 = onKeyDown({ modKeys, keyName, srcElement, tagName, type });
                    const preventDefault2 = isWasm === true ? hotKeysWrpper.invokeMethod('OnKeyDown', modKeys, keyName, tagName, type, key, code) : false;
                    if (preventDefault1 || preventDefault2)
                        ev.preventDefault();
                    if (isWasm === false)
                        hotKeysWrpper.invokeMethodAsync('OnKeyDown', modKeys, keyName, tagName, type, key, code);
                });
            }
            HotKeys.attach = attach;
            const convertToKeyNameMap = {
                "Escape": "ESC",
                "Control": "Ctrl",
                "OS": "Meta",
                "Minus": "Hyphen",
                "Quote": "SingleQuote",
                "Decimal": "Period",
            };
            function convertToKeyName(ev) {
                const converted = convertToKeyNameLevel2(ev.code || ev.key);
                return convertToKeyNameMap[converted] || converted;
            }
            function convertToKeyNameLevel2(codeOrKey) {
                const match = (pattern) => { var _a; return (_a = codeOrKey.match(pattern)) === null || _a === void 0 ? void 0 : _a.input; };
                switch (codeOrKey) {
                    case match(/^Key[A-Z]/): return codeOrKey.charAt(3);
                    case match(/^Digit\d$/): return 'Num' + codeOrKey.charAt(5);
                    case match(/^Numpad\d$/): return 'Num' + codeOrKey.charAt(6);
                    case match(/^Volume.+$/): return 'Audio' + codeOrKey;
                    case match(/^Arrow.+$/): return codeOrKey.substr(5);
                    case match(/^[SACOM].+(Left|Right)$/): return codeOrKey.replace(/(Left|Right)$/, '');
                    default: return codeOrKey.replace(/^Bracket/, 'Blace').replace(/^Page/, 'Pg').replace(/^Numpad/, '');
                }
            }
        })(HotKeys = Blazor.HotKeys || (Blazor.HotKeys = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
