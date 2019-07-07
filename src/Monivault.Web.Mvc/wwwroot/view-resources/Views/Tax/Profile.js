$(function () {
    $('#UpdateTaxProfileBtn').click(function () {
        var updateBtn = $(this);
        var profileForm = $('#TaxProfileForm');

        profileForm.validate({
            rules: {
                Tin: 'required',
                FullName: 'required'
            },
            messages: {
                Tin: 'Tax Identification Number is required',
                FullName: 'Full name is required'
            }
        });

        if (!profileForm.valid()) {
            return;
        }

        updateBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        profileForm.ajaxSubmit({

            success: function (response, status, xhr, $form) {
                //Move to the next form for Personal details
                swal("Success", "Your tax profile update is successful", "success");
            },
            error: function (jqXHR, textStatus, err) {
                var respObj = JSON.parse(jqXHR.responseText);
                swal("Oops", "There was an error updating your tax profile", "error");
            },
            complete: function (jqXHR, textStatus) {
                updateBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });
});