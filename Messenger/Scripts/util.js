$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var messenger = $.connection.messengerHub;

    // Функция, вызываемая при подключении нового пользователя
    messenger.client.onUserConnected = function (userId) {
        if (window.currentReceiver == userId) {
            GetUserStatus(userId);
        }
    };

    messenger.client.onUserDisconnected = function (userId) {
        if (window.currentReceiver == userId) {
            GetUserStatus(userId);
        }
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

    $.connection.hub.start().done(function () {
        var userId = getCookie("user");
        messenger.server.connect(userId);
    });
});