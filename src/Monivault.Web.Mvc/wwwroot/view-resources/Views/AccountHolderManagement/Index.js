$(function(){
    $('#AccountHolderTable').DataTable({});
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
                    console.log('this upload was successful...');
                },
                error: function (jqXHR, textStatus, err) {
                    console.log('this upload failed...');
                },
                complete: function (jqXHR, textStatus) {
                    console.log('this upload is complete.');
                    mApp.unblock('#PortletAccountMgr');
                }
            });
        }
    });
});