var selectedID = null;

var DatatablesDataSourceAjaxServer = {
    init: function () {
        var element = "#m_table_1";
      

        var table = $(element).DataTable({
            responsive: !0,
            searchDelay: 500,
            processing: !0,
            serverSide: !0,
            "language": {
                "lengthMenu": "Exibindo _MENU_ por página",
                "zeroRecords": "Não existem registros correspondentes ao filtro selecionado.",
                "info": "Exibindo página _PAGE_ de _PAGES_",
                "infoEmpty": "Essa consulta não retornou registros.",
                "infoFiltered": "(filtrado de _MAX_ registros)",
                "paginate": {
                    "previous": "Anterior",
                    "next": "Próximo"
                },
                "search": "Filtrar:"
            },
            "lengthMenu": [[5, 10, 25, 50], [5, 10, 25, 50]],
            drawCallback: function (response) {
                $('.editarRegistro').click(function () {
                    var el = this;
                    selectedID = $(el).data('full').ID_CARGO;

                    $('#nmCargo').val($(el).data('full').NM_CARGO);

                    $('#tableRegion').hide();
                    $('#m_form_1').show();
                });

                $('.removerRegistro').click(function () {
                    var el = this;

                    swal({
                        title: 'Você tem certeza?',
                        text: "Você 'não será capaz de reverter isso!",
                        type: 'warning',
                        showCancelButton: true,
                        confirmButtonText: 'Sim, delete isso!',
                        cancelButtonText: 'Não, cancele!',
                        reverseButtons: true
                    }).then(function (result) {
                        if (result.value) {

                            var data = {
                                ID: $(el).data('full').ID_CARGO
                            };

                            $.ajax({
                                type: "POST",
                                url: MapPath.SetCargo,
                                data: JSON.stringify(data),
                                dataType: 'json',
                                contentType: 'application/json',
                                success: function (result) {
                                    swal(
                                        'Removido!',
                                        'Registro removido com sucesso.',
                                        'success'
                                    );

                                    $('#m_table_1').DataTable().ajax.reload();
                                },
                                error: function (xhr, status, error) {
                                    console.log(xhr.statusText);
                                }
                            });
                        } else if (result.dismiss === 'cancel') {
                            swal(
                                'Cancelado',
                                'Seu registro não foi removido :)',
                                'error'
                            );
                        }
                    });
                });
            },
            ajax: {
                url: MapPath.GetCargo,
                type: "POST",
                data: function (d) {
                    return $.extend({}, d, {
                        "search": $('.dataTables_filter input').val() || undefined
                    });
                },
                error: function (jqXHR, exception) {
                },
                statusCode: {
                    200: function () {
                        //console.log('OK 200');
                    },
                    204: function () {
                        console.log('Empty 204');
                    },
                    400: function (e) {
                        console.log('Bad Request 400');
                        $(element).find('tbody')
                            .empty()
                            .append('<tr><td colspan="6" class="dataTables_empty">' + e.statusText + '</td></tr>');
                    },
                    500: function (e) {
                        console.log('Internal server error 500');
                        $(element).find('tbody')
                            .empty()
                            .append('<tr><td colspan="6" class="dataTables_empty">' + e.statusText + '</td></tr>');
                    }
                },
                dataType: "json"
            },
            
            columns: [{
                data: "NM_CARGO",
                title: "Cargo"
            }, {
                data: "NM_CARGO",
                title: ''
            }],
            columnDefs: [
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    render: function (data, type, full, meta) {
                        return `
                        <a href="#" data-full='`+JSON.stringify(full)+`' class="editarRegistro m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="View">
                          <i class="la la-edit"></i>
                        </a>
                        <a href="#" data-full='`+ JSON.stringify(full) +`' class="removerRegistro m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="View">
                          <i class="la la-remove"></i>
                        </a>`;
                    },
             }]
        });
    }
};

var FormControls = function () {
    //== Private functions

    var demo1 = function () {
        $("#m_form_1").validate({
            // define validation rules
            rules: {
                nmCargo: {
                    required: true
                }
            },

            //display error alert on form submit  
            invalidHandler: function (event, validator) {
                var alert = $('#m_form_1_msg');
                alert.removeClass('m--hide').show();
                mUtil.scrollTop();
            },

            submitHandler: function (form) {
                var data = {
                    ID: selectedID,
                    NM_CARGO: $('#nmCargo').val()
                };

                $.ajax({
                    type: "POST",
                    url: MapPath.SetCargo,
                    data: JSON.stringify(data),
                    dataType: 'json',
                    contentType: 'application/json',
                    success: function (result) {
                        $('#tableRegion').show();
                        $('#m_form_1').hide();

                        $('#m_table_1').DataTable().ajax.reload();
                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.statusText);
                    }
                });
            }
        });
    };

    return {
        // public functions
        init: function () {
            demo1();
        }
    };
}();

jQuery(document).ready(function () {
    DatatablesDataSourceAjaxServer.init();
    FormControls.init();

    $('#btnEnviar').click(function () {
        if ($('#m_form_1').valid()) {
            $('#m_form_1').submit();
        }
    });

    $('#btnNovoRegistro').click(function () {
        $('#nmCargo').val('');
        selectedID = null;

        $('#tableRegion').hide();
        $('#m_form_1').show();
    });

    $('#btnCancelar').click(function () {
        $('#tableRegion').show();
        $('#m_form_1').hide();
    });

});