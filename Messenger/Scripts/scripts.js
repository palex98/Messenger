
function ClickOnChat(chatId, receiver, name) {
    if (chatId != window.currentChatId) {
        window.currentChatId = chatId;
        window.currentReceiver = receiver;
        SetChatterName(name);
        GetMessagesFromCurrentChat();
        GetUserStatus(receiver);
        $(".spinner-border").css('visibility', 'visible');

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
        data: { chatId: window.currentChatId, myId: window.myId },
        success: function (data) {
            $("#messageBlock").empty();
            $("#messageBlock").html(data);
            $(".spinner-border").css('visibility', 'hidden');
              ToLastMessage();
        }

    });
}

function GetLastMessage(chatId) {
    $.ajax({
        type: "POST",
        url: '/Message/GetLastMessage',
        data: { chatId: chatId, myId: window.myId },
        success: function (data) {
            $("#messageBlock").append(data);
            ToLastMessage();
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
                ToLastMessage();
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
        success: function (data) {
            $("#messageBlock").append(data);
            alert("Файл успешно отправлен!");
            ToLastMessage();
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
};

function SoundClick() {
    var sound_color = $("#sound-link").html();
    console.log(sound_color);
    if (sound_color == "Sound on") {
        $("#sound-link").html("Sound off");
        $("#sound-link").css('color', '#f23c34');
    }
    else {
        $("#sound-link").html("Sound on");
        $("#sound-link").css('color', "#75c12a");
    }
};