$(function(){
    var accountHolderTable = $('#AccountHolderTable').DataTable({
        ajax: {
            url: abp.appPath + 'AccountHolderManagement/AccountHolderList',
            dataSrc: 'result'
        },
        columns: [
            {
                data: 'accountIdentity',
                render: function (data, type, full, meta) {
                    return '<a href="AccountHolderManagement/Profile/' + full.accountHolderKey + '">' + data + '</a<>';
                }
            },
            { data: 'firstName' },
            { data: 'lastName' },
            { data: 'phoneNumber' },
            { data: 'email' }
        ]
    });
    $('#OpenUploadBoxBtn').click(function(){
        $('#AccountHolderUploadBox').trigger('click');
    });

    $('#AccountHolderUploadBox').change(function (e) {
        var attachedFile = e.target.files[0];
        var splitName = attachedFile.name.split('.', 2);

        if (splitName[1] != 'xlsx') {
            swal('Oops', 'The uploaded file should be a .xlsx type', 'error');
        } else {
            var fileData = new FormData();
            fileData.append('uploadedFile', e.target.files[0]);

            mApp.block('#PortletAccountMgr', {
                overlayColor: '#000000',
                type: 'loader',
                state: 'primary',
                message: 'Please wait...'
            });

            $.ajax({
                url: abp.appPath + 'AccountHolderManagement/UploadAccountHolders',
                method: 'POST',
                data: fileData,
                processData: false,
                contentType: false,
                success: function (data, textStatus, jqXhr) {
                    accountHolderTable.ajax.reload();
                },
                error: function (jqXHR, textStatus, err) {
                    swal('Oops', 'There was an error completing your upload', 'error');
                },
                complete: function (jqXHR, textStatus) {           
                    mApp.unblock('#PortletAccountMgr');
                }
            });
        }
    });
});