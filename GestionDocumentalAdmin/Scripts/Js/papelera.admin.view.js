var table;
var actions;
var dialog;
var title;

$(document).ready(function (event) {
    prepareButtons();
    initTable();
});

function prepareButtons() {
    actions = $('#action-buttons').html();                                          //Recupera botones que se agregarán en cada fila
}

function initTable() {
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        $('.dataTables_empty').hide()
    };

    table = $('#table-content')
        .on('draw.dt', function (e, settings, json, xhr) {
            setTimeout(function () { bindButtons(); }, 500);
            drawRowNumbers("#table-content", table);                                //crear numeración ordenada que se mostrara en la tabla
        })
        .DataTable({
            ajax: "/Administrador/GetPapeleraData",                                 //Url de donde tomara los datos
            //language: { url: "/assets/plugins/dataTable/lang-es.json" },
            language: { url: "/Content/es.json" },
            columns: [
                //{ data: "IdCarpeta", sortable: false, searchable: false },
                { data: "IdDocumento", sortable: false, searchable: false },
                { data: "NombreArchivo" },
                { data: "FechaE" },
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
        var action = $(this).attr("data-action");                                       //Recupera la acción de del boton accionado

        if (action == 'delete') {
            deleteRecord(obj.IdDocumento);                                                //Pasa el id del objeto que se eliminará
        } else if (action == 'reset') {
            resetRecord(obj.IdCarpeta);                                                     //Pasa el id del elemento que se editará
        }
    })
}

/**
 * Muestra un modal para confirmar que se eliminará un registro
 * @param {int} id identificador del registro a eliminar
 */
function deleteRecord(id) {
    bootbox.confirm("¿Está seguro de eliminar el registro?", function (result) {
        if (result) {
            $.ajax({
                url: "/Administrador/EliminarArchivo/" + id,                            //Url del API enpoint para eliminar el registro
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        table.ajax.reload();                                            //Refrescar los datos de la tabla
                        toastr.info(data.message)                                       //Mostrar mensaje emergente del resultado de la operación
                    } else {
                        toastr.error(data.message)                                      //Mostrar mensaje emergente del resultado de la operación
                    }
                }
            });
        }
    });
}

function resetRecord(id) {
    bootbox.confirm("¿Desea restablecer el registro a su ubicación de origen?", function (result) {
        if (result) {
            $.ajax({
                url: "/Administrador/resetArchivo/" + id,                            //Url del API enpoint para eliminar el registro
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        table.ajax.reload();                                            //Refrescar los datos de la tabla
                        toastr.info(data.message)                                       //Mostrar mensaje emergente del resultado de la operación
                    } else {
                        toastr.error(data.message)                                      //Mostrar mensaje emergente del resultado de la operación
                    }
                }
            });
        }
    });
}