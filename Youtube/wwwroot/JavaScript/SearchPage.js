var globalResponse;
$(document).ready(function (e) {
    $.get('http://localhost:55157/Youtube/loggedInUser', function (data) {
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
                '<li><a href="http://localhost:55157/Youtube/users/' + data.id + '" ><span class="glyphicon glyphicon-user"></span> My channel</a></li>' +
                '<li><a href="http://localhost:55157/Youtube/logout"><span class="glyphicon glyphicon-log-out"></span> Log out</a></li>'
            );
        }
    });
});

function searchFn() {
    var searchInput = $('#searchInput');
    var searchText = searchInput.val().trim();
    alert(searchText + " nesto");
    var url = 'http://localhost:55157/Youtube/search/' + searchText;
    window.location.href = url;
};

function searchAll() {
    var searchInput = $('#searchInput');
    var searchText = searchInput.val().trim();

    var searchVideo = false;
    var searchUser = false;
    var searchComment = false;

    var sortVideoBy = "";
    var sortUserBy = "";
    var sortCommentBy = "";
    var ascDesc = "";

    var checkboxVideo = document.getElementById('searchVideo');
    if (checkboxVideo.checked) {
        searchVideo = true;
    } else {
        searchVideo = false;
    }

    var checkboxUser = document.getElementById('searchUser');
    if (checkboxUser.checked) {
        searchUser = true;
    } else {
        searchUser = false;
    }

    var checkboxComment = document.getElementById('searchComment');
    if (checkboxComment.checked) {
        searchComment = true;
    } else {
        searchComment = false;
    }

    sortVideoBy = $('input[name=sortVideoBy]:checked').val();
    sortUserBy = $('input[name=sortUserBy]:checked').val();
    sortCommentBy = $('input[name=sortCommentBy]:checked').val();
    ascDesc = $('input[name=ascDesc]:checked').val();

    $.ajax({
        url: searchText + "/" + searchVideo + "/" + searchUser + "/" + searchComment + "/" +
            sortVideoBy + "/" + sortUserBy + "/" + sortCommentBy + "/" + ascDesc,
        dataType: 'json',
        contentType: "application/json",
        type: 'get',
        success: function (response) {
            fillDiv(response);
        },
        error: function (response) {
        }
    });
}

function fillDiv(data) {
    var videosDiv = $('#videosDiv');
    var usersDiv = $('#usersDiv');
    var commentsDiv = $('#commentsDiv');

    videosDiv.empty();
    usersDiv.empty();
    commentsDiv.empty();

    var videosLength = data.videos.length;
    var usersLength = data.users.length;
    var CommnetsLength = data.comments.length;

    if (videosLength > 0) {
        for (var v in data.videos) {
            videosDiv.append(
                '<div class="col-lg-3">' +
                '<div class="Video">' +
                '<a href="http://localhost:55157/Youtube/videos/' + data.videos[v].id + '" id="videoName">' +
                '<img src="http://localhost:55157/Youtube/uploads/' + data.videos[v].photoUrl + '" class="imgVideo">' +
                '<div class="caption">' +
                '<h3>' + data.videos[v].name + '</h3>' +
                '</div>' +
                '</a>' +
                '<a href="http://localhost:55157/Youtube/users/' + data.videos[v].user.id + '" id="ownerName"><h3>' + data.videos[v].user.username + '</h3></a>' +
                '<h4>' + data.videos[v].description + '<br>' + data.videos[v].numberOfViews + ' views<br>' + data.videos[v].creationDate + '</h4>' +
                '</div>' +
                '</div>'
            );
        }
    }
    if (usersLength > 0) {

        for (var u in data.users) {
            usersDiv.append(
                '<div class="col-lg-3">' +
                '<div class="FollowersContainer">' +
                '<a href="http://localhost:55157/Youtube/users/' + data.users[u].id + '">' +
                '<img src="http://localhost:55157/Youtube/uploads/' + data.users[u].profilePictureUrl + '" class="imgVideo">' +
                '<div class="cufCaption">' +
                '<h3>Username: ' + data.users[u].username + '</h3>' +
                '</div>' +
                '</a>' +
                '<h3>First name: ' + data.users[u].firstName + '</h3>' +
                '<h3>Last name: ' + data.users[u].lastName + '</h3>' +
                '<h3>Email: ' + data.users[u].email + '</h3>' +
                '</div>' +
                '</div>'
            );
        }
    }
    if (CommnetsLength > 0) {

        for (var c in data.comments) {
            commentsDiv.append(
                '<div class="col-lg-3">' +
                '<div class="commentContainer">' +
                '<div class="caption" id="commentsInfoDiv">' +
                '<a href="http://localhost:55157/Youtube/users/' + data.comments[c].user.id + '">' +
                '<h3>' + data.comments[c].user.username + '</h3>' +
                '</a><br>' +
                '<h4>' + data.comments[c].description + '</h4>' +
                '<h4>' + data.comments[c].numberOfLikes + ' - ' + data.comments[c].numberOfDislikes + ' raiting</h4>' +
                '<h4>The name of the video on which the comment is: ' +
                '<a href="http://localhost:55157/Youtube/videos/' + data.comments[c].video.id + '">' +
                data.comments[c].video.name +
                '</a>' +
                '</h4>' +
                '<hr>' +
                '</div>' +
                '</div>' +
                '</div>'
            );
        }
    }
}