$(function(){
    abp.ajax({
        url: 'AdminHome/TotalAccountHolders',
        abpHandleError: false
    }).done((data) => {
        $('#totalAccountHolders').text(2)
        console.log('accountholder count: ' + data.totalAccountHolders);
    }).fail((data) => {
        console.log('this call failed');
    });
});
