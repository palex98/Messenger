function ClickOnChat(chatId, receiver, name) {
    if (chatId != window.currentChatId) {
        window.currentChatId = chatId;
        window.currentReceiver = receiver;
        SetChatterName(name);
        GetMessagesFromCurrentChat();
        GetUserStatus(receiver);
    }
    $("#lastSeenBlock").css("visibility", "visible");
    $("#sendField").css("visibility", "visible");
    $("#plsSelectLabel").css("visibility", "hidden");
    $(".block-chatter").removeClass('active-chatter');
    $("#" + chatId).toggleClass("active-chatter");
    $("#" + chatId + " .chat-counter").text("0");
    $("#" + chatId + " .chat-counter").css('visibility', 'hidden');
}

function GetUserStatus(userId) {
    $.ajax({
        type: "GET",
        url: '/api/User',
        data: { userId: userId },
        success: function (data) {
            $("#lastSeen").text(data);
        }
    });
}

function GetMessagesFromCurrentChat() {
    $.ajax({
        type: "POST",
        url: '/Message/GetMessages',
        data: { chatId: window.currentChatId },
        success: function (data) {
            $("#messageBlock").empty();
            $("#messageBlock").html(data);
        }
    });
}

function GetLastMessage(chatId) {
    $.ajax({
        type: "POST",
        url: '/Message/GetLastMessage',
        data: { chatId: chatId },
        success: function (data) {
            $("#messageBlock").append(data);
        }
    });
}

function SetChatterName(name) {
    $("#chatterName").text(name);
}

function SendMessage() {
    if ($('#message-area').val() != '') {
        var text = $("#message-area").val();
        $.ajax({
            type: "POST",
            url: '/Message/PostMessage',
            data: { text: text, sender: window.myId, chatId: window.currentChatId },
            success: function (data) {
                $("#messageBlock").append(data);
            }
        });
        $("#message-area").val('');
    }
}

function SendFile() {
    var files = $('input[type=file]')[0].files;

    var data = new FormData();
    for (var x = 0; x < files.length; x++) {
        data.append("file" + x, files[x]);
    }

    data.append("myId", window.myId);
    data.append("chatId", window.currentChatId);

    $.ajax({
        type: "POST",
        url: "/Message/PostFileMessage",
        contentType: false,
        processData: false,
        data: data,
        success: function (result) {
            alert("Файл успешно отправлен!");
        },
        error: function (result) {
            alert("Ошибка при отправке файла!");
        }
    });
}

function ToLastMessage() {
    var div = $("#right-col");
    div.scrollTop(div.prop('scrollHeight'));
}

$('#message-area').on('keypress', function (e) {
    if (e.which == 13) {
        e.preventDefault();
        SendMessage();
    }
})

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}