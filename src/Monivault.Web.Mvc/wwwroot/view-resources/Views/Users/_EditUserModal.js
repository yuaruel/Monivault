$(function(){
    $('#UpdateUserBtn').click(function(){
        console.log('update user...');
        var userBtn = $(this);
        var userForm = $('#UserForm');

        userForm.validate({
            rules: {
                UserName: 'required',
                FirstName: 'required',
                LastName: 'required',
                PhoneNumber: 'required'
            },
            messages: {
                UserName: 'Usewr name is required',
                FirstName: 'First name is required',
                LastName: 'Last name is required',
                PhoneNumber: 'Phone number is required'
            }
        });

        if(!userForm.valid()){
            return;
        }

        var _$roleCheckboxes = $("input[name='role']:checked");
        if(_$roleCheckboxes.length < 1){
            swal('Roles', 'Select at least one role', 'error');
            return;
        }

        userBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        var name = $('#FirstName').val();
        var surname = $('#LastName').val();
        var roles = [];
        var enabled = false;

        if($("input[name='Enabled']:checked")){
            enabled = true;
        }

        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                console.log('role value: ' + _$roleCheckbox.val());
                roles.push(_$roleCheckbox.val());
            }
        }

        userForm.ajaxSubmit({

            data: {Name: name, Surname: surname, roleNames: roles, isActive: enabled},
            success: function(response, status, xhr, $form){
                //Move to the next form for Personal details
                $('#_EditUserModal').modal('hide');
            },
            error: function(jqXHR, textStatus, err){
                var respObj = JSON.parse(jqXHR.responseText);
                swal("Oops", respObj.error.message, "error");
            },
            complete: function(jqXHR, textStatus){
                userBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    }); 
});
/*
(function ($) {

    var _userService = abp.services.app.user;
    var _$modal = $('#UserEditModal');
    var _$form = $('form[name=UserEditForm]');

    function save() {

        if (!_$form.valid()) {
            return;
        }

        var user = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js
        user.roleNames = [];
        var _$roleCheckboxes = $("input[name='role']:checked");
        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                user.roleNames.push(_$roleCheckbox.val());
            }
        }

        abp.ui.setBusy(_$form);
        _userService.update(user).done(function () {
            _$modal.modal('hide');
            location.reload(true); //reload page to see edited user!
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    }

    //Handle save button click
    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    //Handle enter key
    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    $.AdminBSB.input.activate(_$form);

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);*/
