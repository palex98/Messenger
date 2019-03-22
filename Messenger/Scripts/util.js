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
            window.messageCounter += 1;
        } else {
            $("#" + chatId + " .chat-counter").css('visibility', 'visible');
            var counter = +$("#" + chatId + " .chat-counter").text();
            counter++;
            $("#" + chatId + " .chat-counter").text(counter);
        }
        if (window.sound) NotificationSound();
        $(".block-chatter").each(function () {
            var currentCount = $(this).css('order');
            currentCount++;
            $(this).css('order', currentCount);
        });
        $("#" + chatId).css('order', '1');
    };

    messenger.client.readMessages = function (chatId) {

        if (chatId == window.currentChatId) {
            $(".message-unread-circle").each(function () {
                $(this).removeClass("message-unread-circle");
            });
        }
    };

    messenger.client.plusCounter = function (chatId, num) {

        if (chatId == window.currentChatId) {
            window.messageCounter += num;
        }
    };

    $.connection.hub.start().done(function () {
        var userId = getCookie("user");
        messenger.server.connect(userId);
    });
});