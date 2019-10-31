"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/clientHub").build();

var estadoBotao = false;

connection.on("RecebeEstadoServo", function (potenciometro) {
    $('.potenciometro').text(potenciometro);
});

connection.on("RecebeEstadoLed", function (ligado) {
    if (ligado) {
        $('.btn-off').hide();
        $('.btn-on').show();
    } else {
        $('.btn-on').hide();
        $('.btn-off').show();
    }
});

connection.start().then(function () {
    console.log('Hub conectado.');
}).catch(function (err) {
    return console.error(err.toString());
});

$('.btn-liga-led').click(function () {
    $(this).text(estadoBotao ? 'Liga LED' : 'Desliga LED');

    estadoBotao = !estadoBotao;

    connection.invoke("EstadoBotao", estadoBotao).catch(function (err) {
        return console.error(err.toString());
    });
});


$('.slider').change(function () {

    var angulo = $(this).val();

    connection.invoke("EstadoSlider", angulo).catch(function (err) {
        return console.error(err.toString());
    });
});
