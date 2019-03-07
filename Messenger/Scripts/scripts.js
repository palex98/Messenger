function ClickOnChat(chatId, sender, name) {
    if (chatId != window.currentChatId) {
        window.currentChatId = chatId;
        /*if (sender == window.myId) {
            window.currentUserId = receiver;
        }
        else {
            window.currentUserId = sender;
        }*/
        SetChatterName(name);
        GetMessagesFromCurrentChat();
    }
    $("#lastSeenBlock").css("visibility", "visible");
    $("#sendField").css("visibility", "visible");
    $("#plsSelectLabel").css("visibility", "hidden");
    $(".block-chatter").removeClass('active-chatter');
    $("#" + chatId).toggleClass("active-chatter");
    $("#" + chatId + " .chat-counter").text("0");
    $("#" + chatId + " .chat-counter").css('visibility', 'hidden');
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
    if ($('#message-area').val() == '');
    else {
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
        CheckCounter();    
    }
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

function CheckCounter() {
    if ($('.chat-counter').html() == '0') {
        $('.chat-counter').css('visibility', 'hidden');
    }
    else {
        $('.chat-counter').css('visibility', 'visible');
    }
}
/* delete comment if you want counter to work*/
//CheckCounter();

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