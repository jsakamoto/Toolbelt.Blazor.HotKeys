namespace Toolbelt.Blazor.HotKeys {

    class HotkeyEntry {
        constructor(
            private hotKeyEntryWrpper: any
        ) {
        }
    }

    let idSeq: number = 0;
    const hotKeyEntries: { [key: number]: HotkeyEntry } = {};

    export function register(hotKeyEntryWrpper: any): number {
        const id = idSeq++;
        const hotKeyEntry = new HotkeyEntry(hotKeyEntryWrpper);
        hotKeyEntries[idSeq] = hotKeyEntry;
        return id;
    }

    export function unregister(id: number): void {
        delete hotKeyEntries[id];
    }

    function onKeyDown(modKeys: number, keyCode: number, tagName: string, type: string | null): void {

        for (const key in hotKeyEntries) {
            if (!hotKeyEntries.hasOwnProperty(key)) continue;
            const entry = hotKeyEntries[key];
            //entry.
        }
    }

    export function attach(hotKeysWrpper: any): void {

        document.addEventListener('keydown', ev => {
            const keyCode = ev.keyCode;
            let modKeys =
                (ev.shiftKey ? 0x01 : 0) +
                (ev.ctrlKey ? 0x02 : 0) +
                (ev.altKey ? 0x04 : 0) +
                (ev.metaKey ? 0x08 : 0);
            let preventDefault = hotKeysWrpper.invokeMethod('OnKeyDown', modKeys, keyCode, (ev.srcElement as HTMLElement).tagName, (ev.srcElement as HTMLElement).getAttribute('type')) as boolean;
            if (preventDefault) ev.preventDefault();
        });
    }
}