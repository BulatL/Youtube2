$(document).ready(function (e) {
    $.get('loggedInUser', function (data) {
        if (data == null) {
            var navbarLogin = $('#navbarLogin');
            navbarLogin.append(
                '<li><a href="http://localhost:55157/Youtube/register"><span class="glyphicon glyphicon-user"></span> Register</a></li>' +
                '<li><a href="http://localhost:55157/Youtube/login"><span class="glyphicon glyphicon-log-out"></span> Login</a></li>'
            );
        }
        else {
            var navbarHeader = $('.navbar-header');
            navbarHeader.append(
                '<a href="http://localhost:55157/Youtube/admin" class="navbar-brand">Admin page</a>'
            );
            var navbarLogin = $('#navbarLogin');
            navbarLogin.append(
                '<li><a href="http://localhost:55157/Youtube/users/' + data.id + '"><span class="glyphicon glyphicon-user"></span> My channel</a></li>' +
                '<li><a href="http://localhost:55157/Youtube/logout"><span class="glyphicon glyphicon-log-out"></span> Log out</a></li>'
            );
        }
    });
});

function searchAll() {
    var searchInput = $('#searchInput');
    var searchText = searchInput.val().trim();

    var url = 'search/' + searchText;
    window.location.href = url;
};