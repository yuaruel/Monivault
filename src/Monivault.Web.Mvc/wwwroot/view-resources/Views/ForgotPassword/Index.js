$(function () {
    $('#requestPasswordResetBtn').click(function () {
        var requestBtn = $(this);
        var requestForm = $(this).closest('form');

        requestForm.validate({
            rules: {
                EmailOrPhoneNumber: {
                    required: true,
                    email: true
                }
            },
            messages: {
                EmailOrPhoneNumber: {
                    required: 'Please enter your email',
                    email: 'Please enter a valid email'
                }
            }
        });

        if (!requestForm.valid()) {
            return;
        }

        requestBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        requestForm.submit();
    });
});