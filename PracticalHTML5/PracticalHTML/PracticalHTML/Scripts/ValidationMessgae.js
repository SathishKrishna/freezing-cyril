//creates a container for validation rules
var rulesName = new Array();

    $(function () {

        alert('document ready');
    debugger;
    //iterate and get the rules name in array variable name
    $('.vmsg').each(function (index, element) {
        if (element.className.indexOf(" ") != -1) {
            rulesName[index] = element.className.split(" ")[0];
        }
    });

    alert(rulesName[0]);
    alert(rulesName[1]);
    alert(rulesName[2]);
    alert(rulesName[3]);
    $(':input:not(:button)').each(function () {
        this.oninvalid = validationFail;
        this.onblur = validate;
    });

    document.querySelector('#checkValidation').onclick = validate;


});

function validate() {
    alert('validate called');
    $('.vmsg').addclass("none");
    document.forms[0].checkValidity();
}

function validationFail(e) {
    alert('validation failed called');
    var element = e.srcElement;
    var validity = element.validity;
    var id = element.id;

    if (!validity.valid) {
        for (var i = 0; i < rulesName.length; i++) {
            checkRule(validity, rulesName[i], id);
        }
        e.preventDefault();

    }
}

function checkRule(validity, ruleName, id) {
    alert('Check Rule called');
    if (eval("validity." + ruleName)) {
        $("#" + id + "Rules." + ruleName).removeClass("none");
    }
}
