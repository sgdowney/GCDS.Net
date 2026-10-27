const sinInput = document.querySelector('gcds-input[name="Sin"]');
sinInput.validator = [sinValidation()];

const otherInput = document.querySelector('gcds-input[name="unconnectedField"]');
otherInput.validator = [sinValidation()];
