var table;
var actions;
var dialog;
var title;
var editar;

$(document).ready(function (event) {
    prepareButtons();
    initTable();
});

function prepareButtons() {
    $('.button-create').click(function () {
        getForm();                                                                  // 0 para un crear un nuevo registro
    });

    actions = $('#action-buttons').html();                                          //Recupera botones que se agregarán en cada fila
}

function initTable() {
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        $('.dataTables_empty').hide()
    };

    var idC = $("#idCarpeta").val();

    table = $('#table-content')
        .on('draw.dt', function (e, settings, json, xhr) {
            setTimeout(function () { bindButtons(); }, 500);
            drawRowNumbers("#table-content", table);                                //crear numeración ordenada que se mostrara en la tabla
        })
        .DataTable({
            ajax: "/Documentos/GetDocumentosData/"+idC,                                  //Url de donde tomara los datos
            language: { url: "/Content/es.json" },
            columns: [
                { data: "IdDocumento", sortable: false, searchable: false },
                {
                    data: "NombreDocumentos",
                    render: function (data, type, row, meta) {
                        return '<a href="#' + row.IdDocumento + '">' + data + '<a/>';
                    }
                },
                { data: "TipoDocumento" },
                { data: "created_at" },
                { data: "updated_at" },
                {
                    sortable: false, searchable: false,
                    render: function (data, type, row, meta) {
                        return actions.replace("{data}", Base64.encode(JSON.stringify(row)));
                    }
                }
            ]
        });

    $('#table-content').removeClass('display').addClass('table table - striped table - bordered');  //Quitando mensaje de -no datos-
};

/**
 * Asignando eventos y validando las acciones de los botones de cada fila
 * */
function bindButtons() {
    $('#table-content tbody tr td button').unbind('click').on('click', function (event) {
        if (event.preventDefault) event.preventDefault();
        if (event.stopImmediatePropagation) event.stopImmediatePropagation();

        var obj = JSON.parse(Base64.decode($(this).parent().attr("data-row")));         //Recupera la data cifrada para editar el registro
       // alert();
        var action = $(this).attr("data-action");                                       //Recupera la acción de del boton accionado

        console.log(obj);

        if (action == 'download') {
            downloadFile(obj.IdDocumento, obj.RutaDocumento);
        } else if (action == 'edit') {
            //alert("editar");
            editar = true;
            getForm(obj.IdDocumento);                                                     //Pasa el id del elemento que se editará
        } else if (action == 'delete') {
            deleteRecord(obj.IdDocumento);                                                //Pasa el id del objeto que se eliminará
        }
    })
}

/**
 * Muestra un modal con el fomulario para crear o editar un nuevo registro
 * @param {int} id  identificador del registro a editar
 */
function getForm(id) {
    var isEditing = !(typeof (id) === "undefined" || id === 0);
    var toUrl = isEditing ? "/Documentos/EditarDocumento/" + id : "/Documentos/SubirDoc/" + $("#idCarpeta").val();
    title = isEditing ? "Editar documento" : "Cargar documento";

    $.ajax({
        url: toUrl,                                                                     // Ruta de la vista del formulario -crear | editar-
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
            nombre: { required: true, minlength: 3, maxlength: 45 },
            idTipoDocumento: { required: true }
        },
        submitHandler: function (form) {
            save();
        }
    });
}

/**
 * Muestra un modal para confirmar que se eliminará un registro
 * @param {int} id identificador del registro a eliminar
 */
function deleteRecord(id) {
    bootbox.confirm("¿Está seguro de eliminar el registro?", function (result) {
        if (result) {
            $.ajax({
                url: "/Documentos/EliminarArchivo/" + id,                                 //Url del API enpoint para eliminar el registro
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        table.ajax.reload();                                            //Refrescar los datos de la tabla
                        toastr.info(data.message)                                    //Mostrar mensaje emergente del resultado de la operación
                    } else {
                        toastr.error(data.message)                                      //Mostrar mensaje emergente del resultado de la operación
                    }
                }
            });
        }
    });
}

/**
 * Guarda la información del formulario para crear o editar un registro
 * */
function save() {

    if (editar) {
        var form = $("#modal-form");
        $.ajax({
            url: form.attr("action"),          //Url del API enpoint para guardar el registro
            type: 'POST',
            data: form.serialize(),
            success: function (data) {
                if (data.success == true) {
                    table.ajax.reload();
                    dialog.modal('hide');
                    toastr.success(data.message);
                } else {
                    toastr.error(data.message);
                }
            }
        });

    } else {

        //nuevo
    var form = $("#modal-form");
    var formData = new FormData();
    var fileUpload = $('#file-upload')[0].files[0];

    formData.append("file", fileUpload);
    jQuery.each($(form).serializeArray(), function (index, field) {
        formData.append(field.name, field.value);
    });

        $.ajax({
        url: "/Documentos/SubirDoc",   
        type: 'POST',
        processData: false,
        contentType: false,
        cache: false,
        data: formData,
        success: function (data) {
            if (data.success == true) {
                table.ajax.reload();
                dialog.modal('hide');
                toastr.success(data.message);
            } else {
                toastr.error(data.message);
                alert("Fail");
            }
        }
    });
    }
}

function cambiar() {
    var file = $('#file-upload')[0].files[0];
    $("#nombreDocumentos").val(file.name);
}

function downloadFile(id, path) {
    $.ajax({
        url: "/Documentos/Descargar/"+id,
        method: 'GET',
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var a = document.createElement('a');
            var url = window.URL.createObjectURL(data);
            a.href = url;
            a.download = getNameFile(path);
            document.body.append(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
        }
    });
}

function getNameFile(path) {
    var pathArray = path.split('/');
    var last = pathArray[pathArray.length - 1]

    return last;
}