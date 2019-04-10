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
       decimals: 2,
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
        decimals: 2,
        postfix: '%'
    });
    
    if(!$('#InterestStatus').bootstrapSwitch('state')){
        disableInterestSettingForm();
    }
    
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
        
        var stopDeposit = $('#StopDeposit').bootstrapSwitch('state');
        var stopSignUp = $('#StopSignUp').bootstrapSwitch('state');

        generalBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        abp.ajax({
            url: abp.appPath + 'Settings/UpdateGeneralSettings',
            data: JSON.stringify({ StopDeposit: stopDeposit, StopSignUp: stopSignUp })
        }).done(function(data){
            swal('', 'Update was successful!', 'success');
        }).fail(function(data){
            swal('Oops', 'There was an error completing the update', 'error');
        }).always(function(){
            generalBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        });
    });
    
    $('#UpdateWithdrawalSettingsBtn').click(function(){
        var withdrawalBtn = $(this);
        
        var stopWithdrawal = $('#StopWithdrawal').bootstrapSwitch('state');
        var withdrawalServiceCharge = $('#WithdrawalServiceCharge').val();
        
        console.log('service charge: ' + withdrawalServiceCharge);

        withdrawalBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        abp.ajax({
            url: abp.appPath + 'Settings/UpdateWithdrawalSettings',
            data: JSON.stringify({ StopWithdrawal: stopWithdrawal, WithdrawalServiceCharge: withdrawalServiceCharge })
        }).done(function(data){
            swal('', 'Update was successful!', 'success');
        }).fail(function(data){
            swal('Oops', 'There was an error completing the update', 'error');
        }).always(function(){
            withdrawalBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        });
    });
    
    $('#UpdateInterestSettingsBtn').click(function(){
        var interestBtn = $(this);

        var interestRunning = $('#InterestStatus').bootstrapSwitch('state');
        var interestType = $('input[name=InterestType]:checked').val();
        var interestRate = $('#InterestRate').val();
        var interestDuration = $('#InterestDuration').val();
        var penaltyDeduction = $('#PenaltyDeduction').val();

        console.log('interest status: ' + interestRunning);
        console.log('interestType: ' + interestType);

        interestBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        abp.ajax({
            url: abp.appPath + 'Settings/UpdateInterestSettings',
            data: JSON.stringify({ InterestStatus: interestRunning, InterestType: interestType, InterestRate: interestRate, 
                                            InterestDuration: interestDuration, PenaltyDeduction: penaltyDeduction })
        }).done(function(data){
            swal('', 'Update was successful!', 'success');
        }).fail(function(data){
            swal('Oops', 'There was an error completing the update', 'error');
        }).always(function(){
            interestBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        });
    });
});
