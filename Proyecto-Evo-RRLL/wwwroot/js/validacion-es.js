/* SC-008: mensajes de jquery.validate en español (los que no vienen del servidor). */
(function ($) {
    if (!$ || !$.validator) return;
    $.extend($.validator.messages, {
        required: 'Este campo es obligatorio.',
        email: 'Ingrese un correo electrónico válido.',
        url: 'Ingrese una URL válida.',
        date: 'Ingrese una fecha válida.',
        dateISO: 'Ingrese una fecha válida (AAAA-MM-DD).',
        number: 'Ingrese un número.',
        digits: 'Ingrese solo dígitos.',
        equalTo: 'Ingrese el mismo valor otra vez.',
        maxlength: $.validator.format('Máximo {0} caracteres.'),
        minlength: $.validator.format('Mínimo {0} caracteres.'),
        rangelength: $.validator.format('Ingrese entre {0} y {1} caracteres.'),
        range: $.validator.format('Ingrese un valor entre {0} y {1}.'),
        max: $.validator.format('Ingrese un valor menor o igual a {0}.'),
        min: $.validator.format('Ingrese un valor mayor o igual a {0}.'),
        step: $.validator.format('Ingrese un múltiplo de {0}.')
    });
})(window.jQuery);
