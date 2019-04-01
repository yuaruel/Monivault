$(function(){
/*	var _createOrEditModal = new app.ModalManager({
		viewUrl: abp.appPath + 'Roles/CreateOrEditModal',
		scriptUrll: abp.appPath + 'view-resources/Views/Roles/_CreateOrEditModal.js',
		modalClass: 'CreateOrEditRoleModal'
	});*/
	var roleTable = $('#RoleTable').DataTable({
		ajax: {
			url: abp.appPath + 'Roles/AllRoles',
			dataSrc: 'result'
		},
		columnDefs: [
			{
				targets: 0,
				width: '30px'
			}
		],
		columns: [

			{
				data: 'displayName',
				width: '50px'
			},
			{data: 'createdTime'},
			{
				data: 'permissions',
				render: function(data, type, full, meta){
					var output = '';

					for(var index in data){
						output = output.concat(data[index].displayName, '<span class="m-badge m-badge--brand m-badge--dot"></span>');
					}
					
					return output;
				}
			},
			{
				data: 'roleKey',
				render: function(data, type, full, meta){
					return '<a id="EditRoleBtn" href="#" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="View">'
							+	'<i class="la la-edit"></i>' + 
							'</a>'
				}
			}
		]
	});
	$('#RoleTable tbody').on('click', '#EditRoleBtn', function(e){
		e.preventDefault();
		var data = roleTable.row($(this).closest('tr')).data();

		$.ajax({
			url: abp.appPath + 'Roles/EditRoleModal?roleKey=' + data.roleKey,
			type: 'GET',
			contentType: 'application/html',
			success: function (content) {
				$('#_EditRoleModal').modal('show');
				$('#_EditRoleModal div.modal-body').html(content);
			},
			error: function (e) { }
		});
	});
	$('#AddNewRoleBtn').click(function (e) {
		e.preventDefault();
		$.ajax({
			url: abp.appPath + 'Roles/CreateRoleModal',
			type: 'GET',
			contentType: 'application/html',
			success: function (content) {
				$('#_CreateRoleModal div.modal-body').html(content);
			},
			error: function (e) { }
		});
	});
	
	$('#_CreateRoleModal').on('hidden.bs.modal', function(e){
		roleTable.ajax.reload();
	});
	$('#_EditRoleModal').on('hidden.bs.modal', function(e){
		roleTable.ajax.reload();
	});
});
/*(function () {
	$(function () {

		var _roleService = abp.services.app.role;
		var _$modal = $('#RoleCreateModal');
		var _$form = _$modal.find('form');

		_$form.validate({
		});

		$('#RefreshButton').click(function () {
			refreshRoleList();
		});

		$('.delete-role').click(function () {
			var roleId = $(this).attr("data-role-id");
			var roleName = $(this).attr('data-role-name');

			deleteRole(roleId, roleName);
		});

		$('.edit-role').click(function (e) {
			var roleId = $(this).attr("data-role-id");

			e.preventDefault();
			$.ajax({
				url: abp.appPath + 'Roles/EditRoleModal?roleId=' + roleId,
				type: 'POST',
				contentType: 'application/html',
				success: function (content) {
					$('#RoleEditModal div.modal-content').html(content);
				},
				error: function (e) { }
			});
		});

		_$form.find('button[type="submit"]').click(function (e) {
			e.preventDefault();

			if (!_$form.valid()) {
				return;
			}

			var role = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js
			role.permissions = [];
			var _$permissionCheckboxes = $("input[name='permission']:checked");
			if (_$permissionCheckboxes) {
				for (var permissionIndex = 0; permissionIndex < _$permissionCheckboxes.length; permissionIndex++) {
					var _$permissionCheckbox = $(_$permissionCheckboxes[permissionIndex]);
					role.permissions.push(_$permissionCheckbox.val());
				}
			}

			abp.ui.setBusy(_$modal);
			_roleService.create(role).done(function () {
				_$modal.modal('hide');
				location.reload(true); //reload page to see new role!
			}).always(function () {
				abp.ui.clearBusy(_$modal);
			});
		});

		_$modal.on('shown.bs.modal', function () {
			_$modal.find('input:not([type=hidden]):first').focus();
		});

		function refreshRoleList() {
			location.reload(true); //reload page to see new role!
		}

		function deleteRole(roleId, roleName) {
			abp.message.confirm(
                abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Monivault'), roleName),
				function (isConfirmed) {
					if (isConfirmed) {
						_roleService.delete({
							id: roleId
						}).done(function () {
							refreshRoleList();
						});
					}
				}
			);
		}
	});
})();*/
