$(function () {
    $.validator.addMethod("networkProvider", function (value, element, params) {
        //The value passed in, is same with the realVal processed below. I just felt like using the processed realVal.
        console.log('sent value: ' + value);
        var realVal = $(element).selectpicker('val');
        console.log('netowrk provider value: ' + realVal);
        return realVal.length >= 3;
    }, "Select a network");

    $('.m_selectpicker').selectpicker();

    $('#SubmitAirtimePurchaseFormBtn').click(function () {

        var purchaseFormBtn = $(this);
        var purchaseForm = $(this).closest('form');

        purchaseForm.validate({
            rules: {
                AirtimeNetwork: {
                    required: true,
                    networkProvider: true
                },
                PhoneNumber: {
                    required: true,
                    digits: true,
                    minlength: 11,
                    maxlength: 11
                },
                Amount: {
                    required: true,
                    min: 50,
                    number: true
                }
            },
            messages: {
                AirtimeNetwork: {
                    required: 'Select a network',
                    networkProvider: 'Select a network'
                },
                PhoneNumber: {
                    required: 'Your phone number is required',
                    digits: 'Enter a valid phone number',
                    minlength: 'Enter a valid phone number',
                    maxlength: 'Enter a valid phone number'
                },
                Amount: {
                    required: 'Enter amount',
                    min: 'The minimum airtime is N50',
                    number: 'Enter valid amount'
                }
            }
        });

        if (!purchaseForm.valid()) {
            return;
        }

        purchaseFormBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        purchaseForm.ajaxSubmit({
            success: function (responseData, textStatus, jqXHR) {
                swal('Success', 'Your airtime purchase is successful', 'success');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var responseErr = JSON.parse(jqXHR.responseText);
                swal('Purchase failure', responseErr.error.message, 'error');//responseErr);
            },
            complete: function (jqXHR, textStatus) {
                purchaseFormBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });
});