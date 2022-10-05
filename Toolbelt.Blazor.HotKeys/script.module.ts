export namespace Toolbelt.Blazor.HotKeys {

    const enum Exclude {
        None = 0,
        InputText = 0b0001,
        InputNonText = 0b0010,
        TextArea = 0b0100,
        ContentEditable = 0b1000
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

    const enum HotKeyMode {
        Default,
        NativeKey
    }

    class HotkeyEntry {
        public selector: string;

        constructor(
            private hotKeyEntryWrpper: any,
            public mode: HotKeyMode,
            public modKeys: ModKeys,
            public keyName: string,
            public exclude: Exclude
        ) {
            const excludeSelectors = [] as string[];
            if ((exclude & Exclude.InputText) !== 0) excludeSelectors.push("input[type=text]");
            if ((exclude & Exclude.InputNonText) !== 0) excludeSelectors.push("input:not([type=text])");
            if ((exclude & Exclude.TextArea) !== 0) excludeSelectors.push("textarea");
            this.selector = "*:not(" + excludeSelectors.join(",") + ")";
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

    export function register(hotKeyEntryWrpper: any, mode: HotKeyMode, modKeys: ModKeys, keyName: string, exclude: Exclude): number {
        const id = idSeq++;
        const hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper, mode, modKeys, fixingKeyNameTypoMap[keyName] || keyName, exclude);
        hotKeyEntries[id] = hotKeyEntry;
        return id;
    }

    export function unregister(id: number): void {
        delete hotKeyEntries[id];
    }

    function onKeyDown(event: { modKeys: ModKeys, keyName: string, nativeKey: string, srcElement: HTMLElement, tagName: string, type: string | null }): boolean {
        let preventDefault = false;

        for (const key in hotKeyEntries) {
            if (!hotKeyEntries.hasOwnProperty(key)) continue;
            const entry = hotKeyEntries[key];
            const isDefaultMode = entry.mode === HotKeyMode.Default;

            const eventKeyName = isDefaultMode ? event.keyName : event.nativeKey;
            if (entry.keyName.localeCompare(eventKeyName, 'en', { sensitivity: 'base' }) !== 0) continue;

            const eventModkeys = isDefaultMode ? event.modKeys : (event.modKeys & (0xffff ^ ModKeys.Shift));
            let entryModKeys = isDefaultMode ? entry.modKeys : (entry.modKeys & (0xffff ^ ModKeys.Shift));
            if (entry.keyName === 'Shift' && isDefaultMode) entryModKeys |= ModKeys.Shift;
            if (entry.keyName === 'Ctrl') entryModKeys |= ModKeys.Ctrl;
            if (entry.keyName === 'Alt') entryModKeys |= ModKeys.Alt;
            if (eventModkeys !== entryModKeys) continue;

            if (!isAllowedIn(entry, event)) continue;

            preventDefault = true;

            entry.action();
        }

        return preventDefault;
    }

    function isAllowedIn(entry: HotkeyEntry, e: { srcElement: HTMLElement, tagName: string, type: string | null }): boolean {

        if ((entry.exclude & Exclude.InputText) !== 0) {
            if (e.tagName === "INPUT" && NonTextInputTypes.indexOf(e.type || '') === -1) return false;
        }
        if ((entry.exclude & Exclude.InputNonText) !== 0) {
            if (e.tagName === "INPUT" && NonTextInputTypes.indexOf(e.type || '') !== -1) return false;
        }
        if ((entry.exclude & Exclude.TextArea) !== 0) {
            if (e.tagName === "TEXTAREA") return false;
        }
        if ((entry.exclude & Exclude.ContentEditable) !== 0) {
            if (e.srcElement.contentEditable === "true") return false;
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

            const srcElement = ev.srcElement as HTMLElement;
            const tagName = srcElement.tagName;
            const type = srcElement.getAttribute('type');

            const preventDefault1 = onKeyDown({ modKeys, keyName, nativeKey: key, srcElement, tagName, type });
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
        const converted = convertToKeyNameLevel2(ev.code || ev.key);
        return convertToKeyNameMap[converted] || converted;
    }

    function convertToKeyNameLevel2(codeOrKey: string): string {
        const match = (pattern: RegExp) => codeOrKey.match(pattern)?.input;
        switch (codeOrKey) {

            // "KeyA/B/C..." -> "A/B/C..."
            case match(/^Key[A-Z]/): return codeOrKey.charAt(3);

            // "Digit1" -> "Num1"
            case match(/^Digit\d$/): return 'Num' + codeOrKey.charAt(5);

            // "Numpad1" -> "Num1"
            case match(/^Numpad\d$/): return 'Num' + codeOrKey.charAt(6);

            // "VolumeUp/Down" -> "AudioVolumeUp/Down"
            case match(/^Volume.+$/): return 'Audio' + codeOrKey;

            // "ArrowUp/Down/Left/Right" -> "Up/Down/Left/Right"
            case match(/^Arrow.+$/): return codeOrKey.substr(5);

            // "ShiftLeft" -> "Shift"
            // "SACOM" ... ("S"hift|"A"lt|"C"ontrol|"O"S|"M"eta)(Left|Right)
            case match(/^[SACOM].+(Left|Right)$/): return codeOrKey.replace(/(Left|Right)$/, '');

            // "BracketLeft/Right" -> "BlaceLeft/Right"
            // "PageUp/Down" -> "PgUp/Down"
            default: return codeOrKey.replace(/^Bracket/, 'Blace').replace(/^Page/, 'Pg').replace(/^Numpad/, '');
        }
    }
}