$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var messenger = $.connection.messengerHub;

    // Функция, вызываемая при подключении нового пользователя
    messenger.client.onConnected = function () {

        alert("Connect");
    }

    messenger.client.onUserDisconnected = function (id, userName) {

        alert("DISCONNECT");
    }

    // Открываем соединение
    $.connection.hub.start().done(function () {

        // обработка логина
        $("#btnLogin").click(function () {

            var name = $("#txtUserName").val();
            if (name.length > 0) {
                messenger.server.connect(name);
            }
            else {
                alert("Введите имя");
            }
        });
    });
});