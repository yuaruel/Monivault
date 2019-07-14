$(function () {
    $.validator.addMethod("bankName", function (value, element, params) {
        //The value passed in, is same with the realVal processed below. I just felt like using the processed realVal.
        var bankVal = $(element).selectpicker('val');

        return bankVal !== '-1';
    }, "Select bank");

    $('.m_selectpicker').selectpicker();

    $('#UpdateBankAccountBtn').click(function () {
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

        if (!bankAccountForm.valid()) {
            return;
        }

        updateBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        abp.ajax({
            url: bankAccountForm.attr('action'),
            data: JSON.stringify({ bankName: $('#BankName').selectpicker('val'), accountNumber: $('#AccountNumber').val(), accountName: $('#AccountName').val() }),
            abpHandleError: false
        }).done(function (data) {
            swal('Success', 'Account detail updated', 'success');
        }).fail(function (data) {
            console.log(data);
            swal('Oops', data.message, 'error');
        }).always(function () {
            updateBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        })
    });

    $('#UpdatePersonalDetailBtn').click(function () {
        var updateBtn = $(this);
        var personalDetailForm = $(this).closest('form');

        personalDetailForm.validate({
            ignore: [],
            rules: {
                FirstName: 'required',
                LastName: 'required',
                PhoneNumber: {
                    required: true,
                    minlength: 11
                }
            },
            messages: {
                FirstName: 'First name is required',
                LastName: 'Last name is required',
                PhoneNumber: {
                    required: 'Phone number is required',
                    minlength: 'Please enter a valid GSM number'
                }
            }
        });

        if (!personalDetailForm.valid()) {
            return;
        }

        updateBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        personalDetailForm.ajaxSubmit({
            success: function (response, status, xhr, $form) {
                //Move to the next form for Personal details
                swal('Success', 'Personal update was successful', 'success');
            },
            error: function (jqXHR, textStatus, err) {
                var respObj = JSON.parse(jqXHR.responseText);
                swal("Oops", respObj.error.message, "Signin error");
            },
            complete: function (jqXHR, textStatus) {
                updateBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });

    $('#UpdatePasswordBtn').click(function () {
        var updateBtn = $(this);
        var updatePasswordForm = $(this).closest('form');

        updatePasswordForm.validate({
            ignore: [],
            rules: {
                CurrentPassword: 'required',
                NewPassword: {
                    required: true,
                    minlength: 8
                },
                ConfirmPassword: {
                    equalTo: '#NewPassword'
                }
            },
            messages: {
                CurrentPassword: 'Your current password is required',
                NewPassword: {
                    required: 'Your new password is required',
                    minlength: 'Your password should be minimum of 8 characters'
                },
                ConfirmPassword: 'Does not match with your new password'
            }
        });
        console.log('validation object assigned');

        if (!updatePasswordForm.valid()) {
            return;
        }

        updateBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        updatePasswordForm.ajaxSubmit({
            success: function (response, status, xhr, $form) {
                swal('Success', 'Password update was successful', 'success');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal("Oops", jqXHR.responseText, "error");
            },
            complete: function (jqXHR, textStatus) {
                updateBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });

    $('#updateProfilePictureBtn').click(function () {

        var profilePictureForm = $(this).closest('form');

        profilePictureForm.validate({
            rules: {
                profilePicture: 'required'
            },
            messages: {
                profilePicture: 'Select a picture'
            }
        });

        if (!profilePictureForm.valid()) {
            return;
        }

        var attachedFile = $('#profilePicture')[0].files[0];
        if (attachedFile.type != "image/png") {
            swal('Oops', 'Your display picture should be a png file', 'error');
        } else {
            mApp.block('#profileDetailTab', {
                overlayColor: '#000000',
                type: 'loader',
                state: 'primary',
                message: 'Please wait...'
            });

            var fileData = new FormData();
            fileData.append("imageFile", attachedFile);

            $.ajax({
                url: abp.appPath + 'Profile/UploadProfilePicture',
                method: 'POST',
                data: fileData,
                processData: false,
                contentType: false,
                success: function (data, textStatus, jqXhr) {

                },
                error: function (jqXHR, textStatus, err) {
                    swal('Oops', 'There was an error completing your upload', 'error');
                },
                complete: function (jqXHR, textStatus) {
                    mApp.unblock('#profileDetailTab');
                }
            });
        }
    });
});