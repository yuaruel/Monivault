$(function(){
    var userTable = $('#UserTable').DataTable({
        ajax:{
            url: abp.appPath + 'Users/UserList',
            dataSrc: 'result'
        },
        columns: [
            {
                data: 'name',
                render: function(data, type, full, meta){
                    return data + ' ' + full.surname;
                }
            },
            {
                data: 'userName'
            },
            {
                data: 'roleNames',
                render: function (data, type, full, meta){
                    var roles = '';
                    if(data.length < 2){
                        roles = data;
                    }else{
                        data.forEach(function(element){
                            roles = roles + element  + ', ';
                        });
                    }
                    
                    return roles;
                }
            },
            {
                data: 'phoneNumber'
            },
            {
                data: 'userKey',
                render: function(data, type, full, meta){
                    return '<a id="EditUserBtn" href="#" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="View">'
                        +	'<i class="la la-edit"></i>' +
                            '</a>'
                }
            }
        ]
    });

    $('#UserTable tbody').on('click', '#EditUserBtn', function(e){
        e.preventDefault();
        var data = userTable.row($(this).closest('tr')).data();

        $.ajax({
            url: abp.appPath + 'Users/EditUserModal?userKey=' + data.userKey,
            type: 'GET',
            contentType: 'application/html',
            success: function (content) {
                $('#_EditUserModal').modal('show');
                $('#_EditUserModal div.modal-body').html(content);
            },
            error: function (e) { }
        });
    });

    $('#AddSingleUserBtn').click(function (e) {
        e.preventDefault();
        $.ajax({
            url: abp.appPath + 'Users/CreateUserModal',
            type: 'GET',
            contentType: 'application/html',
            success: function (content) {
                $('#_CreateUserModal div.modal-body').html(content);
                $('#_CreateUserModal').modal('show');
            },
            error: function (e) { }
        });
    });

    $('#_CreateUserModal').on('hidden.bs.modal', function(e){
        userTable.ajax.reload();
    });
    $('#_EditUserModal').on('hidden.bs.modal', function(e){
        userTable.ajax.reload();
    });
});
/*
(function() {
    $(function() {

        var _userService = abp.services.app.user;
        var _$modal = $('#UserCreateModal');
        var _$form = _$modal.find('form');

        _$form.validate({
            rules: {
                Password: "required",
                ConfirmPassword: {
                    equalTo: "#Password"
                }
            }
        });

        $('#RefreshButton').click(function () {
            refreshUserList();
        });

        $('.delete-user').click(function () {
            var userId = $(this).attr("data-user-id");
            var userName = $(this).attr('data-user-name');

            deleteUser(userId, userName);
        });

        $('.edit-user').click(function (e) {
            var userId = $(this).attr("data-user-id");

            e.preventDefault();
            $.ajax({
                url: abp.appPath + 'Users/EditUserModal?userId=' + userId,
                type: 'POST',
                contentType: 'application/html',
                success: function (content) {
                    $('#UserEditModal div.modal-content').html(content);
                },
                error: function (e) { }
            });
        });

        _$form.find('button[type="submit"]').click(function (e) {
            e.preventDefault();

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

            abp.ui.setBusy(_$modal);
            _userService.create(user).done(function () {
                _$modal.modal('hide');
                location.reload(true); //reload page to see new user!
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });
        });
        
        _$modal.on('shown.bs.modal', function () {
            _$modal.find('input:not([type=hidden]):first').focus();
        });

        function refreshUserList() {
            location.reload(true); //reload page to see new user!
        }

        function deleteUser(userId, userName) {
            abp.message.confirm(
                abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Monivault'), userName),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userService.delete({
                            id: userId
                        }).done(function () {
                            refreshUserList();
                        });
                    }
                }
            );
        }
    });
})();*/
