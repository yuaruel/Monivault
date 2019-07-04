$(function () {
    $('#UpdateAccountHolderBtn').click(function () {
        var editBtn = $(this);
        var editAccountHolderForm = $('#EditAccountHolderForm');

        editBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        editAccountHolderForm.ajaxSubmit({

            data: { AccountOfficer: $('#AccountOfficer').selectpicker('val') },
            success: function (response, status, xhr, $form) {
                //Move to the next form for Personal details
                console.log('returned status: ' + status);
                $('#_EditAccountHolderModal').modal('hide');
            },
            error: function (jqXHR, textStatus, err) {
                var respObj = JSON.parse(jqXHR.responseText);
                swal("Oops", respObj.error.message, "error");
            },
            complete: function (jqXHR, textStatus) {
                editBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });
});