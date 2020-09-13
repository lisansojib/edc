'use strict'
function makeArray(stringData) {
    return $.parseJSON(stringData);
}

String.prototype.replaceAll = function (stringToFind, stringToReplace) {
    if (stringToFind === stringToReplace) return this;
    var temp = this;
    var index = temp.indexOf(stringToFind);
    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }
    return temp;
};

function showBootboxConfirm(title, message, callback) {
    bootbox.confirm({
        title: title,
        message: message,
        size: "small",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            return callback(result);
        }
    });
}

function showBootboxPrompt(title, message, callback) {
    bootbox.prompt({
        title: title,
        message: "<p>" + message + "</p>",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            return callback(result);
        }
    });
}

/**
 * Show bootbox.js Select Prompt
 * @param {any} title - Prompt Title
 * @param {any} optionsArray - Input Options Array
 * @param {any} size - Prompt Size
 * @param {any} callback - Callback function
 */
function showBootboxSelectPrompt(title, optionsArray, size, callback) {
    if (!size) size = "small";
    bootbox.prompt({
        title: title,
        size: size,
        inputType: 'select',
        inputOptions: optionsArray,
        callback: function (result) {
            return callback(result);
        }
    });
}

/**
 * Preview on image load
 * @param {Event} event - onchange evnet
 * @param {HTMLElement} - Preview Element
 */
function previewImage(event, previewEl) {
    var file = event.target.files[0];
    var reader = new FileReader();

    reader.addEventListener("load", function () {
        previewEl.attr('src', reader.result);
    }, false);

    if (file) {
        reader.readAsDataURL(file);
    }
}

/**
 * Converts a string to boolean
 * @param {string} str - input string
 */
function convertToBoolean(str) {
    if (!str)
        return false;

    switch (str.toLowerCase()) {
        case "true":
        case "yes":
        case "1":
            return true;
        default: return false;
    }
}

/**
 * Set all checkbox value of a form element to true or false
 * instead of on/off.
 * @param {HTMLFormElement} $formEl - Jquery Form Element
 */
function setAllFormCheckBoxValue($formEl) {
    $formEl.find(':checkbox').each(function () {
        this.value = this.checked;
    });
}

/**
 * Convert form data to Json
 * @param {HTMLElement} $formEl - Jquery form element
 */
function formDataToJson($formEl) {
    setAllFormCheckBoxValue($formEl);

    var data = $formEl.serializeArray();
    var jsonObj = {};
    $.each(data, function (i, v) {
        jsonObj[v.name] = v.value;
    });

    return jsonObj;
}

/**
 * Set Form Data
 * @param {any} $formEl - Form Element
 * @param {any} data - data object
 */
function setFormData($formEl, data) {
    if (!data && ! typeof data === 'object') {
        console.error("Your data is not valid.");
        return;
    }

    try {
        $formEl.find("input, select").each(function () {
            try {
                var $input = $(this);
                var value = data[$input.attr('name')];
                if (!value) return;

                switch ($input.attr('type')) {
                    case "checkbox":
                        $input.prop("checked", value);
                        break;
                    case "radio":
                        $input.each(function (i) {
                            if ($(this).val() == value) $(this).attr({
                                checked: true
                            })
                        });
                        break;
                    case undefined:
                        if ($input.hasClass("select2-hidden-accessible")) {
                            var optionListName = $input[0].multiple ? $input.attr('name').replace("Ids", '') : $input.attr('name').replace("Id", '');
                            optionListName += "List";
                            initSelect2($input, data[optionListName]);
                            $input.val(value).trigger("change");
                        }
                        break;
                    case "file":
                        break;
                    default:
                        $input.val(value);
                        break;
                }
            } catch (e) {
                console.error(e);
            }
        });
    } catch (e) {
        console.error(e);
    }
}

/**
 * Disable elemnt
 * @param {HTMLElement} el - HTMLElement
 */
function disableElement(el) {
    el.attr('disabled', 'disabled');
}

/**
 * Enable element
 * @param {HTMLElement} el - HTMLElement
 */
function enableElement(el) {
    el.removeAttr('disabled');
}

/**
 * Makes an input readonly
 * @param {HTMLInputElement} el - Input element
 */
function makeReadonly(el) {
    el.prop("readonly", true);
}

/**
 * Removes readonly attribute
 * @param {HTMLInputElement} el - Input element
 */
function removeReadonly(el) {
    el.prop("readonly", false);
}

/**
 * Check a checkbox element
 * @param {HTMLInputElement} el - Checkbox Element
 */
function checkInput(el) {
    el.prop("checked", true);
}

function setCheckBox(el, value) {
    el.prop("checked", value);
}

/**
 * Un-check a checkbox element
 * @param {HTMLInputElement} el - Checkbox Element
 */
function unCheckInput(el) {
    el.prop("checked", false);
}

/**
 * Returned if checkbox is checked or not
 * @param {HTMLInputElement} el - Checkbox Element
 */
function isChecked(el) {
    return el.is(':checked');
}

/**
 * Initialize Select2
 * @param {HTMLSelectElement} Select Element
 * @param {Array} Array of Select2 Data
 * @param {Boolean} Allow Clear
 * @param {String} Placeholder
 * @param {Boolean} Show Default Option
 */
function initSelect2($el, data, allowClear = true, placeholder = "Select a Value", showDefaultOption = true) {
    debugger;
    if (showDefaultOption)
        data.unshift({ id: '', text: '' });

    $el.html('').select2({
        'data': data,
        'allowClear': allowClear,
        'placeholder': placeholder,
        'theme': "bootstrap"
    });
}

/**
 *
 * @param {Array} Array Data
 * @param {string} Filter By Array element
 */
function getMaxIdForArray(data, filterBy) {
    if (data.length == 0)
        return 1;
    if (!filterBy)
        throw "Filter By is required.";

    var maxId = Math.max.apply(Math, data.map(function (el) { return el[filterBy]; }));
    return ++maxId;
}

/**
 * Make button loading while request is processing
 * @param {HTMLButtonElement} $buttonEl - Button Element
 */
function setLoadingButton($buttonEl) {
    $buttonEl.prop("disabled", true);
    $buttonEl.html(`<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...`);
}

/**
 * Reset loading button and set to original state
 * @param {HTMLButtonElement} $buttonEl - Button Element
 * @param {originalText} - Original Butto Text
 */
function resetLoadingButton($buttonEl, originalText) {
    $buttonEl.html(originalText);
    $buttonEl.prop("disabled", false);
}

/**
 * Format date like (7:00 AM)
 * @param {any} date
 */
function formatDateTohhmmA(date) {
    return moment(date).format("hh:mm A");
}

function getChatDate(date) {
    return moment(date).format("MMM DD");
}

function getChatTime(date) {
    if (!date) date = new moment();
    var today = new moment();
    var diff = today.diff(date, "days");

    var datePart = "";
    if (diff === 0)
        datePart = "Today";
    else if (diff === 1)
        datePart = "Yesterday";
    else
        datePart = getChatDate(date);

    var timePart = formatDateTohhmmA(date);

    return `${timePart} | ${datePart}`;
}


function showValidationToast(errorObj) {
    var messages = "";

    for (var property in errorObj) {
        var thisPropertyErrors = errorObj[property];
        if (!thisPropertyErrors) continue;
        thisPropertyErrors.forEach(function (msg) {
            messages += messages ? `<br> ${msg}` : msg;
        });
    }

    $("#toast-container").remove();
    toastr.error(messages, "Please fix all validation errors!", {
        "positionClass": "toast-top-full-width",
        "closeButton": true,
        "escapeHtml": false
    });
}

// #region Validation
/**
 * Initialize validation on the container
 * @param {HTMLDivElement | HTMLFormElement} container - form container
 * @param {Array} constraints - Array of Constraints
 */
function initializeValidation(container, constraints) {
    extendValidation();
    var inputs = container.find("input[name], select[name]");
    for (var i = 0; i < inputs.length; ++i) {
        inputs[i].addEventListener("change", function (ev) {
            var errors = validate(container, constraints) || {};
            showErrorsForInput(this, errors[this.name])
        });
    }
}

/**
 * Validate the container
 * @param {HTMLDivElement | HTMLFormElement} container - form container
 * @param {Array} constraints - Array of Constraints
 */
function isValidForm(container, constraints) {
    hideValidationErrors(container);

    var errors = validate(container, constraints);

    if (errors) showErrors(container, errors || {});

    return errors;
}

/**
 * Show errors to the form container
 * Loops through all the inputs and show the errors for that input
 * @param {HTMLDivElement | HTMLFormElement} container
 * @param {Array} errors - Array of errors
 */
function showErrors(container, errors) {
    var inputs = container.find("input[name], select[name]");
    $.each(inputs, function (i, inputEl) {
        var thisPropertyErrors = errors[inputEl.name];
        if (!thisPropertyErrors) return;

        var errorMessage = "";
        thisPropertyErrors.forEach(function (msg) {
            errorMessage += errorMessage ? `<br> ${msg}` : msg;
        });

        showErrorsForInput(inputEl, errorMessage);
    });
}

/**
 * shows/hide validation error for the input
 * @param {HTMLInputElement} inputEl - HTMLInputElement
 * @param {string} error - Error message
 */
function showErrorsForInput(inputEl, error) {
    if (!error) {
        hideValidationErrorForInput(inputEl);
        return;
    }
    var formGroup = inputEl.closest('.form-group');
    inputEl.classList.remove("is-valid");
    inputEl.classList.add("is-invalid");
    $(formGroup).append(`<div class="invalid-feedback">${error}</div>`);
}

/**
 * Hides all validation error
 *  @param {HTMLDivElement | HTMLFormElement} container - form container
 */
function hideValidationErrors(container) {
    var inputs = container.find("input[name], select[name]");

    container.find('.invalid-feedback').remove();

    $.each(inputs, function (i, input) {
        input.classList.remove("is-invalid");
        input.classList.add("is-valid");
    });
}

function hideValidationErrorForInput(inputEl) {
    $(inputEl.closest(".form-group")).find(".invalid-feedback").remove()
    inputEl.classList.remove("is-invalid");
    inputEl.classList.add("is-valid");
}

function extendValidation() {
    // Before using it we must add the parse and format functions
    // Here is a sample implementation using moment.js
    validate.extend(validate.validators.datetime, {
        // The value is guaranteed not to be null or undefined but otherwise it
        // could be anything.
        parse: function (value, options) {
            return +moment.utc(value);
        },
        // Input is a unix timestamp
        format: function (value, options) {
            var format = options.dateOnly ? "YYYY-MM-DD" : "YYYY-MM-DD hh:mm:ss";
            return moment.utc(value).format(format);
        }
    });
}
// #endregion

// #region Polyfills
// https://tc39.github.io/ecma262/#sec-array.prototype.find
if (!Array.prototype.find) {
    Object.defineProperty(Array.prototype, 'find', {
        value: function (predicate) {
            // 1. Let O be ? ToObject(this value).
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If IsCallable(predicate) is false, throw a TypeError exception.
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }

            // 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
            var thisArg = arguments[1];

            // 5. Let k be 0.
            var k = 0;

            // 6. Repeat, while k < len
            while (k < len) {
                // a. Let Pk be ! ToString(k).
                // b. Let kValue be ? Get(O, Pk).
                // c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
                // d. If testResult is true, return kValue.
                var kValue = o[k];
                if (predicate.call(thisArg, kValue, k, o)) {
                    return kValue;
                }
                // e. Increase k by 1.
                k++;
            }

            // 7. Return undefined.
            return undefined;
        },
        configurable: true,
        writable: true
    });
}

// https://tc39.github.io/ecma262/#sec-array.prototype.findindex
if (!Array.prototype.findIndex) {
    Object.defineProperty(Array.prototype, 'findIndex', {
        value: function (predicate) {
            // 1. Let O be ? ToObject(this value).
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If IsCallable(predicate) is false, throw a TypeError exception.
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }

            // 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
            var thisArg = arguments[1];

            // 5. Let k be 0.
            var k = 0;

            // 6. Repeat, while k < len
            while (k < len) {
                // a. Let Pk be ! ToString(k).
                // b. Let kValue be ? Get(O, Pk).
                // c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
                // d. If testResult is true, return k.
                var kValue = o[k];
                if (predicate.call(thisArg, kValue, k, o)) {
                    return k;
                }
                // e. Increase k by 1.
                k++;
            }

            // 7. Return -1.
            return -1;
        },
        configurable: true,
        writable: true
    });
}

// Search substring in a string
if (!String.prototype.includes) {
    String.prototype.includes = function (search, start) {
        'use strict';
        if (typeof start !== 'number') {
            start = 0;
        }

        if (start + search.length > this.length) {
            return false;
        } else {
            return this.indexOf(search, start) !== -1;
        }
    };
}
// #endregion