var DatatablesDataSourceAjaxServer = {
    init: function () {
        var element = "#m_table_1";

        var table = $(element).DataTable({
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
                data: "NM_CARGO",
                title: "Cargo"
            }, {
                title: 'Ações',
                orderable: false,
                render: function (data, type, full, meta) {
                    return `
                    <span class="dropdown">
                        <a href="#" class="btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown" aria-expanded="true">
                            <i class="la la-ellipsis-h"></i>
                        </a>
                        <div class="dropdown-menu dropdown-menu-right">
                            <a class="dropdown-item" href="#"><i class="la la-edit"></i> Edit Details</a>
                            <a class="dropdown-item" href="#"><i class="la la-leaf"></i> Update Status</a>
                            <a class="dropdown-item" href="#"><i class="la la-print"></i> Generate Report</a>
                        </div>
                    </span>
                    <a href="#" class="m-portlet__nav-link btn m-btn m-btn--hover-brand m-btn--icon m-btn--icon-only m-btn--pill" title="View">
                        <i class="la la-edit"></i>
                    </a>`;
                }
             }]
        });

        $(element).find('tbody').on('click', 'tr', function () {
            var data = table.row(this).data();
          
        });
    }
};
jQuery(document).ready(function () {
    DatatablesDataSourceAjaxServer.init();
});