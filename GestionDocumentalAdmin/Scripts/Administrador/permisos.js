var table;
var actions;
var dialog;
var title;
var opd;

$(document).ready(function (event) {
    prepareButtons();
    initTable();
    start();
});



function prepareButtons() {
    $('.button-guardar').click(function () {
        obtenerPermisos();                                                           
    });

    actions = $('#action-buttons').html();                                         
}

//funcion para marcar los check de cada permiso bloqueado por Rol
function start() {

    var _idRol = $("#idrol").text();
     
    $.ajax({
        type: "POST",                                              // tipo de request que estamos generando
        url: "/Administrador/GetPermisosD/" + _idRol,                        // URL al que vamos a hacer el pedido
        data: null,                                                // data es un arreglo JSON que contiene los parámetros que 
                                                                   // van a ser recibidos por la función del servidor
        //contentType: "application/json; charset=utf-8",            // tipo de contenido
        dataType: "json",                                          // formato de transmición de datos
        async: true,                                               // si es asincrónico o no
        success: function (resultado) { 

            //var permisos = resultado;

            //$('.lblResultado').text(resultado);

            //alert(permisos);
            $('input:checkbox').each(function (i) {
                //Obtenemos la data de la fila para saber el idPermiso
                var obj = JSON.parse(Base64.decode($(this).parent().attr("data-row")));

                for (var j = 0; j < 13; j++) {

                    if (obj.idPermiso == resultado[j]) {

                        //Marcamos el check por su indice
                        $("input[type=checkbox]").eq(i).prop("checked", true);
                    }
                }

            });
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) { 
            var error = eval("(" + XMLHttpRequest.responseText + ")");
            //aler(error.Message);
        }
    });
}

function initTable() {
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        $('.dataTables_empty').hide()
    };

    table = $('#table-content')
        .on('draw.dt', function (e, settings, json, xhr) {
            setTimeout(function () { /*bindButtons();*/ }, 500,);
            drawRowNumbers("#table-content", table);
        })
        .DataTable({
            paging: false,
            scrollY: 400,
            ajax: "/Administrador/GetPermisos",                                    //Url de donde tomara los datos
            //language: { url: "/Content/es.json" },
            language: {
                "search": "Buscar:"
            },
            columns: [
                { data: "idPermiso", sortable: false, searchable: false },
                { data: "modulo" },
                { data: "permiso" },
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

function obtenerPermisos() {

    $('input:checkbox').each(function () {

            var obj = JSON.parse(Base64.decode($(this).parent().attr("data-row")));

            if (this.checked == true) {
                var _idRol = $("#idrol").text();
                //si esta seleccionado se bloqueara ese permiso
                save(_idRol, obj.idPermiso);

            } else {
                var _idRol = $("#idrol").text();
                //si esta deseleccionado se llamara la funcion de remover permiso
                //Ya que puede haber sido deseleccionado para darle el permiso
                removePermiso(_idRol, obj.idPermiso);
            }
        });
}

function save(id_Rol, idPermiso) {

    var b = false;

    $.ajax({
        url: "/Administrador/SavePermisos/",
        type: 'POST',
        data: {
            idRol: id_Rol,
            permisoId: idPermiso
        },
        success: function (data) {

            if (data.success == true) {
                //Se muestran un msj por cada registro
               // toastr.success(data.message);
              
            } else {
                // toastr.error(data.message);
            } 
        }
    });
    
}

function removePermiso(id_Rol, idPermiso) {

    $.ajax({
        url: "/Administrador/RemovePermiso/",                                   
        type: 'DELETE',
        data: {
            idRol: id_Rol,
            permisoId: idPermiso
        },
        success: function (data) {
            if (data.success) {                                               
                //toastr.info(data.message)                                           
            } else {
                //toastr.error(data.message)                                         
            }
        }
    });

    
}
