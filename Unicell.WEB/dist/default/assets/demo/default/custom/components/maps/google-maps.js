//== Class definition
var _GoogleMaps = function () {
    var initMaps = function () {
        var map = new GMaps({
            div: '#m_gmap_3'
        });

        $('#m_gmap_3').height($(window).height());
       
        $.ajax({
            type: "POST",
            url: MapPath.GetMapPost,
            dataType: 'json',
            contentType: 'application/json',
            success: function (result) {
                $.each(result, function () {
                    var el = this;
                    map.addMarker({
                        lat: el.GeoLocalizacao.split(':')[0],
                        lng: el.GeoLocalizacao.split(':')[1],
                        title: el.NomeFuncionario,
                        infoWindow: {
                            content: '<span style="color:#000">HTML Content!</span>'
                        },
                        details: {
                            database_id: 42,
                            author: 'HPNeo'
                        },
                        click: function (e) {
                            if (console.log) console.log(e);
                            alert('You clicked in this marker');
                        }
                    });
                });

                GMaps.geolocate({
                    success: function (position) {
                        map.setCenter(result[0].GeoLocalizacao.split(':')[0], result[0].GeoLocalizacao.split(':')[1]);
                    },
                    error: function (error) {
                        alert('Geolocation failed: ' + error.message);
                    },
                    not_supported: function () {
                        alert("Your browser does not support geolocation");
                    },
                    always: function () {
                        //alert("Geolocation Done!");
                    }
                });
            },
            error: function (xhr, status, error) {
                console.log(xhr.statusText);
            }
        });

        map.setZoom(8);
    };

    return {
        init: function () {
            initMaps();
        }
    };
}();

jQuery(document).ready(function () {
    _GoogleMaps.init();
});