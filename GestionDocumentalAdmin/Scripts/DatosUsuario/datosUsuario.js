$(document).ready(function () {
    dropConfig();
    mostrarPwd();
});

function dropConfig() {

    $('#menuUser').dropdown();

    $('#menuUser').click(function () {

        $("#modal-content").load("/DatosUsuario/UpdateDatos/", function () {

            $("#modal-form").submit(function (ev) {
                ev.preventDefault();
            });

            startValidation();
        });
    }); 
}


function startValidation() {
    $("#modal-form").validate({
        rules: {
            usuario: { required: true, minlength: 3, maxlength: 45 },
            clave: { required: true, minlength: 6, maxlength: 45 },
            nuevaClave: { required: true, minlength: 6, maxlength: 45 },
            confirClave: { required: true, minlength: 6, maxlength: 45 }
        },
        submitHandler: function () {
            save();
        }
    });
}

function save() {

    var form = $("#modal-form");

    $.ajax({
        url: form.attr("action"),              
        type: 'POST',
        data: form.serialize(),
        success: function (data) {
            if (data.success == true) {
                toastr.success(data.message);
                $('.drop').hide();
            } else {
                toastr.error(data.message);
            }
        }
    });
}

function mostrarPwd() {
    //input password actual
    $('#mostrarC').click(function () {

        if ($(this).hasClass('fa-eye')) {
            $('.password1').removeAttr('type');
            $('#mostrarC').addClass('fa-eye-slash').removeClass('fa-eye');
        }

        else {
            $('.password1').attr('type', 'password');
            $('#mostrarC').addClass('fa-eye').removeClass('fa-eye-slash');
        }
    });

    //input password nueva
    $('#mostrarC1').click(function () {

        if ($(this).hasClass('fa-eye')) {
            $('.password2').removeAttr('type');
            $('#mostrarC1').addClass('fa-eye-slash').removeClass('fa-eye');
        }

        else {
            $('.password2').attr('type', 'password');
            $('#mostrarC1').addClass('fa-eye').removeClass('fa-eye-slash');
        }
    });

    //input respetir password
    $('#mostrarC2').click(function () {

        if ($(this).hasClass('fa-eye')) {
            $('.password3').removeAttr('type');
            $('#mostrarC2').addClass('fa-eye-slash').removeClass('fa-eye');
        }

        else {
            $('.password3').attr('type', 'password');
            $('#mostrarC2').addClass('fa-eye').removeClass('fa-eye-slash');
        }
    });

}   



