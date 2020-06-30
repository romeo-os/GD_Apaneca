
function EliminarArchivo(id) {

    var btn = document.getElementById("btnModalElimiar");

    btn.onclick = function () {
        var url = $(this).data('request-url');
        location.href = url + "/" + id;
    }
}