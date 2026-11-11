$(document).ready(function () {

  
   // --- 1. CONFIGURACIÓN REGIONAL DE JQUERY VALIDATE ---
    if ($.validator) {
     
        $.extend($.validator.messages, {
            min: $.validator.format("La fecha debe ser igual o posterior a hoy.")
        });
        $.validator.methods.number = function (value, element) {
            var valorFormateado = value.replace(',', '.');
            return this.optional(element) || !isNaN(parseFloat(valorFormateado));
        };
        $.validator.methods.range = function (value, element, param) {
            var valorFormateado = parseFloat(value.replace(',', '.'));
            return this.optional(element) || (valorFormateado >= param[0] && valorFormateado <= param[1]);
        };
    }


    $('#viajeModal').on('hidden.bs.modal', function () {
        var form = $('#formCreateExcursion');
        if (form.length) {
            form[0].reset(); 
            
            var validator = form.validate();
            if (validator) {
                validator.resetForm(); 
            }
            
          
            form.find('.input-validation-error').removeClass('input-validation-error');
            form.find('.field-validation-error')
                .removeClass('field-validation-error')
                .addClass('field-validation-valid')
                .text('');
            
         
            form.find('span[data-valmsg-for]').text('').removeClass('field-validation-error').addClass('field-validation-valid');
        }
    });

    
    $('#modalEdit').on('hidden.bs.modal', function () {
        var form = $(this).find('form');
        if (form.length) {
            var validator = form.validate();
            if (validator) {
                validator.resetForm();
            }
            form.find('.input-validation-error').removeClass('input-validation-error');
            form.find('.field-validation-error')
                .removeClass('field-validation-error')
                .addClass('field-validation-valid')
                .text('');
            form.find('span[data-valmsg-for]').text('').removeClass('field-validation-error').addClass('field-validation-valid');
        }
    });

 
    $('#modalDetails').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);

        // Capturamos el ID de la excursión para usarlo tanto en el diseño como en la petición
        var idExcursion = button.data('id');

        $('#detailsId').text('#' + idExcursion);
        $('#detailsNombre').text(button.data('nombre'));
        $('#detailsFecha').text(button.data('fecha'));
        $('#detailsPrecio').text('$ ' + button.data('precio'));
        $('#detailsCupo').text(button.data('cupo') + ' pasajeros');
        $('#detailsDuracion').text(button.data('duracion') + ' días');
        $('#detailsDescripcion').text(button.data('descripcion') || 'Sin descripción detallada.');

        var idEstado = button.data('estado');
        var textoEstado = idEstado == 1 ? 'Programada' : (idEstado == 2 ? 'Completada' : 'Cancelada');
        $('#detailsEstado').text(textoEstado);

        // NUEVO: Mostrar mensaje de carga inicial y buscar los pasajeros por AJAX
        $('#contenedorPasajeros').html('<p class="text-muted small mb-0 text-center py-2"><span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span> Cargando pasajeros...</p>');

        $.get('/Excursiones/ObtenerPasajeros', { id: idExcursion })
            .done(function (htmlRespuesta) {
                // Inyectamos la tabla HTML que nos devuelve el servidor
                $('#contenedorPasajeros').html(htmlRespuesta);
            })
            .fail(function () {
                $('#contenedorPasajeros').html('<p class="text-danger small mb-0 text-center py-2">Error al cargar la lista de pasajeros.</p>');
            });
    });

    $('#modalDelete').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        $('#deleteId').val(button.data('id'));
        $('#deleteNombre').text(button.data('nombre'));
    });

    $('#modalDelete').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        $('#deleteId').val(button.data('id'));
        $('#deleteNombre').text(button.data('nombre'));
    });
   


    $(document).ready(function () {

    $('.btn-ajax-modal').on('click', function () {
        var url = $(this).data('url'); 
        
        $.get(url, function (htmlDevuelto) {
            $('#contenedorModales').html(htmlDevuelto);

            var form = $('#contenedorModales').find('form');
            if (form.length > 0) {
                $.validator.unobtrusive.parse(form[0]);
            }
      
            $.validator.unobtrusive.parse('#contenedorModales');
            
            $('#modalEdit').modal('show');
        });
    });
});
});