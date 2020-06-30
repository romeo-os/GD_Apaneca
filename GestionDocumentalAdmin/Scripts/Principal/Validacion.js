
// Aun no se esta utilizando
 
$.validator.setDefaults({
        highlight: function (element) {
        $(element).closest('.form-group').removeClass('has-success has-feedback').addClass('has-error has-feedback'); // red highlighting
    $(element).closest('.form-group').find('.form-control-feedback').removeClass('glyphicon-ok').addClass('glyphicon-remove'); // red cross glyphicon
},
        unhighlight: function (element) {
        $(element).closest('.form-group').removeClass('has-error has-feedback').addClass('has-success has-feedback'); // green highlighting
    $(element).closest('.form-group').find('.form-control-feedback').removeClass('glyphicon-remove').addClass('glyphicon-ok'); // green tick glyphicon
}
});
