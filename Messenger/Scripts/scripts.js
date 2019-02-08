function ClickOnChat(chatId, sender, receiver) {
    if (chatId != window.currentChatId) {
        window.currentChatId = chatId;
        if (sender == window.myId) {
            window.currentUserId =  receiver;
        }
        else{
            window.currentUserId = sender;
        }
        GetChatterName();
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

function GetChatterName() {
    $.ajax({
        type: "GET",
        url: '/api/user/GetName',
        data: { id: window.currentUserId },
        success: function (data) {
            $("#chatterName").text(data);
        }
    });
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

window.onload = function () {
    window.myId = 2;
};