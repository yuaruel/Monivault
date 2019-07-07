$(function(){
    $('#PayTaxBtn').click(function(){
        if ($('#TaxIdentificationNumber').length) {
            //Validate tax payment form.
            var paymentBtn = $(this);
            var paymentForm = $('#TaxPaymentForm');

            paymentForm.validate({
                rules: {
                    Period: 'required',
                    TaxAmount: 'required'
                },
                messages: {
                    Period: 'Select the tax period you are paying for',
                    TaxAmount: 'Tax amount is required'
                }
            });

            if (!paymentForm.valid()) {
                return;
            }

            paymentBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
            console.log('the tax period: ' + $('#TaxPeriod').datepicker('getUTCDate'));
            var momentTime = moment($('#TaxPeriod').datepicker('getUTCDate'));
            console.log('iso date from moment: ' + momentTime.toISOString());
            paymentForm.ajaxSubmit({
                data: { taxPeriod: momentTime.toISOString(), amount: $('#Amount').autoNumeric('get') },
                success: function (response, status, xhr, $form) {
                    //Move to the next form for Personal details
                    $('#_TaxPaymentModal').modal('hide');
                    taxPaymentTable.ajax.reload();
                    swal("Success", "Your tax payment is successful", "success");
                    
                },
                error: function (jqXHR, textStatus, err) {
                    var respObj = JSON.parse(jqXHR.responseText);
                    swal("Oops", "There was an error making your tax payment", "error");
                },
                complete: function (jqXHR, textStatus) {
                    paymentBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                }
            });
        } else {
            console.log('tax identification did not indicate null');
        }
    });
});