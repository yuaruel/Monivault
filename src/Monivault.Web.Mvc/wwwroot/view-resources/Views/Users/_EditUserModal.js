$(function(){
    $('#UpdateUserBtn').click(function(){

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
