$(function(){
    $('#OtpCode').inputmask({
        'mask': '99999',
        greedy: true,
        placeholder: "",
        autoUnmask: true
    });
    
    $('#ValidateOtpBtn').click(function(){
        var validateBtn = $(this);
        var otpForm = $(this).closest('form');
        console.log('about to validate otp');
        otpForm.validate({
            rules: {
                OtpCode: {
                    required: true,
                    rangelength: [5, 5],
                    digits: true
                }
            },
            messages: {
                OtpCode: {
                    required: 'Otp is required',
                    rangelength: 'Invalid OTP',
                    digits: 'Invalid OTP'
                }
            }
        });
        
        if(!otpForm.valid()){
            return;
        }

        validateBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        otpForm.ajaxSubmit({
            data: {otp: $('#OtpCode').inputmask('unmaskedvalue')},
            success: function(response, status, xhr, $form){
                $('#BankAccountTransferDiv').html(response);
            },
            error: function(jqXHR, textStatus, err){
                swal('Oops', 'There was an error processing your request', 'error');
            },
            complete: function(jqXHR, textStatus){
                console.log('this is complete');
                validateBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });
});