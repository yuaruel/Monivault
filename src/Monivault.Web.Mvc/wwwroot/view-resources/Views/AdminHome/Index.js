$(function(){
    abp.ajax({
        url: 'AdminHome/TotalAccountHolders',
        abpHandleError: false
    }).done((data) => {
        $('#totalAccountHolders').text(data.totalAccountHolders);
        console.log('accountholder count: ' + data.totalAccountHolders);
    }).fail((data) => {
        console.log('this call failed');
    });

    //Get total credit and debit
    abp.ajax({
        url: 'AdminHome/TotalCreditAndDebit',
        abpHandleError: false
    }).done((data) => {
        $('#totalCredit').text('N' + $.number(data.totalCredit, 2));
        $('#totalDebit').text('N' + $.number(data.totalDebit, 2));
    }).fail((data) => {
        console.log('this call failed');
    });
});
