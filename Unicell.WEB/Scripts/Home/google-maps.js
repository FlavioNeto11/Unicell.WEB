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
        $.ajax({
            type: "POST",
            url: MapPath.GetMapPost,
            dataType: 'json',
            contentType: 'application/json',
            success: function (result) {

                if (firstTime)
                    $('#m_gmap_3').height($(window).height());

                $.each(result.data, function () {
                    var el = this;

                    var lat = parseFloat(el.GEO_LOCALIZACAO.split(':')[0]);
                    var lng = parseFloat(el.GEO_LOCALIZACAO.split(':')[1]);

                    if (el.GEO_LOCALIZACAO.split(':').length > 1) {
                        var marker = markers.find(item => item.details.androidID === el.ANDROID_ID);

                        if (marker) {
                            var latlng = new google.maps.LatLng(lat, lng);
                            marker.setPosition(latlng);
                        }
                        else {
                            markers.push(
                                map.addMarker({
                                    lat: lat,
                                    lng: lng,
                                    title: el.NM_FUNCIONARIO,
                                    infoWindow: {
                                        content: '<span style="color:#000">' + el.NM_FUNCIONARIO + '</span>'
                                    },
                                    details: {
                                        androidID: el.ANDROID_ID
                                    },
                                    click: function (e) {
                                        if (console.log) console.log(e);
                                        selectedANDROID_ID = e.details.androidID;
                                    }
                                }));
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
        if (name != displayname)
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
            if (e.keyCode == 13) {

                $.map(usuarios, function (item) {
                    if (item.username == selectedANDROID_ID) {
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

