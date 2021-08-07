var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        function fireOnChange(element) {
            var event = new Event('change');
            element.dispatchEvent(event);
            console.log('onchange has been fired.', element)
        }
        Blazor.fireOnChange = fireOnChange;
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));

