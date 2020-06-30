var dialog;
var title;
var reporte;

/**
 * Cuando el documento haya cargado
 * */
$(document).ready(function (event) {
    prepareButtons();
    //initTable();
});

/**
 * Cargar, asignar los eventos a los botones
 * */
function prepareButtons() {
    //clase del btn ara asignar el evento click
    $('#btnRepDS').click(function () {
        getForm(1);
        reporte = 1;
    });

    $('#btnRepDUp').click(function () {
        getForm(2);
        reporte = 2;
    });
}

function getForm(id) {
   
    //var toUrl = "/Reportes/ValorReporteDs/";
    //title = "Seleccionar documento";
    if (id == 1) {
        var toUrl = "/Reportes/ValorReporteDs/";
        title = "Seleccionar documento";
    } else {
        var toUrl = "/Reportes/ValorReporteDUp/";
        title = "Seleccionar usuario";
    }
    //var opc = !(typeof (id) === 0 || id === 1);
    //var toUrl = opc ? "/Reportes/ValorReporteDs /" : "/Reportes/ValorReporteDUp/";
    //title = opc ? "Seleccionar documento" : "Seleccionar usuario";

    $.ajax({
        url: toUrl,                          
        type: 'GET',
        cache: false,
        success: showDialog
    });
}

/**
 * Muestra en fomrulario en el modal
 * @param {string} form
 */
function showDialog(form) {
    dialog = bootbox.dialog({
        title: title,                                                                   //Titulo que mostrará el modal
        message: form,                                                                  //El mensaje que mostrara el modal es el string DOM del formulario
        size: "medium"                                                                  //Tamaño del modal, puede ser -large-
    });

    $("#modal-form").submit(function (ev) {
        ev.preventDefault();                                                            //bloque la acción por defecto del formulario
    });

    startValidation();

}

/**
 * Reglas de validacion para el formulario
 * */
function startValidation() {

    $("#modal-form").validate({
        rules: {
            idDocumento: { required: true },
            idUsuario: { required: true}
        },
        submitHandler: function (form) {
            crearPDF();
        }
    });
}

/**
 * Guarda la información del formulario para crear o editar un registro
 * */
function crearPDF() {

    if (reporte == 1) {
        var id = $(".idD").val();
        var toUrl = "/Reportes/reporteDS/" + id;
    } else {
        var id = $(".idU").val();
        var toUrl = "/Reportes/reporteDUp/" + id;
    }
    

    $.ajax({
        url: toUrl,
        type: "get",
        data: null,
        success: function (response) {
            dialog.modal('hide');
            window.open(toUrl, '_blank');
            toastr.success("Creando reporte");
        },
        async: true
    });
  
}