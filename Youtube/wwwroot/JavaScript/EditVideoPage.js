$(document).ready(function (e) {
    $.get('http://localhost:55157/Youtube/loggedInUser', function (data) {
        if (data == null) {
            alert("You can't access this page")
            history.back();
        }
        else if (data.blocked == true) {
            alert("You can't access this page")
            history.back();
        }
    });
});

function cancel(id) {
    window.history.back()
}