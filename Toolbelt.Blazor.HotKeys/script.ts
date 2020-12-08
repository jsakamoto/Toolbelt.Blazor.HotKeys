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
            public key: number,
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

    export function register(hotKeyEntryWrpper: any, modKeys: ModKeys, key: number, allowIn: AllowIn): number {
        const id = idSeq++;
        const hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper, modKeys, key, allowIn);
        hotKeyEntries[idSeq] = hotKeyEntry;
        return id;
    }

    export function unregister(id: number): void {
        delete hotKeyEntries[id];
    }

    function onKeyDown(e: { modKeys: ModKeys, keyCode: number, tagName: string, type: string | null }): boolean {

        let preventDefault = false;

        for (const key in hotKeyEntries) {
            if (!hotKeyEntries.hasOwnProperty(key)) continue;
            const entry = hotKeyEntries[key];

            if (entry.key !== e.keyCode) continue;

            let modKeys = entry.modKeys;
            if (entry.key == Keys.Shift) modKeys |= ModKeys.Shift;
            if (entry.key == Keys.Ctrl) modKeys |= ModKeys.Ctrl;
            if (entry.key == Keys.Alt) modKeys |= ModKeys.Alt;
            if (e.modKeys != modKeys) continue;

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
            const modKeys =
                (ev.shiftKey ? ModKeys.Shift : 0) +
                (ev.ctrlKey ? ModKeys.Ctrl : 0) +
                (ev.altKey ? ModKeys.Alt : 0) +
                (ev.metaKey ? ModKeys.Meta : 0);
            const keyCode = ev.keyCode;
            const preventDefault1 = onKeyDown({ modKeys, keyCode, tagName: (ev.srcElement as HTMLElement).tagName, type: (ev.srcElement as HTMLElement).getAttribute('type') });
            const preventDefault2 = isWasm === true ? hotKeysWrpper.invokeMethod('OnKeyDown', modKeys, keyCode, (ev.srcElement as HTMLElement).tagName, (ev.srcElement as HTMLElement).getAttribute('type')) : false;
            if (preventDefault1 || preventDefault2) ev.preventDefault();
            if (isWasm === false) hotKeysWrpper.invokeMethodAsync('OnKeyDown', modKeys, keyCode, (ev.srcElement as HTMLElement).tagName, (ev.srcElement as HTMLElement).getAttribute('type'));
        });
    }
}