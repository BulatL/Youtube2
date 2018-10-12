
$(document).ready(function (e) {
    $.get('http://localhost:55157/Youtube/loggedInUser', function (data) {
        if (data == null) {

        }
        else {
            var navbarHeader = $('.navbar-header');
            navbarHeader.append(
                '<a href="http://localhost:55157/Youtube/admin" class="navbar-brand">Admin page</a>'
            );
            var navbarLogin = $('#navbarLogin');
            navbarLogin.append(
                '<li><a href="http://localhost:55157/Youtube/users/' + data.id + '"><span class="glyphicon glyphicon-user"></span> My channel</a></li>' +
                '<li><a href="http://localhost:55157/Youtube/logout" ><span class="glyphicon glyphicon-log-out"></span> Log out</a></li>'
            );
        }
    });

    $.get('http://localhost:55157/Youtube/users', function (data) {
        if (data != null) {
            var userDiv = $('#userDiv');
            for (i = 0; i < data.length; i++) {
                userDiv.append(
                    '<div class="col-lg-3">' +
                    '<div class="container" id="containerUser">' +
                    '<div class="container">' +
                    '<a href="http://localhost:55157/Youtube/users/' + data[i].id + '">' +
                    '<img src="http://localhost:55157/Youtube/uploads/' + data[i].profilePictureUrl + '" class="imgUser">' +
                    '<div class="cufCaption">' +
                    '<h3>' + data[i].username + '</h3>' +
                    '</div>' +
                    '</a>' +
                    '</div>' +
                    '<div class="container" id="' + data[i].username + '">' +
                    '<button class="btn btn-danger" id="btnDelete" onclick="deleteUser(' + data[i].id + ')">Delete</button>' +
                    '</div>' +

                    '<div class="container" id="userInfo">' +
                    '<h3>First name: ' + data[i].firstName + '</h3>' +
                    '<h3>Last name: ' + data[i].lastName + '</h3>' +
                    '<h3>Email: ' + data[i].email + '</h3>' +
                    '<h3>Role: ' + data[i].role + '</h3>' +
                    '<h3>Block: ' + data[i].blocked + '</h3><hr>' +
                    '</div>' +
                    '</div>' +
                    '</div>'
                );
                var userButtonsDiv = $('#' + data[i].username);
                if (data[i].blocked == true) {
                    userButtonsDiv.append(
                        '<button class="btn btn-danger"id="btnBlock" onclick="unblockUser(' + data[i].id + ')">Unblock</button>'
                    );
                } else {
                    userButtonsDiv.append(
                        '<button class="btn btn-danger" id="btnBlock" onclick="blockUser(' + data[i].id + ')">Block</button>'
                    );
                }
                if (data[i].role != "Admin") {
                    userButtonsDiv.append(
                        '<button class="btn btn-primary" id="btnPromote" onclick="promoteUser(' + data[i].id + ')">Promote</button>'
                    );

                }
            }
        }
    });
});

function deleteUser(id) {
    var r = confirm("Are u sure u want to delete this user");
    if (r == true) {
        $.ajax({
            url: 'users/delete/' + id,
            type: 'Delete',
            dataType: 'json',
            success: function (response) {
                alert(response)
                if (response == "Success") {
                    location.reload();
                }


            },
            error: function (response) {
                alert("Something went wrong");
            }
        });
    }
}

function promoteUser(id) {
    var r = confirm("Are u sure u want to promote this user");
    if (r == true) {
        $.ajax({
            url: 'users/promote/' + id,
            type: 'patch',
            dataType: 'json',
            success: function (response) {
                alert(response)
                if (response == "Success") {
                    location.reload();
                }


            },
            error: function (response) {
                alert("Something went wrong");
            }
        });
    }
}
function blockUser(id) {
    var r = confirm("Are u sure u want to promote this user");
    if (r == true) {
        $.ajax({
            url: 'users/block/' + id,
            type: 'patch',
            dataType: 'json',
            success: function (response) {
                alert(response)
                if (response == "Success") {
                    location.reload();
                }


            },
            error: function (response) {
                alert("Something went wrong");
            }
        });
    }
}
function unblockUser(id) {
    var r = confirm("Are u sure u want to promote this user");
    if (r == true) {
        $.ajax({
            url: 'users/unblock/' + id,
            type: 'patch',
            dataType: 'json',
            success: function (response) {
                alert(response)
                if (response == "Success") {
                    location.reload();
                }


            },
            error: function (response) {
                alert("Something went wrong");
            }
        });
    }
}

function searchUsers() {
    var searchInput = $('#searchInput');
    var searchText = searchInput.val().trim();
    var sortBy = "";
    var ascDesc = "";

    sortBy = $('input[name=orderBy]:checked').val();
    ascDesc = $('input[name=ascDesc]:checked').val();

    $.ajax({
        url: 'users/search',
        dataType: 'json',
        contentType: "application/json",
        type: 'get',
        data: {
            'searchText': searchText,
            'sortBy': sortBy,
            'ascDesc': ascDesc
        },
        success: function (response) {
            fillDiv(response);
        },
        error: function (response) {
        }
    });
}

function fillDiv(data) {
    var userDiv = $('#userDiv');
    userDiv.empty();
    for (i = 0; i < data.length; i++) {
        userDiv.append(
            '<div class="col-lg-3">' +
            '<div class="container" id="containerUser">' +
            '<div class="container">' +
            '<a href="http://localhost:55157/Youtube/users/' + data[i].id + '">' +
            '<img src="/Youtube/uploads/' + data[i].profilePictureUrl + '" class="imgUser">' +
            '<div class="cufCaption">' +
            '<h3>' + data[i].username + '</h3>' +
            '</div>' +
            '</a>' +
            '</div>' +
            '<div class="container" id="' + data[i].username + '">' +
            '<button class="btn btn-danger" id="btnDelete" onclick="deleteUser(' + data[i].id + ')">Delete</button>' +
            '</div>' +

            '<div class="container" id="userInfo">' +
            '<h3>First name: ' + data[i].firstName + '</h3>' +
            '<h3>Last name: ' + data[i].lastName + '</h3>' +
            '<h3>Email: ' + data[i].email + '</h3>' +
            '<h3>Role: ' + data[i].role + '</h3>' +
            '<h3>Block: ' + data[i].blocked + '</h3><hr>' +
            '</div>' +
            '</div>' +
            '</div>'
        );
        var userButtonsDiv = $('#' + data[i].username);
        if (data[i].blocked == true) {
            userButtonsDiv.append(
                '<button class="btn btn-danger"  id="btnBlock" onclick="unblockUser(' + data[i].id + ')">Unblock</button>'
            );
        } else {
            userButtonsDiv.append(
                '<button class="btn btn-danger"  id="btnBlock" onclick="blockUser(' + data[i].id + ')">Block</button>'
            );
        }
        if (data[i].role != "Admin") {
            userButtonsDiv.append(
                '<button class="btn btn-primary"  id="btnPromote" onclick="promoteUser(' + data[i].id + ')">Promote</button>'
            );

        }
    }
}
