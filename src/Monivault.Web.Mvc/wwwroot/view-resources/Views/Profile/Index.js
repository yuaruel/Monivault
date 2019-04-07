$(function(){
    $.validator.addMethod("bankName", function (value, element, params) {
        //The value passed in, is same with the realVal processed below. I just felt like using the processed realVal.
        var bankVal = $(element).selectpicker('val');

        return bankVal !== '-1';
    }, "Select bank");
    
    $('.m_selectpicker').selectpicker();
    
    Dropzone.options.mDropzoneOne = {
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 1,
        maxFilesize: 5, // MB
        addRemoveLinks: true,
        accept: function(file, done) {
            if (file.name == "justinbieber.jpg") {
                done("Naha, you don't.");
            } else {
                done();
            }
        }
    };
    
    $('#UpdateBankAccountBtn').click(function(){
        var updateBtn = $(this);
        var bankAccountForm = $(this).closest('form');
        
        bankAccountForm.validate({
            ignore: [],
            rules: {
                BankName: {
                    required: true,
                    bankName: true
                },
                AccountNumber: {
                    required: true,
                    minlength: 10
                },
                AccountName: {
                    required: true,
                    minlength: 5
                }
            },
            messages: {
                BankName: 'Select bank',
                AccountNumber: {
                    required: 'Account number is required',
                    minlength: 'Invalid account number'
                },
                AccountName: 'Account name is required'
            }
        });
        
        if(!bankAccountForm.valid()){
            return;
        }

        updateBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        abp.ajax({
            url: bankAccountForm.attr('action'),
            data: JSON.stringify({bankName: $('#BankName').selectpicker('val'), accountNumber: $('#AccountNumber').val(), accountName: $('#AccountName').val()}),
            abpHandleError: false
        }).done(function(data){
            swal('Success', 'Account detail updated', 'success');
        }).fail(function(data){
            console.log(data);
            swal('Oops', data.message, 'error');
        }).always(function(){
            updateBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        })
    });
});