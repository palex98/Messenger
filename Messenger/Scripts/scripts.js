function ClickOnChat(chatId, sender, receiver, name) {
    if (chatId != window.currentChatId) {
        window.currentChatId = chatId;
        if (sender == window.myId) {
            window.currentUserId =  receiver;
        }
        else{
            window.currentUserId = sender;
        }
        SetChatterName(name);
        GetMessagesFromCurrentChat();
    }
    $("#lastSeenBlock").css("visibility", "visible");
    $("#sendField").css("visibility", "visible");
    $("#plsSelectLabel").css("visibility", "hidden");
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

function SetChatterName(name) {
            $("#chatterName").text(name);
}

function SendMessage() {
    var text = $("#message-area").val();
    $.ajax({
        type: "POST",
        url: '/Message/PostMessage',
        data: { text: text, sender: window.myId, receiver: window.currentUserId, chatId: window.currentChatId },
        success: function (data) {
            $("#messageBlock").append(data);
        }
    });
    $("#message-area").val('');
}