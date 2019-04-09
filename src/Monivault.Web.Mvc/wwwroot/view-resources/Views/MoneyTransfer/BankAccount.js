$(function(){
    console.log('current balance: ' + currentBalance);
    $.validator.addMethod("withinBalance", function (value, element) {
        var deductableAmount = parseInt($(element).autoNumeric('get'));

        return deductableAmount <= currentBalance;
    }, "Insufficient fund to complete request");

    $.validator.addMethod("minimumTransfer", function (value, element) {
        var transferAmount = parseInt($(element).autoNumeric('get'));

        return transferAmount >= 1000;
    }, "Minimum transfer is N1,000");

    $.validator.addMethod('amountMultiple', function (value, element) {
        var transferAmount = parseInt($(element).autoNumeric('get'));

        return (transferAmount % 1000) === 0;
    }, "Amount must be multiple of N1,000");

    $('#TransferAmount').autoNumeric('init', {currencySymbol: ''});
    
    $('#SubmitBankTransferRequestBtn').click(function(){
        var submitBtn = $(this);
        var bankAccountTransferForm = $(this).closest('form');
        
        bankAccountTransferForm.validate({
            rules: {
                TransferAmount: {
                    required: true,
                    withinBalance: true,
                    minimumTransfer: true
                }
            },
            messages: {
                TransferAmount: {
                    required: 'Amount is required'
                }
            }
        });
        
        if(!bankAccountTransferForm.valid()){
            return;
        }

        submitBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        bankAccountTransferForm.ajaxSubmit({
            data: {amount: $('#TransferAmount').autoNumeric('get')},
            success: function(response, status, xhr, $form){
                $('#BankAccountTransferDiv').html(response);
            },
            error: function(jqXHR, textStatus, err){
                swal('Oops', 'There was an error processing your request', 'error');
            },
            complete: function(jqXHR, textStatus){
                console.log('this is complete');
                submitBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
/*        abp.ajax({
            url: abp.appPath + 'MoneyTransfer/GetBankTransferOtpForm',
            data: JSON.stringify({amount: $('#Amount').autoNumeric('get'), comment: $('#Comment').val()}),
            abpHandleError: false
        }).done(function(data){
            $('#BankAccountTransferDiv').html(data)
        }).fail(function(data){
            swal('Oops', data.message, 'error');
        }).always(function(){
            submitBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        });*/
    });
});