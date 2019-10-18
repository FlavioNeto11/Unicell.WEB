var DatatablesDataSourceAjaxServer = {
    init: function () {
        var element = "#m_table_1";

        $(element).DataTable({
            responsive: !0,
            searchDelay: 500,
            processing: !0,
            serverSide: !0,
            ajax: {
                url: MapPath.GetMapPost,
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
                data: "ANDROID_ID"
            }, {
                    data: "NM_FUNCIONARIO"
            }, {
                    data: "GEO_LOCALIZACAO"
            }, {
                    data: "ULTIMO_ACESSO"
            }, {
                    data: "ANDROID_STATUS"
            }]
        });
    }
};
jQuery(document).ready(function () {
    DatatablesDataSourceAjaxServer.init();
});