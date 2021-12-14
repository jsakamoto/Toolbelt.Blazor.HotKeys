namespace Toolbelt.Blazor.HotKeys {
    const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
    const url = ['./script.module.min.js', searchParam].filter(v => v != '').join('?');
    export var ready = import(url).then(m => {
        Object.assign(HotKeys, m.Toolbelt.Blazor.HotKeys);
    });
}
