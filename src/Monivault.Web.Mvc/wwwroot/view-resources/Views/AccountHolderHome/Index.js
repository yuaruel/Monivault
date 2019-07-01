$(function(){
    abp.ajax({
        url: 'AccountHolderDashboardService/AvailableBalanceAndReceivedInterest',
        method: 'GET'
    }).done(function(data){
        $('#availableBalance').text('N' + $.number(data.availableBalance, 2));
        $('#interestReceived').text('N' + $.number(data.receivedInterest, 2));
    }).fail(function(data){
        console.log(data)
    }).always();
    
/*    $('#RecentTransactionsTable').DataTable({
        ajax: {
            url: abp.appPath + 'AccountHolderHome/RecentTransactions',
            dataSrc: 'result'
        },
        paging: false,
        ordering: false,
        info: false,
        columns: [
            {
                data: 'amount',
                render: function(data, type, full, meta){
                    if(full.transactionType === 'Credit'){
                        return '<span class="m--font-success">' + 'N' + $.number(data, 2) + '</span>';
                    }else{
                        return '<span class="m--font-danger">' + 'N' + $.number(data, 2) + '</span>';
                    }
                }
            },
            {data: 'transactionType'},
            {data: 'description'},
            {data: 'creationTime'},
            {
                data: 'balanceAfterTransaction',
                render: function(data, type, full, meta){
                    return 'N' + $.number(data, 2);
                }
            }
        ]
    });*/
    $('.m_datatable').mDatatable({
        data: {
            type: 'remote',
            source: {
                read: {
                    url: abp.appPath + 'AccountHolderHome/RecentTransactions',
                    map: function(raw) {
                        console.log('raw data: ' + JSON.stringify(raw.result));
                        // sample data mapping
                        var dataSet = raw;
                        if (typeof raw.result !== 'undefined') {
                            console.log('result is defined');
                            dataSet = raw.result;
                        }
                        return dataSet;
                    }
                }
            }
        },
        sortable: false,
        pagination: false,
        columns: [
            {
                field: 'amount',
                title: 'Amount',
                width: 100,
                template: function(row){
                    if(row.transactionType === 'Credit'){
                        return '<span class="m--font-success">' + 'N' + $.number(row.amount, 2) + '</span>';
                    }else{
                        return '<span class="m--font-danger">' + 'N' + $.number(row.amount, 2) + '</span>';
                    }
                }
            },
            {
                title: 'Type',
                field: 'transactionType',
                width: 100
            },
            {
                field: 'description',
                title: 'Description',
                width: 300
            },
            {
                field: 'creationTime',
                title: 'Transaction Date',
                width: 150,
                template: function(row){
                    return moment(row.creationTime).format('DD-MM-YYYY h:mm:ssa');
                }
            },
            {
                field: 'balanceAfterTransaction',
                title: 'Balance',
                width: 100,
                template: function(row){
                    return 'N' + $.number(row.balanceAfterTransaction, 2);
                }
            }
        ]
    });
});