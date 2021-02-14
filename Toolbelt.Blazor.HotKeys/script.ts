namespace Toolbelt.Blazor.HotKeys {

    const enum AllowIn {
        None = 0b0000,
        Input = 0b0001,
        TextArea = 0b0010,
        NonTextInput = 0b0100,
    }

    const enum ModKeys {
        None = 0,
        Shift = 0x01,
        Ctrl = 0x02,
        Alt = 0x04,
        Meta = 0x08
    }

    const enum Keys {
        Shift = 0x10,
        Ctrl = 0x11,
        Alt = 0x12,
    }

    class HotkeyEntry {
        constructor(
            private hotKeyEntryWrpper: any,
            public modKeys: ModKeys,
            public keyName: string,
            public allowIn: AllowIn
        ) {
        }

        public action(): void {
            this.hotKeyEntryWrpper.invokeMethodAsync('InvokeAction');
        }
    }

    const NonTextInputTypes = ["button", "checkbox", "color", "file", "image", "radio", "range", "reset", "submit",];

    let idSeq: number = 0;
    const hotKeyEntries: { [key: number]: HotkeyEntry } = {};

    const fixingKeyNameTypoMap: { [key: string]: string } = {
        "BackQuart": "BackQuote",
        "SingleQuart": "SingleQuote",
    };

    export function register(hotKeyEntryWrpper: any, modKeys: ModKeys, keyName: string, allowIn: AllowIn): number {
        const id = idSeq++;
        const hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper, modKeys, fixingKeyNameTypoMap[keyName] || keyName, allowIn);
        hotKeyEntries[id] = hotKeyEntry;
        return id;
    }

    export function unregister(id: number): void {
        delete hotKeyEntries[id];
    }

    function onKeyDown(e: { modKeys: ModKeys, keyName: string, tagName: string, type: string | null }): boolean {
        let preventDefault = false;

        for (const key in hotKeyEntries) {
            if (!hotKeyEntries.hasOwnProperty(key)) continue;
            const entry = hotKeyEntries[key];

            if (entry.keyName.toLocaleLowerCase() !== e.keyName.toLocaleLowerCase()) continue;

            let modKeys = entry.modKeys;
            if (entry.keyName === 'Shift') modKeys |= ModKeys.Shift;
            if (entry.keyName === 'Ctrl') modKeys |= ModKeys.Ctrl;
            if (entry.keyName === 'Alt') modKeys |= ModKeys.Alt;
            if (e.modKeys !== modKeys) continue;

            if (!isAllowedIn(entry, e.tagName, e.type)) continue;

            preventDefault = true;

            entry.action();
        }

        return preventDefault;
    }

    function isAllowedIn(entry: HotkeyEntry, tagName: string, type: string | null): boolean {
        if (tagName === "TEXTAREA") {
            return (entry.allowIn & AllowIn.TextArea) === AllowIn.TextArea;
        }

        if (tagName == "INPUT") {
            if ((type !== null) &&
                (NonTextInputTypes.indexOf(type) !== -1) &&
                (entry.allowIn & AllowIn.NonTextInput) === AllowIn.NonTextInput
            ) return true;

            return (entry.allowIn & AllowIn.Input) === AllowIn.Input;
        }

        return true;
    }

    export function attach(hotKeysWrpper: any, isWasm: boolean): void {
        document.addEventListener('keydown', ev => {
            if (typeof (ev["altKey"]) === 'undefined') return;
            const modKeys =
                (ev.shiftKey ? ModKeys.Shift : 0) +
                (ev.ctrlKey ? ModKeys.Ctrl : 0) +
                (ev.altKey ? ModKeys.Alt : 0) +
                (ev.metaKey ? ModKeys.Meta : 0);
            const key = ev.key;
            const code = ev.code;
            const keyName = convertToKeyName(ev);
            const tagName = (ev.srcElement as HTMLElement).tagName;
            const type = (ev.srcElement as HTMLElement).getAttribute('type');
            const preventDefault1 = onKeyDown({ modKeys, keyName, tagName, type });
            const preventDefault2 = isWasm === true ? hotKeysWrpper.invokeMethod('OnKeyDown', modKeys, keyName, tagName, type, key, code) : false;
            if (preventDefault1 || preventDefault2) ev.preventDefault();
            if (isWasm === false) hotKeysWrpper.invokeMethodAsync('OnKeyDown', modKeys, keyName, tagName, type, key, code);
        });
    }

    const convertToKeyNameMap: { [key: string]: string } = {
        "Escape": "ESC",
        "Control": "Ctrl",
        "OS": "Meta",
        "Minus": "Hyphen",
        "Quote": "SingleQuote",
        "Decimal": "Period",
    };

    function convertToKeyName(ev: KeyboardEvent): string {
        return /^[a-z]$/i.test(ev.key) ? ev.key.toUpperCase() :
            /^\d$/.test(ev.key) ? 'Num' + ev.key :
                convertToKeyNameLevel2(ev.code || ev.key);
    }

    function convertToKeyNameLevel2(keyName: string): string {
        // "Digit1" -> "Num1"
        const converted = /^Digit\d$/.test(keyName) ? 'Num' + keyName.charAt(5) :
            // "Numpad1" -> "Num1"
            /^Numpad\d$/.test(keyName) ? 'Num' + keyName.charAt(6) :
                // "VolumeUp" -> "AudioVolumeUp"
                /^Volume.+$/.test(keyName) ? 'Audio' + keyName :
                    // "ArrowUp" -> "Up"
                    /^Arrow.+$/.test(keyName) ? keyName.substr(5) :
                        // "ShiftLeft" -> "Shift"
                        // "SACOM" ... ("S"hift|"A"lt|"C"ontrol|"O"S|"M"eta)(Left|Right)
                        /^[SACOM].+(Left|Right)$/.test(keyName) ? keyName.replace(/(Left|Right)$/, '') :
                            // "BracketLeft" -> "BlaceLeft"
                            // "PageUp" -> "PgUp"
                            keyName.replace(/^Bracket/, 'Blace').replace(/^Page/, 'Pg').replace(/^Numpad/, '');
        return convertToKeyNameMap[converted] || converted;
    }
}