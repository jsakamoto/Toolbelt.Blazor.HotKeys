namespace Toolbelt.Blazor.HotKeys {
    const searchParam = document.currentScript?.getAttribute('src')?.split('?').pop() || '';
    export var ready = import('./script.module.min.js?' + searchParam).then(m => {
        Object.assign(HotKeys, m.Toolbelt.Blazor.HotKeys);
    });
}
