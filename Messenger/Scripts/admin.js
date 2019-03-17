function form_adding() {
    $('#add-user-block').css('display', 'block');
    $('#user-contacts').css('display', 'none');
    $('.right-part').css('display', 'block');
}

function form_contacts(userId) {
    window.currentUser = userId;

    $.ajax({
        type: "GET",
        url: "/Admin/GetContacts",
        data: { userId: userId },
        success: function (data) {
            $("#user-contacts").html(data);
        }
    });

    $('.right-part').css('display', 'block');
    $('#add-user-block').css('display', 'none');
    $('#user-contacts').css('display', 'block');
}

function CloseContactsForm() {
    $('.right-part').css('display', 'none');
    $('#user-contacts').css('display', 'none');
}

function DeleteUser(userId) {
    $.ajax({
        type: "DELETE",
        url: "/api/User",
        data: { userId: userId },
        success: function () {
            ReloadPage();
        }
    });
}

function ChangePassword(userId) {
    var password = $("#pass_" + userId).val();

    $.ajax({
        type: "PUT",
        url: "/api/User",
        data: { userId: userId, password: password },
        success: function () {
            ReloadPage();
        }
    });
}

function ReloadPage() {
    window.location.reload(false);
}

function ChangeContact(contactId) {
    $.ajax({
        type: "POST",
        url: "/api/User",
        data: { userId: window.currentUser, contactId: contactId }
    });
}