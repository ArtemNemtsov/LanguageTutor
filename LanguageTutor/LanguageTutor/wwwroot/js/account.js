$(document).ready(function ()
{
    $('#idChangePasssword').on('click', function TrySignIn() {
        const login = $("#login").val();
        const password = $("#oldPassword").val();
        const newPassword = $("#newPassword").val();

        $.ajax({
            type: "Post",
            url: '/Account/ChangePassword',
            data: {
                login: login,
                password: password,
                newPassword: newPassword,
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (data) {
                let result = data;
                if (result.success)
                {       
                    document.getElementById('changePassswordResult').innerHTML = result.body;
                }
                else
                {
                    document.getElementById('changePassswordResult').innerHTML = result.errorsMessage;
                }
            }
        })
    })

})