//== Class definition

var selectedANDROID_ID;
var usuarios;
var markers = [];
var firstTime = true;

var _GoogleMaps = function () {

    var map = new GMaps({
        div: '#m_gmap_3',
        lat: -23.3542983,
        lng: -48.6814203
    });

    var initMaps = function () {
        loadCoord();
    };

    var loadCoord = function () {

        var mapTemplate = + ' <div class="m-widget24"> '
                          + '		<div class="m-widget24__item"> '
                          + '			<h4 class="m-widget24__title"> '
                          + '				{0} '
                          + '			</h4> '
                          + '			<br> '
                          + '			<span class="m-widget24__desc"> '
                          + '				{3} '
                          + '			</span> '
                          + '			<div class="m--space-10"></div> '
                          + '			<div class="progress m-progress--sm"> '
                          + '				<div class="progress-bar m--bg-brand" role="progressbar" style="width: {1}%;" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div> '
                          + '			</div> '
                          + '			<span class="m-widget24__change"> '
                          + '				Bateria '
                          + '			</span> '
                          + '			<span class="m-widget24__number"> '
                          + '				{2}% '
                          + '			</span> '
                          + '			<div class="m--space-10"></div> '
                          + '			<div class="progress m-progress--sm"> '
                          + '				<div class="progress-bar m--bg-brand" role="progressbar" style="width: {4}%;" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div> '
                          + '			</div> '
                          + '			<span class="m-widget24__change"> '
                          + '				Sinal '
                          + '			</span> '
                          + '			<span class="m-widget24__number"> '
                          + '				{5}% '
                          + '			</span> '
                          + '		</div> '
            + '	</div> ';

        var getMapTemplate = function (el) {
            return mapTemplate
                .replace('{0}', (el.NM_FUNCIONARIO) ? el.NM_FUNCIONARIO : el.ANDROID_ID)
                .replace('{1}', el.CHARGELEVEL * 100)
                .replace('{2}', el.CHARGELEVEL * 100)
                .replace('{3}', (el.ISCHARGING) ? 'Carregando' : 'Na bateria')
                .replace('{4}', el.SIGNALSTRENGTH * 25)
                .replace('{5}', el.SIGNALSTRENGTH * 25);
        };

        var data = {
            search: $('#searchBox').val()
        };

        $.ajax({
            type: "POST",
            url: MapPath.GetMapPost,
            data: JSON.stringify(data),
            dataType: 'json',
            contentType: 'application/json',
            success: function (result) {

                if (firstTime)
                    $('#m_gmap_3').height($(window).height());

                $.each(markers, function () {
                    var _m = this;
                    if (!result.data.find(item => item.ANDROID_ID === _m.details.androidID)) {
                        _m.setMap(null);

                        markers = jQuery.grep(markers, function (value) {
                            return value !== _m;
                        });
                    }
                });

                $.each(result.data, function () {
                    var el = this;

                    var lat = parseFloat(el.GEO_LOCALIZACAO.split(':')[0]);
                    var lng = parseFloat(el.GEO_LOCALIZACAO.split(':')[1]);

                    if (el.GEO_LOCALIZACAO.split(':').length > 1) {
                        var marker = markers.find(item => item.details.androidID === el.ANDROID_ID);

                        if (marker) {
                            var latlng = new google.maps.LatLng(lat, lng);
                            marker.setPosition(latlng);

                            var infowindow = new google.maps.InfoWindow({
                                content: getMapTemplate(el)
                            });

                            google.maps.event.clearInstanceListeners(marker);

                            google.maps.event.addListener(marker, 'click', function () {
                                infowindow.open(map, marker);
                            });

                        }
                        else {
                            var _marker = map.addMarker({
                                lat: lat,
                                lng: lng,
                                title: (el.NM_FUNCIONARIO) ? el.NM_FUNCIONARIO : el.ANDROID_ID,
                                
                                details: {
                                    androidID: el.ANDROID_ID
                                },
                                click: function (e) {
                                    if (console.log) console.log(e);
                                    selectedANDROID_ID = e.details.androidID;
                                }
                            });

                            var _infowindow = new google.maps.InfoWindow({
                                content: getMapTemplate(el)
                            });

                            google.maps.event.addListener(_marker, 'click', function () {
                                _infowindow.open(map, _marker);
                            });

                            markers.push(_marker);
                        }
                    }
                });

                if (firstTime)
                    map.setCenter(result.data[0].GEO_LOCALIZACAO.split(':')[0], result.data[0].GEO_LOCALIZACAO.split(':')[1]);

                if (firstTime)
                    map.setZoom(8);

                firstTime = false;

                loadCoord();
            },
            error: function (xhr, status, error) {
                console.log(xhr.statusText);

                $.each(markers, function () {
                    var _m = this;
                   
                    _m.setMap(null);

                    markers = jQuery.grep(markers, function (value) {
                        return value !== _m;
                    });
                  
                });

                loadCoord();
            }
        });
    };

return {
    init: function () {
        initMaps();
    }
};
}();

jQuery(document).ready(function () {
    _GoogleMaps.init();

    let templateMensagemTerceiro = '<div class="m-messenger__wrapper"> ' +
        '    <div class="m-messenger__message m-messenger__message--in">' +
        '        <div class="m-messenger__message-pic">' +
        '            <img src="../dist/default/assets/app/media/img//users/user3.jpg" alt="" />' +
        '        </div>' +
        '        <div class="m-messenger__message-body">' +
        '            <div class="m-messenger__message-arrow"></div>' +
        '            <div class="m-messenger__message-content">' +
        '                <div class="m-messenger__message-username">' +
        '                    {remetente} wrote' +
        '                </div>' +
        '                <div class="m-messenger__message-text">' +
        '                    {mensagem} ' +
        '                </div>' +
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>';

    let templateMensagem = ' <div class="m-messenger__wrapper">' +
        '    <div class="m-messenger__message m-messenger__message--out">' +
        '        <div class="m-messenger__message-body">' +
        '            <div class="m-messenger__message-arrow"></div>' +
        '            <div class="m-messenger__message-content">' +
        '                <div class="m-messenger__message-text">' +
        '                    {mensagem} ' +
        '                </div>' +
        '            </div>' +
        '        </div>' +
        '    </div>' +
        '</div>';

    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;

    // Create a function that the hub can call back to display messages.
    chat.client.sendMessage = function (message, name) {
        // Add the message to the page.
        if (name !== displayname)
            $('#mensagens').append(templateMensagemTerceiro.replace('{remetente}', name).replace('{mensagem}', message));
        else
            $('#mensagens').append(templateMensagem.replace('{mensagem}', message));
    };

    // Set initial focus to message input box.
    $('#messageText').focus();

    $.connection.hub.qs = { 'username': displayname };

    var refreshUsuarios = function () {
        chat.server.getUsers().done(function (connections) {
            usuarios = connections;

            refreshUsuarios();
        });
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        refreshUsuarios();

        $('#messageText').keydown(function (e) {
            if (e.keyCode === 13) {

                $.map(usuarios, function (item) {
                    if (item.username === selectedANDROID_ID) {
                        chat.server.sendMessage($('#messageText').val(), item.connectionID);
                        // Clear text box and reset focus for next comment.
                        $('#messageText').val('').focus();
                    }
                });



            }
        });
    });

});



// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

