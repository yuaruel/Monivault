$(function(){
    $('[data-switch=true]').bootstrapSwitch();
    $('#InterestStatus').bootstrapSwitch({
        onText: 'Running',
        offText: 'Stopped',
        onSwitchChange: function(event, state){
            console.log('about to check switch: ' + state);
            if(state){
                enableInterestSettingForm();
                console.log('enable interest setting form');
            }else{
                disableInterestSettingForm();
                console.log('disable interest setting form');
            }
        }
    });
    $('#InterestRate').TouchSpin({
       buttondown_class: 'btn btn-secondary',
       buttonup_class: 'btn btn-secondary',
       
       min: 0,
       max: 100,
       step: 0.5,
       postfix: '% per yr' 
    });
    $('#InterestDuration').TouchSpin({
        buttondown_class: 'btn btn-secondary',
        buttonup_class: 'btn btn-secondary',

        min: 0,
        max: 100,
        step: 1,
        postfix: 'Day(s)'
    });
    $('#PenaltyDeduction').TouchSpin({
        buttondown_class: 'btn btn-secondary',
        buttonup_class: 'btn btn-secondary',

        min: 0,
        max: 100,
        step: 0.5,
        postfix: '%'
    });
    
    function disableInterestSettingForm(){
        $('input[name=InterestType]').prop('disabled', true);
        $('#InterestRate').prop('disabled', true);
        $('#InterestDuration').prop('disabled', true);
        $('#PenaltyDeduction').prop('disabled', true);
    }
    
    function enableInterestSettingForm(){
        $('input[name=InterestType]').prop('disabled', false);
        $('#InterestRate').prop('disabled', false);
        $('#InterestDuration').prop('disabled', false);
        $('#PenaltyDeduction').prop('disabled', false);
    }
    
    $('#UpdateGeneralSettingsBtn').click(function(){
        var generalBtn = $(this);
        var generalForm = $(this).closest('form');
    });
    
    $('#UpdateWithdrawalSettingsBtn').click(function(){
        var withdrawalBtn = $(this);
        var withdrawalForm = $(this).closest('form');
    });
    
    $('#UpdateInterestSettingsBtn').click(function(){
        var interestBtn = $(this);
        var interestForm = $(this).closest('form');
    });
});
