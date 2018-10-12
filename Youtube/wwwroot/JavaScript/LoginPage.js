function login() {
    var userNameInput = $('#userNameInput');
    var passwordInput = $('#passwordInput');

    var userName = userNameInput.val().trim();
    var password = passwordInput.val().trim();

    if (userName == "" || password == "") {
        alert("All fields must be filled.")
        return;
    }
    $.ajax({
        url: 'login',
        type: 'post',
        dataType: 'json',
        data: {
            "username": userName,
            "password": password
        },
        success: function (response) {
            window.location.href = "http://localhost:55157/Youtube/Home";
        },
        error: function (response, jqXHR) {
            alert("Wrong username/password");
        }
    });
}