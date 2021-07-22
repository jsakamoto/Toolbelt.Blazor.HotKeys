function fireOnChange(element) {
    var event = new Event('change');
    element.dispatchEvent(event);
    console.log('onchange has been fired.', element)
}