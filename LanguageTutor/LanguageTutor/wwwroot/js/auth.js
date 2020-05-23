$(document).ready(function () {

    const disableOff = () => {
        $('button.reg').removeAttr('disabled');
        $('button.reg').css('background', '#316879');
        $('button.reg').css('cursor', 'pointer');
    }

    const disableOn =() => {
        $('button.reg').attr('disabled', 'disabled');
        $('button.reg').css('background', '#ced7d8');
        $('button.reg').css('cursor', 'default');
    }

    const loginDisableOff = () => {
        $('button.login').removeAttr('disabled');
        $('button.login').css('background', '#316879');
        $('button.login').css('cursor', 'pointer');
    }

    const loginDisableOn = () => {
        $('button.login').attr('disabled', 'disabled');
        $('button.login').css('background', '#ced7d8');
        $('button.login').css('cursor', 'default');
    }

    $('input.login').on("input", function () {
        let login = $('.login#signInLogin').val();
        let pass = $('.login#signInPassword').val();
        
        if (login.length != 0 && pass.length != 0) {
            loginDisableOff();
        } else {
            loginDisableOn();
        }
    });

    $('input.reg').on("input", function () {
        let login = $('.reg#signUpLogin').val();
        let pass = $('.reg#signUpPassword').val();
        let pass2 = $('.reg#signUpConfirmPsw').val();

        if (pass === pass2) {
            $('.signUpErrorMsg').html('');
            if (login.length != 0 && pass.length != 0 && pass2.length != 0) {
                disableOff()
            } else {
                disableOn()
            }
        } else {
            disableOn()
            $('.signUpErrorMsg').html('Пароли не совпадают');
        };
    });

    $('#signIn').on('click', function TrySignIn() {
        const login = $("#signInLogin").val();
        const password = $("#signInPassword").val();

        $.ajax({
            type: "Post",
            url: '/Auth/SignIn',
            data: {
                login: login,
                password: password
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
                loginDisableOn();
            },
            success: function (data) {
                let result = data;
                if (result.success) {
                    window.location = result.body;
                }
                else {
                    $('.signInErrorMsg').html(`<h5> ${result.errorsMessage} </h5>`);
                    
                }
                loginDisableOff();
            }
        })

    })

    $('#signUp').on('click', function TrySignUp() {
        $('#signUpErrorMsg').text("");

        const login = $("#signUpLogin").val();
        const password = $("#signUpPassword").val();
        const confirmPassword = $("#signUpConfirmPsw").val();

        $.ajax({
            type: "Post",
            url: '/Auth/SignUp',
            data: {
                login: login,
                password: password
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
                disableOff()
            },
            success: function (data) {
                let result = data;
                if (result.success) {
                    $('#signInLogin').val(result.login);
                    $('#tab-1').prop('checked', true);
                    $('.message').html('<p>Ok</p>');
                    $("#form-2")[0].reset();
                }
                else {
                    $('.signUpErrorMsg').html(`<h6> ${result.errorsMessage} </h6>`);
                }
                disableOff();
            }
        })
    })


})