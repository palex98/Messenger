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
    $(".block-chatter").removeClass('active-chatter');
       $("#" + chatId).toggleClass("active-chatter");
};

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
};

function SetChatterName(name) {
            $("#chatterName").text(name);
};

function SendMessage() {
    if ($('#message-area').val() == '') ;
    else {
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
        CheckCounter();
        
    }
};

function SearchMessages() {
    var searchRequest = $("#searchMsgInput").val();

    $.ajax({
        type: "POST",
        url: '/Message/SearchMessages',
        data: { chatId: window.currentChatId, searchRequest: searchRequest },
        success: function (data) {
            $("#messageBlock").empty().append(data);
        }
    });
};

function ToLastMessage() {
    var div = $("#right-col");
    div.scrollTop(div.prop('scrollHeight'));
};

$('#message-area').on('keypress', function (e) {
    if (e.which == 13) {
        e.preventDefault();
        SendMessage();
    }
});


function CheckCounter() {
    if ($('.chat-counter').html() == '0') {
        $('.chat-counter').css('visibility', 'hidden');
    }
    else {
        $('.chat-counter').css('visibility', 'visible');
    }
};
/* delete comment if you want counter to work*/
//CheckCounter();