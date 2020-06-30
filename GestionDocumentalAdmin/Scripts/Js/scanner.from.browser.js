$(document).ready(function (event) {
    prepareButtons();
    startValidation();
});

function prepareButtons() {
    $('.button-create').click(
        function () {
            scanToPdfWithThumbnails();
        }
    );

    $('.button-secondary').prop("disabled", true);
}

//Validate form
function startValidation() {
    jQuery.validator.addMethod("noSpace", function (value, element) {
        return value == '' || value.trim().length != 0;
    }, "El nombre debe ser sin espacios");

    $('#modal-form').validate({
        rules: {
            nombreDocumentos: { required: true, minlength: 2, maxlength: 45},
            idTipoDocumento: { required: true, number: true},
            idCarpeta: { required: true, number: true}
        },
        submitHandler: function (form) {
            submitFormWithScannedImages();
        }
    });
}

/**
 * Limpiar los datos cargados
 * */
function clearForm() {
    $("#nombreDocumentos").val("");
    $("#idTipoDocumento").val("");
    $("#idCarpeta").val("");

    document.getElementById('images').innerHTML = '';   // clear images
    document.getElementById('server_response');         // clear response
    imagesScanned = [];

    $('.button-create').prop("disabled", false);
    $('.button-secondary').prop("disabled", true);
}

/** Scan: output PDF original and JPG thumbnails */
function scanToPdfWithThumbnails() {
    scanner.scan(displayImagesOnPage,
        {
            "output_settings": [
                {
                    "type": "return-base64",
                    "format": "pdf",
                    "pdf_text_line": "By ${USERNAME} on ${DATETIME}"
                },
                {
                    "type": "return-base64-thumbnail",
                    "format": "jpg",
                    "thumbnail_height": 200
                }
            ]
        }
    );
}

/** Processes the scan result */
function displayImagesOnPage(successful, mesg, response) {
    if (!successful) { // On error
        toastr.warning("Se denegó el permiso");
        return;
    }

    if (successful && mesg != null && mesg.toLowerCase().indexOf("user cancel") >= 0) { // User cancelled.
        toastr.info("Canceló el escaneo");
        return;
    }

    var scannedImages = scanner.getScannedImages(response, true, false); // returns an array of ScannedImage
    for (var i = 0; (scannedImages instanceof Array) && i < scannedImages.length; i++) {
        var scannedImage = scannedImages[i];
        processOriginal(scannedImage);
    }

    var thumbnails = scanner.getScannedImages(response, false, true); // returns an array of ScannedImage
    for (var i = 0; (thumbnails instanceof Array) && i < thumbnails.length; i++) {
        var thumbnail = thumbnails[i];
        processThumbnail(thumbnail);
    }

    $('.button-create').prop("disabled", true);
    $('.button-secondary').prop("disabled", false);
}

/** Images scanned so far. */
var imagesScanned = [];

/** Processes an original */
function processOriginal(scannedImage) {
    imagesScanned.push(scannedImage);
}

/** Processes a thumbnail */
function processThumbnail(scannedImage) {
    var elementImg = scanner.createDomElementFromModel({
        'name': 'img',
        'attributes': {
            'class': 'scanned',
            'src': scannedImage.src
        }
    });
    document.getElementById('images').appendChild(elementImg);
}

/** Upload scanned images by submitting the form */
function submitFormWithScannedImages() {
    if (scanner.submitFormWithImages('modal-form', imagesScanned, function (xhr) {
        if (xhr.readyState == 4) { // 4: request finished and response is ready
            var obj = JSON.parse(xhr.responseText);

            if (obj.success) {
                toastr.success(obj.message);
                clearForm();
            } else {
                toastr.success(obj.message);
            }
        }
    })) {
        document.getElementById('server_response').innerHTML = "Enviando, por favor espere ...";
    } else {
        document.getElementById('server_response').innerHTML = "Por favor, escanear primero.";
    }
}