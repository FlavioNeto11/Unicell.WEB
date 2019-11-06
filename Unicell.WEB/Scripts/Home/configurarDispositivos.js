var element = "#table-dispositivos";
var elementAcesso = "#table-acessos";
function carregaGrid() {
    
    var table = $(element).DataTable({
        responsive: !0,
        searchDelay: 500,
        "language": {
            "lengthMenu": "Exibindo _MENU_ por pagina",
            "zeroRecords": "Não existem registros correspondentes ao filtro selecionado.",
            "info": "Exibindo _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "Essa consulta não retornou registros.",
            "infoFiltered": "(filtrado de _MAX_ registros)",
            "paginate": {
                "previous": "Anterior",
                "next": "Proximo"
            },
            "search": "Filtrar:"
        },

        serverSide: !0,
        ajax: {
            url: MapPath.GetMapPost,
            type: "POST",
            data: function (d) {
                return $.extend({}, d, {
                    "search": $('#div-dispositivos').find('.dataTables_filter input').val() || undefined
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
        "drawCallback": function (response) {
              
          
        },
        columns: [{
            data: "ANDROID_ID",
            title: "IMEI"
        }, {
            data: "NM_FUNCIONARIO",
            title: "Funcionario"
        }, {
            data: "ENDERECO",
            title: "Endereco"
        }, {
            data: "ULTIMO_ACESSO_STRING",
            title: "Acesso"
        }, {
            data: "ANDROID_STATUS",
            title: "Status"
        }, {
            "data": "ANDROID_ID", "title": "", "autowidth": true, "render": function (data, type, full, meta) {
                return '<i onclick="abreChat('+data+')" data-id="' + data + '" class="fa flaticon-chat chat-grid"></i>';
            }
        }, {
            "data": "ANDROID_ID", "title": "", "autowidth": true, "render": function (data, type, full, meta) {
                return '<i onclick="abreEdit(' + data +')" data-id="' + data + '" class="fa fa-pencil-alt edit-grid"></i>';
            }
        }]
    });

   

    var tableAcesso = $(elementAcesso).DataTable({
        responsive: !0,
        searchDelay: 500,
        "language": {
            "lengthMenu": "Exibindo _MENU_ por pagina",
            "zeroRecords": "Não existem registros correspondentes ao filtro selecionado.",
            "info": "Exibindo _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "Essa consulta não retornou registros.",
            "infoFiltered": "(filtrado de _MAX_ registros)",
            "paginate": {
                "previous": "Anterior",
                "next": "Proximo"
            },
            "search": "Filtrar:"
        },

        serverSide: !0,
        ajax: {
            url: MapPath.GetAcessos,
            type: "POST",
            data: function (d) {
                return $.extend({}, d, {
                    "search": $('#div-acessos').find('.dataTables_filter input').val() || undefined,
                    "androidID": androidID
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
        "drawCallback": function (response) {
       
        },

        columns: [{
            data: "ACESSO_STRING",
            title: "Acesso"
        }, {
                data: "DESCRICAO",
            title: "Descricao"
            }, {
                data: "PACKAGE_NAME",
                title: "Package"
            }, {
                "data": "DATA_COVER_SMALL", "title": "", "autowidth": true, "render": function (data, type, full, meta) {
                    return '<img style="width:36px;" src="' + ((data) ? data : "https://lh3.googleusercontent.com/v0Na9wm3dVQlJ6eiGchC8FoUVgnSY4ik0yOLZwyeX1iieMJ_byckvj9yEHp8IpVzCXty=s128-rw")+'" />';
                }
            }]
    });
}


var abreChat = function (data) {
    selectedANDROID_ID = data;
    $('#m_quick_sidebar_toggle').click();
};

var abreEdit = function (data) {

    androidID = data;
    $('#div-dispositivos').hide();
    $('#apps').show();
    carregaApps();
};


$(document).ready(function () {

    carregaGrid();


  

    $('#btnLookupUser').click(function () {
        loadApps();
    });

   

    $('#btnCancelar').click(function () {
        $('#div-dispositivos').show();
        $('#apps').hide();
    });
});

var template = `<div class="appContainer {containerClass}">
                <img src="{dataCoverSmall}" alt="{Descricao}" />
                <span></span>

                <span class="cardTitle" title="{Descricao}" aria-hidden="true" tabindex="-1">
                    {Descricao} <span class="paragraph-end"></span>
                </span>
            </div>`;



function render(props) {
    return function (tok, i) {
        return (i % 2) ? props[tok] : tok;
    };
}

function loadApps() {
    var app = $('#Aplicativo').val();

    if (app.trim() === '') {
        return;
    } else {

        $.ajax({
            url: MapPath.GetAPPs,
            type: 'POST',
            dataType: 'json',
            async: true,
            data: {
                nomeAplicativo: app,
                androidID: androidID
            },
            success: function (result) {

                loadAppsInner(result);


            },
            error: function (xhr, ajaxOptions, thrownError) {


            }
        });
    }
}

function galleryDragAndDrop(mainContainer, containerOne, containerTwo, dragItem) {
    setTimeout(function () {
        $(dragItem).draggable({
            revert: "invalid",
            containment: mainContainer,
            helper: "clone",
            cursor: "move",
            drag: function (event, ui) {
                $(ui.helper.prevObject).addClass("box_current");
            },
            stop: function (event, ui) {
                $(ui.helper.prevObject).removeClass("box_current");
            }
        });

        $(containerTwo).droppable({

            activeClass: "ui-state-highlight",
            drop: function (event, ui) {
                acceptBoxIngalleryTwo(ui.draggable);

                $(ui.draggable).addClass("container_two");
                $(ui.draggable).removeClass("container_one");
            },
            accept: function (draggable) {
                return $(draggable).hasClass("container_one");
            }
        });

        $(containerOne).droppable({

            activeClass: "ui-state-highlight",
            drop: function (event, ui) {
                acceptBoxIngalleryOne(ui.draggable);

                $(ui.draggable).addClass("container_one");
                $(ui.draggable).removeClass("container_two");
            },
            accept: function (draggable) {
                return $(draggable).hasClass("container_two");
            }
        });
    }, 500);

    function acceptBoxIngalleryTwo(item) {
        var el = JSON.parse($(item).data('full'));

        var autorizar = false;

        var executarBloqueio = function () {
            $.ajax({
                url: MapPath.SendApps,
                type: 'POST',
                dataType: 'json',
                async: true,
                data: {
                    androidID: androidID,
                    id_app: el.Id,
                    descricao: el.Descricao,
                    packageName: el.PackageName,
                    dataCoverLarge: el.dataCoverLarge,
                    dataCoverSmall: el.dataCoverSmall,
                    incluir: true,
                    autorizar: autorizar
                },
                success: function (result) {
                    $(item).fadeOut(function () {
                        $(containerTwo).append(item);
                    });

                    if (autorizar)
                        $(item).css('box-shadow', '0 2px 4px rgba(0,255,0,0.5)');
                    else
                        $(item).css('box-shadow', '0 2px 4px rgba(255,0,0,0.5)');

                    $(item).fadeIn();
                    $(item).removeClass("box_current");

                },
                error: function (xhr, ajaxOptions, thrownError) {


                }
            });
        };

        swal({
            title: 'Autorização',
            text: "Deseja bloquear ou desbloquear o aplicativo?",
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Desbloquear',
            cancelButtonColor: '#FF0000',
            cancelButtonText: 'Bloquear',
            reverseButtons: false
        }).then(function (result) {
            if (result.value) {
                autorizar = true;
                executarBloqueio();
            } else if (result.dismiss === 'cancel') {
                autorizar = false;
                executarBloqueio();
            }
        });
    }

    function acceptBoxIngalleryOne(item) {

        var el = JSON.parse($(item).data('full'));

        $.ajax({
            url: MapPath.SendApps,
            type: 'POST',
            dataType: 'json',
            async: true,
            data: {
                androidID: androidID,
                id_app: el.Id,
                descricao: el.Descricao,
                packageName: el.PackageName,
                dataCoverLarge: el.dataCoverLarge,
                dataCoverSmall: el.dataCoverSmall,
                incluir: false,
                autorizar: false
            },
            success: function (result) {
                $(item).fadeOut(function () {
                    $(containerOne).append(item);
                });

                $(item).fadeIn();
                $(item).removeClass("box_current");

                $(item).css('box-shadow', '0 2px 4px rgba(0,0,0,0.1)');

            },
            error: function (xhr, ajaxOptions, thrownError) {


            }
        });


    }
}

function loadAppsInner(result) {
    $('.list-items').empty();
    $('.list-items-autorizado').empty();

    var items = jQuery.parseJSON(result);

    $.each(items, function () {
        var el = this;

        var item = template
            .replace(/\{dataCoverSmall}/g, el.dataCoverSmall)
            .replace(/\{Descricao}/g, el.Descricao);

        if (el.Autorizado === null) {
            item = $.parseHTML(item.replace(/\{containerClass}/g, 'container_one'));
            $(item).data("full", JSON.stringify(el));
            $('.list-items').append(item);
        } else if (el.Autorizado === true) {
            item = $.parseHTML(item.replace(/\{containerClass}/g, 'container_two'));
            $(item).css('box-shadow', '0 2px 4px rgba(0,255,0,0.5)');
            $(item).data("full", JSON.stringify(el));
            $('.list-items-autorizado').append(item);
        } else if (el.Autorizado === false) {
            item = $.parseHTML(item.replace(/\{containerClass}/g, 'container_two'));
            $(item).css('box-shadow', '0 2px 4px rgba(255,0,0,0.5)');
            $(item).data("full", JSON.stringify(el));
            $('.list-items-autorizado').append(item);
        }

    });

    galleryDragAndDrop("#test-container", "#gallery-one", "#gallery-two", ".appContainer");
}

var carregaApps = function () {


    $(elementAcesso).DataTable().ajax.reload();


    $.ajax({
        url: MapPath.GetAppsList,
        type: 'POST',
        dataType: 'json',
        async: true,
        data: {
            androidID: androidID
        },
        success: function (result) {
            loadAppsInner(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {


        }
    });

};