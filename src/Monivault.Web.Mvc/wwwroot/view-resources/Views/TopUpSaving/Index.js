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

    $('input[name=TopUpOption]').change(function(){
        var topUpOption = $(this).val();
        var oneCardFormDiv = $('#OneCardFormDiv');
        var debitCardFormDiv = $('#DebitCardFormDiv');

        if (topUpOption == 'OneCard') {
            oneCardFormDiv.removeClass('m--hide');
            debitCardFormDiv.addClass('m--hide');
        } else if (topUpOption == 'DebitCard') {
            debitCardFormDiv.removeClass('m--hide');
            oneCardFormDiv.addClass('m--hide');
        }
    });
    
    $('#SubmitOneCardPinBtn').click(function(){
 
        var submitBtn = $(this);
        var pinForm = $(this).closest('form');
        
        pinForm.validate({
            rules: {
                CardPin: {
                    oneCardPin: true
                },
                Comment: {
                    maxlength: 150
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
            data: JSON.stringify({pin: pinValue, comment: $('#Comment').val()}),
            abpHandleError: false
        }).done(function(data){
            swal('Sucess', 'Pin Redeemed', 'success');
        }).fail(function(data){
            console.log(data);
            swal('Oops', data.message, 'error');
        }).always(function(){
            submitBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        })
    });
});