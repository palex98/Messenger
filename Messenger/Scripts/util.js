$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var messenger = $.connection.messengerHub;

    // Функция, вызываемая при подключении нового пользователя
    messenger.client.onUserConnected = function () {

        alert("Connect");
    };

    messenger.client.onUserDisconnected = function (id, userName) {

        alert("DISCONNECT");
    };

    messenger.client.newMessage = function (chatId) {
        if (chatId == window.currentChatId) {
            GetLastMessage(chatId);
        } else {
            $("#" + chatId + " .chat-counter").css('visibility', 'visible');
            var counter = +$("#" + chatId + " .chat-counter").text();
            counter++;
            $("#" + chatId + " .chat-counter").text(counter);
        }
    };

    // Открываем соединение
    $.connection.hub.start().done(function () {
        alert('start');
        var userId = getCookie("user");
        messenger.server.connect(userId);
    });
});