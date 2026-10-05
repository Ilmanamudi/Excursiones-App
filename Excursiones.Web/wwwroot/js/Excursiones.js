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
        
       
        $('#detailsId').text('#' + button.data('id'));
        $('#detailsNombre').text(button.data('nombre'));
        $('#detailsFecha').text(button.data('fecha'));
        $('#detailsPrecio').text('$ ' + button.data('precio'));
        $('#detailsCupo').text(button.data('cupo') + ' pasajeros');
        $('#detailsDuracion').text(button.data('duracion') + ' días');
        $('#detailsDescripcion').text(button.data('descripcion') || 'Sin descripción detallada.');
        
        var idEstado = button.data('estado');
        var textoEstado = idEstado == 1 ? 'Programada' : (idEstado == 2 ? 'Completada' : 'Cancelada');
        $('#detailsEstado').text(textoEstado);
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