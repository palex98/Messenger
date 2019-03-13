$(function () {
    var messenger = $.connection.messengerHub;

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
            ReadMessages();
        } else {
            $("#" + chatId + " .chat-counter").css('visibility', 'visible');
            var counter = +$("#" + chatId + " .chat-counter").text();
            counter++;
            $("#" + chatId + " .chat-counter").text(counter);
        }
    };

    messenger.client.readMessages = function (chatId) {

        if (chatId == window.currentChatId) {
            $(".message-unread-circle").each(function () {
                $(this).removeClass("message-unread-circle");
            });
        }
    };

    $.connection.hub.start().done(function () {
        var userId = getCookie("user");
        messenger.server.connect(userId);
    });
});