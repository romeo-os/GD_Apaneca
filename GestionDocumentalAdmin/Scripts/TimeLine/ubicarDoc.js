var table;
var actions;
var dialog;
var title;

/**
 * Cuando el documento haya cargado
 * */
$(document).ready(function (event) {
    prepareButtons();
    initTable();
});

/**
 * Cargar, asignar los eventos a los botones
 * */
function prepareButtons() {
    //clase del btn ara asignar el evento click
    $('#button-recibir').click(function () {
        getForm();
    });

    actions = $('#action-buttons').html(); 
}

/*
 * Cargar los datos en la tabla
 * */
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
            ajax: "/TimeLine/GetDocumentos",                                            //Url de donde tomara los datos
            language: { url: "/Content/es.json" },
            columns: [
                { data: "IdDocumento", sortable: false, searchable: false },
                {
                    data: "NombreDoc",
                    render: function (data, type, row, meta) {
                        return '<a href="/TimeLine/UbicarDocumentosR/' + row.IdDocumento + '">' + data + '<a/>';
                    }
                },                                                                      //Asigna el item  de la data obtenida a la segunda celda de la fila
                { data: "TipoDocumento" },                                            //Asigna el item de la data obtenida a la tercera celda de la fila
                {
                    sortable: false, searchable: false,
                    render: function (data, type, row, meta) {
                        return actions.replace("{data}", Base64.encode(JSON.stringify(row)));   //Cargando data cifrada para recuperarla cuando se edite una fila. Tambien coloca los botones
                    }
                }
            ]
        });

    $('#table-content').removeClass('display').addClass('table table - striped table - bordered');  //Quitando mensaje de -no datos-
};

function getForm() {

    var toUrl = "/TimeLine/RecibirDoc";
    title = "Recibir documento";

    $.ajax({
        url: toUrl,                               // Ruta de la vista del formulario -crear | editar-
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