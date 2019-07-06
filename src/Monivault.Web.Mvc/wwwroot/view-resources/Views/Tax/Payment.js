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

    var taxPaymentTable = $('#TaxPayments').DataTable();

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
});