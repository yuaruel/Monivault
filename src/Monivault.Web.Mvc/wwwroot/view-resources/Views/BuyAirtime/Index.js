$(function () {
    $('.m_selectpicker').selectpicker();

    $('#SubmitAirtimePurchaseFormBtn').click(function () {

        var purchaseFormBtn = $(this);
        var purchaseForm = $(this).closest('form');

        purchaseForm.validate({
            rules: {
                AirtimeNetwork: {
                    required: true,
                    minlength: 3
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
                    minlength: 'Select a network'
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
                console.log(responseData);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus);
            },
            complete: function (jqXHR, textStatus) {
                purchaseFormBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });
});