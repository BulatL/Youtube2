var loggedInUser;
var globalModelRole;
var globalModelId;
var globalModelBlocked;
var globalFollowUnfollow;

$(document).ready(function (e) {
    $.get('http://localhost:55157/Youtube/loggedInUser', function (data) {
        loggedInUser = data;
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
                '<li><a href="http://localhost:55157/Youtube/logout" ><span class="glyphicon glyphicon-log-out"></span> Log out</a></li>'
            );

            var userButtons = $('#userButtons');
            userButtons.empty();

            if (loggedInUser.role == 0 || loggedInUser.id == globalModelId) {
                userButtons.append(
                    '<a class="btn btn-success" id="btnEditProfile" href="http://localhost:55157/Youtube/users/edit/' + globalModelId + '"">Edit Profile</a>' +
                    '<a class="btn btn-danger" id="btnDeleteProfile" onclick="deleteUser(' + globalModelId + ')">Delete</a>'
                );
                var userInfoDiv = $('#UserInfoDiv');
                userInfoDiv.append(
                    '<div class="row-lg-2">' +
                    '<h3 class="userInfoH3">Type of user: ' + globalModelRole + '</h3>' +
                    '</div> ' +
                    '<div class="row-lg-2" > ' +
                    '<h3 class="userInfoH3"> Blocked: ' + globalModelBlocked + '</h3> ' +
                    '</div>'
                );
            }
            if (loggedInUser.role == 0) {
                if (globalModelRole == "Admin") {
                    userButtons.append(
                        '<button type="submit" class="btn btn-primary" id="btnPromote"  onclick="promoteUser(' + globalModelId + ')">Promote</button>'
                    );
                }

            }
            if (globalModelBlocked == "True") {
                userButtons.append(
                    '<a class="btn btn-danger" id="btnUnblock"  onclick="unblockUser(' + globalModelId + ')">Unblock</a>'
                );

            }
            if (globalModelBlocked == "False") {
                userButtons.append(
                    '<button class="btn btn-danger" id="btnBlock" onclick="blockUser(' + globalModelId + ')">Block</button>'
                );
            }
            if (loggedInUser.id == globalModelId && globalModelBlocked == "False") {
                var addVideoDiv = $('.addVideoDiv');
                addVideoDiv.append(
                    ' <a href="http://localhost:55157/Youtube/videos/add" id="btnAddVideo" class="btn btn-primary" >Add video</a>'
                );
            }
            if (loggedInUser.id != globalModelId) {
                var userButtons = $('#userButtons');
                if (globalFollowUnfollow == "follow") {
                    userButtons.append(
                        '<button onclick="unFollow(' + globalModelId + ');" class="btn btn-danger" id="btnUnfollow">Unfollow</button>'
                    );
                } else if (globalFollowUnfollow == "nofollow") {

                    userButtons.append(
                        '<button onclick="follow(' + globalModelId + ');" class="btn btn-primary" id="btnFollow">Follow</button>'
                    );
                }
            }
        }
    });
});

function promoteUser(id) {
    var r = confirm("Are u sure u want to promote this user");
    if (r == true) {
        $.ajax({
            url: 'promote/' + id,
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
    var r = confirm("Are u sure u want to block this user");
    if (r == true) {
        $.ajax({
            url: 'block/' + id,
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
    var r = confirm("Are u sure u want to unblock this user");
    if (r == true) {
        $.ajax({
            url: 'unblock/' + id,
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

function deleteUser(id) {
    var r = confirm("Are u sure u want to delete this user");
    if (r == true) {
        $.ajax({
            url: 'delete/' + id,
            type: 'Delete',
            dataType: 'json',
            contentType: "application/json",
            success: function (response) {
                alert(response)
                window.location.href = "http://localhost:55157/Youtube/Home";
            },
            error: function (response) {
                alert(response);
            }
        });
    }
}

function follow(id) {
    var r = confirm("Are u sure u want to follow this user");
    if (r == true) {
        $.ajax({
            url: 'follow/' + id,
            type: 'Post',
            dataType: 'json',
            success: function (response) {
                if (response == "Success") {
                    alert(response);
                    location.reload();
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    }
}

function unFollow(id) {
    var r = confirm("Are u sure u want to unfollow this user");
    if (r == true) {
        $.ajax({
            url: '/users/unfollow/' + id,
            type: 'Delete',
            dataType: 'json',
            success: function (response) {
                if (response == "Success") {
                    alert(response);
                    location.reload();
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    }
}

function existFollow(id) {
    $.ajax({
        url: 'existfollow/' + id,
        type: 'Get',
        dataType: 'json',
        success: function (response) {
            if (response == "Follow") {
                globalFollowUnfollow = "follow";

            } if (response == "No follow") {
                globalFollowUnfollow = "nofollow"
            }
        },
        error: function (response) {
        }
    });
}

function fillUserDiv(user) {
    var splitovanUser = user.split(",");
    var role = splitovanUser[0];
    var blocked = splitovanUser[1];
    var id = splitovanUser[2];
    globalModelBlocked = blocked;
    globalModelRole = role;
    globalModelId = id;
}

function searchAll() {
    var searchInput = $('#searchInput');
    var searchText = searchInput.val();

    var url = 'http://localhost:55157/Youtube/search/' + searchText;
    window.location.href = url;
};
