$(function(){
    //One Card Pin Validator
    $.validator.addMethod("oneCardPin", function (value, element, params) {
        //The value passed in, is same with the realVal processed below. I just felt like using the processed realVal.
        var realVal = $(element).inputmask('unmaskedvalue');

        return (realVal.length === 12) && (/^\d+$/.test( realVal ));
    }, "Invalid Pin");
    
    $('#CardPin').inputmask({
        'mask': '9999-9999-9999[-9999]',
        greedy: false,
        placeholder: "",
        autoUnmask: true
    });
    
    $('#SubmitOneCardPinBtn').click(function(){
        console.log('validate Pin form...');
        var submitBtn = $(this);
        var pinForm = $(this).closest('form');
        
        pinForm.validate({
            rules: {
                CardPin: {
                    oneCardPin: true
                }
            }
        });
        
        if(!pinForm.valid()){
            return;
        }

        submitBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        var pinValue = $('#CardPin').inputmask('unmaskedvalue');

        abp.ajax({
            url: pinForm.attr('action'),
            data: JSON.stringify({pin: pinValue}),
            abpHandleError: false
        }).done(function(data){
            swal('')
        }).fail(function(data){
            console.log('an error occurred');
            console.log(JSON.stringify(data));
        }).always(function(){
            submitBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        })
    });
});