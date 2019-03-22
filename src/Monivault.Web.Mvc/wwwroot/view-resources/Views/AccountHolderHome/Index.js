$(function(){
    abp.ajax({
        url: 'AccountHolderDashboardService/AvailableBalance',
        method: 'GET'
    }).done(function(data){
        $('#availableBalance').text('N' + $.number(data.availableBalance, 2));
    }).fail(function(data){
        console.log(data)
    }).always();
});