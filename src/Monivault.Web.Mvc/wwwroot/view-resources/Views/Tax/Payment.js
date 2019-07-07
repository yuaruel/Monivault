$(function () {

    var arrows;
    if (mUtil.isRTL()) {
        arrows = {
            leftArrow: '<i class="la la-angle-right"></i>',
            rightArrow: '<i class="la la-angle-left"></i>'
        }
    } else {
        arrows = {
            leftArrow: '<i class="la la-angle-left"></i>',
            rightArrow: '<i class="la la-angle-right"></i>'
        }
    }

    var taxPaymentTable = $('#TaxPayments').DataTable({
        ajax: {
            url: abp.appPath + 'Tax/GetTaxPaymentList',
            dataSrc: 'result'
        },
        columns: [
            { data: 'taxType' },
            {
                data: 'amount',
                render: function (data, type, full, meta) {
                    return 'N' + $.number(data, 2);
                }
            },
            {
                data: 'taxPeriod',
                render: function (data, type, full, meta) {
                    return moment(data).format('D/M/YYYY');
                }
            },
            {
                data: 'datePaid',
                render: function (data, type, full, meta) {
                    return moment(data).format('D/M/YYYY');
                }
            }
        ]
    });

    $('#OpenTaxPaymentFormBtn').click(function (e) {
        e.preventDefault();
        mApp.blockPage({
            overlayColor: '#000000',
            type: 'loader',
            state: 'success',
            message: 'Please wait...'
        });
        $.ajax({
            url: abp.appPath + 'Tax/TaxPaymentModal',
            type: 'GET',
            contentType: 'application/html',
            success: function (content) {
                mApp.unblockPage();
                $('#_TaxPaymentModal div.modal-body').html(content);
                $('#_TaxPaymentModal').modal('show');

                $('.m_selectpicker').selectpicker();
                $('#TaxPeriod').datepicker({
                    rtl: mUtil.isRTL(),
                    todayBtn: "linked",
                    clearBtn: true,
                    todayHighlight: true,
                    templates: arrows
                });
                $('#Amount').autoNumeric('init', { currencySymbol: '' });

            },
            error: function (e) { }

        });
    });

    $('#PayTaxBtn').click(function () {
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