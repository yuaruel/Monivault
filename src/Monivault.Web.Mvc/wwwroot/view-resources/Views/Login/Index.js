$(function(){
    
    $('#m_login_signin_submit').click(function(){
        var submitBtn = $(this);
        var loginForm = $(this).closest('form');
        
        loginForm.validate({
            rules: {
                UserName: {
                    required: true
                },
                Password: {
                    required: true
                }
            },
            messages: {
                UserName: {
                    required: 'UserName is required'
                },
                Password: {
                    required: 'Password is required'
                }
            }
        });
        
        if(!loginForm.valid()){
            return;
        }
        
        submitBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        loginForm.ajaxSubmit({
            success: function(response, status, xhr, $form){
                //Move to the next form for Personal details
                console.log('returned status: ' + status);
                window.location.replace(response.targetUrl);
            },
            error: function(jqXHR, textStatus, err){
                var respObj = JSON.parse(jqXHR.responseText);
                swal("Oops", respObj.error.message, "Signin error");
            },
            complete: function(jqXHR, textStatus){
                submitBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
    });
});