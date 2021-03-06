﻿$(document).ready(function() {
    $(".select2-init").each(function() {
        $(this).select2({
            ajax: {
                method: 'POST',
                url: '/api/v1/places/getall/select2',
                dataType: 'json',
                delay: 250,
                processResults: function(data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.items,
                        pagination: {
                            more: data.items.length >= 30
                        }
                    }
                },
                cache: true
            },
            minimumInputLength: 1
        });
        $(this).next('.select2').find('.select2-selection').one('focus', select2Focus).on('blur',
            function() {
                $(this).one('focus', select2Focus);
            });

        function select2Focus() {
            $(this).closest('.select2').prev('select').select2('open');
        }
    });
});

function addNestedForm(container, counter, ticks, content) {
    var nextIndex = $(counter).length;
    var pattern = new RegExp(ticks, "gi");
    content = content.replace(pattern, nextIndex);
    $(container).append(content);
}
