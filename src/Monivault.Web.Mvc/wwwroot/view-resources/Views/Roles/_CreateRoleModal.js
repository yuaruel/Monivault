$(function(){
    
    $('#SaveRoleBtn').click(function(){
        
        var roleBtn = $(this);
        var roleForm = $('#RoleForm');
        
        roleForm.validate({
            rules: {
                RoleName: 'required'
            },
            messages: {
                RoleName: 'RoleName is required'
            }
        });
        
        if(!roleForm.valid()){
            return;
        }

        var _$permissionCheckboxes = $("input[name='permission']:checked");
        if(_$permissionCheckboxes.length < 1){
            swal('Permissions', 'Select at least one permission', 'error');
            return;
        }
        
        roleBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        var displayName = $('#RoleName').val();
        var name = displayName.replace(/\s/g, '');
        var permissions = [];

        if (_$permissionCheckboxes) {
            for (var permissionIndex = 0; permissionIndex < _$permissionCheckboxes.length; permissionIndex++) {
                var _$permissionCheckbox = $(_$permissionCheckboxes[permissionIndex]);
                console.log('permission value: ' + _$permissionCheckbox.val());
                permissions.push(_$permissionCheckbox.val());
            }
        }

        roleForm.ajaxSubmit({
            
            data: {Name: name, DisplayName: displayName, Permissions: permissions},
            success: function(response, status, xhr, $form){
                //Move to the next form for Personal details
                console.log('returned status: ' + status);
                $('#_CreateRoleModal').modal('hide');
            },
            error: function(jqXHR, textStatus, err){
                var respObj = JSON.parse(jqXHR.responseText);
                swal("Oops", respObj.error.message, "error");
            },
            complete: function(jqXHR, textStatus){
                roleBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });
});