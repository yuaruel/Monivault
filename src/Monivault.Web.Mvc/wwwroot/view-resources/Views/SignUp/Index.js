$(function() {

    $('#PhoneNumber').inputmask({
        'mask': '99999999999',
        'greedy': true,
        placeholder: ""
    });
    
    var signup = $('#m_signup');
    
    var displayPersonalDetail = function(){
        signup.removeClass('m-login--contact-detail');
        
        signup.addClass('m-login--personal-detail');
        mUtil.animateClass(signup.find('.m-login__personal-detail')[0], 'flipInX animated');
    };
    
    $('#m_login_signup_next').click(function(e){
        var nextBtn = $(this);
        var contactForm = $(this).closest('form');
        contactForm.validate({
            rules: {
                PhoneNumber: {
                    required: true,
                    minlength: 11
                },
                Email: {
                    email: {
                        depends: function (element) {
                            console.log('email content: ' + $('#Email').val());
                            return $('#Email').val() !== '';
                        }
                    }
                }
            },
            messages: {
                Email: {
                    email: 'Invalid email'
                },
                PhoneNumber: {
                    required: 'Phone is required'
                }
            }
        });
        
        if(!contactForm.valid()){
            return;
        }

        nextBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        
        contactForm.ajaxSubmit({
           success: function(response, status, xhr, $form){
               //Move to the next form for Personal details
               displayPersonalDetail();
           },
           error: function(jqXHR, textStatus, err){
               var respObj = JSON.parse(jqXHR.responseText);
               abp.message.error(respObj.error.message, "SignUp Error");
           },
           complete: function(jqXHR, textStatus){
               nextBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
           } 
        });
    });
    
    $('#m_login_signup_submit').click(function(e){
        var submitBtn = $(this);
        var personalForm = $(this).closest('form');
        
        personalForm.validate({
            rules: {
                VerificationCode: {
                    required: true,
                    rangelength: [5, 5]
                },
                FirstName: {
                    required: true
                },
                LastName: {
                    required: true
                },
                UserName: {
                    required: true
                },
                Password: {
                    required: true
                },
                ConfirmPassword: {
                    equalTo: '#Password'
                }
            },
            messages: {
                VerificationCode: {
                    required: 'Verification code is required',
                    rangelength: 'Invalid verification code'
                },
                FirstName: {
                    required: 'First name is required'
                },
                LastName: {
                    required: 'Last name is required'
                },
                UserName: {
                    required: 'User name is required'
                },
                Password: {
                    required: 'Password is required'
                }
            }
        });
        
        if(!personalForm.valid()){
            return;
        }
        
        submitBtn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
        
        /*var signUpData = {
            PhoneNumber: $('#PhoneNumber').val(),
            Email: $('#Email').val(),
            VerificationCode : $('input[name=VerificationCode]').val(),
            FirstName: $('input[name=FirstName]').val(),
            LastName: $('input[name=LastName]').val(),
            UserName: $('input[name=UserName]').val(),
            Password: $('#Password').val()
        };
        
        abp.ajax({
           url: personalForm.attr('action'),
           data: JSON.stringify(signUpData)
        }).done(function(data){
            
        }).fail(function(data){
            console.log(data);
            swal("Oops", data.message, "error");
        }).always(function(){
            //nextBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        });*/
        
        personalForm.ajaxSubmit({
            data: { PhoneNumber: $('#PhoneNumber').val(), Email: $('#Email').val() },
            success: function(response, status, xhr, $form){
                //console.log('response: ' + JSON.stringify(response));
                //console.log('xhr status: ' + xhr.status);
                //redirect to signup successful page.
                window.location.replace(response.targetUrl);
            },
            error: function(jqXHR, textStatus, err){
                //console.log(err);
                var respObj = JSON.parse(jqXHR.responseText);
                
                swal("Oops", respObj.error.message, "error");
            },
            complete: function(jqXHR, textStatus){
                submitBtn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
        });
        
    });
    
    //Login.init();
    //jqXHR.setRequestHeader($('meta[name=_csrf_header]').attr('content'), $('meta[name=_csrf]').attr('content'));
/*    var token = $("meta[name='_csrf']").attr("content");
    var header = $("meta[name='_csrf_header']").attr("content");

    $(document).ajaxSend(function(e, xhr, options){
        xhr.setRequestHeader(header, token);
    });*/

/*    $('.register-form').submit(function(e){
        e.preventDefault();
        var registerValidator = $('.register-form').validate({
            ignore: "",
            rules: {
                Email: {
                    Email: {
                        depends: function (element) {
                            return $('#Email').val() !== '';
                        }
                    }
                },
                Phone: {
                    required: true,
                    minlength: 11
                }
            },
            messages: {
                Email: {
                    required: 'Email is required',
                    Email: 'Invalid email'
                },
                Phone: {
                    required: 'Phone is required'
                }
            },
            highlight: function (element) { // hightlight error inputs
                $(element).closest('.form-group').addClass('has-error'); // set error class to the control group
            },
            unhighlight: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorPlacement: function (error, element) {
                error.insertAfter($(element).closest('.form-group'));
            }
        });

        if (registerValidator.form()) {

            var phoneNo = $('#phone').val();
            var emailAdd = $('#email').val();

            /!*App.blockUI({
                target: '#register-form',
                boxed: true,
                message: 'Please wait...'
            });*!/

            $.ajax('/rest/signup/verification-code?phone=' + phoneNo + '&email=' + emailAdd, {
                method: 'POST',
                dataType: 'text',
                success: function (data, textStatus, jqXHR) {
                    console.log('successful verification code: ' + jqXHR.responseText);
                    $('.register-form').hide();
                    $('.personal-detail-form').show();
                },
                error: function (jqXHR, textStatus, err) {
                    console.log('error status: ' + jqXHR.status);
                    console.log('error: ' + jqXHR.responseText);
                    if(jqXHR.status == 400){
                        if(jqXHR.responseText == 'duplicate-phone'){
                            swal('MoniVault Signup', 'This phone number is a already in use.', 'error');
                        }else if(jqXHR.responseText == 'duplicate-email'){
                            swal('MoniVault Signup', 'This email is already in use.', 'error');
                        }else{
                            swal('MoniVault Signup', 'Error sending verification code. Try again later!', 'error');
                        }
                    }else{
                        swal('MoniVault Signup', 'Error sending verification code. Try again later!', 'error');
                    }
                },
                complete: function (jqXHR, textStatus) {
                    //App.unblockUI('#register-form');
                }
            });
        }

        //return false;
    });

    $('#personal_detail').submit(function (e) {
       e.preventDefault();

        var phoneNo = $('#phone').val();
        var emailAdd = $('#email').val();

        $(this).ajaxSubmit({
            beforeSubmit: function (arr, $form, options) {
                var personalFormValidator = $form.validate({
                    ignore: "",
                    rules: {

                        verificationCode: 'required',
                        firstname: {
                            required: true
                        },
                        lastname: {
                            required: true
                        },
                        username: 'required',
                        password: 'required',
                        confirm_password: {
                            equalTo: '#password'
                        },
                        tnc: {
                            required: true
                        }
                    },

                    messages: { // custom messages for radio buttons and checkboxes
                        tnc: {
                            required: "Please accept TNC first."
                        }
                    },

                    invalidHandler: function(event, validator) { //display error alert on form submit

                    },

                    highlight: function(element) { // hightlight error inputs
                        $(element).closest('.form-group').addClass('has-error'); // set error class to the control group
                    },

                    success: function(label) {
                        label.closest('.form-group').removeClass('has-error');
                        label.remove();
                    },

                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "tnc") { // insert checkbox errors after the container
                            error.insertAfter($('#register_tnc_error'));
                        } else if (element.closest('.input-icon').size() === 1) {
                            error.insertAfter(element.closest('.input-icon'));
                        } else if(element.attr('name') == 'username'){
                            error.insertAfter(element.closest('.mt-radio-inline'));
                        } else{
                            error.insertAfter(element);
                        }
                    }
                });

                if(personalFormValidator.form()){
                    App.blockUI({
                        target: '#personal_detail',
                        message: 'Please wait...',
                        boxed: true
                    });
                    return true;
                }else{
                    return false;
                }
            },

            data: {'phone': phoneNo, 'email': emailAdd},
            success: function (data, textStatus, jqXHR) {
                $('.personal-detail-form').hide();
                $(location).attr('href', '/web/signup/successful-verification-page');
            },
            error: function (jqXHR, textStatus, err) {

                swal('MoniVault Signup', jqXHR.responseText, 'error');
            },
            complete: function (jqXHR, textStatus) {
                App.unblockUI('#personal_detail');
            }
        });
    });*/
});