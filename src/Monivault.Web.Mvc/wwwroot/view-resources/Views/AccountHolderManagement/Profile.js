$(function () {
    $('.m_selectpicker').selectpicker();

    var transactionsTable = $('#Transactions').DataTable({
        ajax: {
            url: abp.appPath + 'Transaction/AccountHolderTransactions/' + accountHolderKey,
            dataSrc: 'result'
        },
        columns: [
            {
                data: 'amount',
                render: function (data, type, full, meta) {
                    if (full.transactionType === 'Credit') {
                        return '<span class="m--font-success">' + 'N' + $.number(data, 2) + '</span>';
                    } else {
                        return '<span class="m--font-danger">' + 'N' + $.number(data, 2) + '</span>';
                    }
                }
            },
            { data: 'transactionType' },
            { data: 'description' },
            {
                data: 'creationTime',
                render: function (data, type, full, meta) {
                    return moment(data).format('DD-MM-YYYY h:mm:ssa');
                }
            },
            {
                data: 'balanceAfterTransaction',
                render: function (data, type, full, meta) {
                    return 'N' + $.number(data, 2);
                }
            }
        ]
    });

    $('#EditAccountHolder').click(function (e) {
        e.preventDefault();
        mApp.blockPage({
            overlayColor: '#000000',
            type: 'loader',
            state: 'success',
            message: 'Please wait...'
        });
        $.ajax({
            url: abp.appPath + 'AccountHolderManagement/EditAccountHolderModal?accountHolderKey=' + accountHolderKey,
            type: 'GET',
            contentType: 'application/html',
            success: function (content) {
                mApp.unblockPage();
                $('#_EditAccountHolderModal div.modal-body').html(content);
                $('#_EditAccountHolderModal').modal('show');

                $('.m_selectpicker').selectpicker();
            },
            error: function (e) {}

        });
        $('#EditAccountHolderModal').modal('show');
    });
});