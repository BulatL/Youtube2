var globalData;
var oldDescription;
var globalOwnerId;
var globalVideoId;
var globalVideoBlocked;

$(document).ready(function (e) {
    $.get('http://localhost:55157/Youtube/loggedInUser', function (data) {
        globalData = data;
        if (data == null) {
            var navbarLogin = $('#navbarLogin');
            navbarLogin.append(
                '<li><a href="http://localhost:55157/Youtube/register"><span class="glyphicon glyphicon-user"></span> Register</a></li>' +
                '<li><a href="http://localhost:55157/Youtube/login"><span class="glyphicon glyphicon-log-out"></span> Login</a></li>'
            );
            $('.deleteCommentButton').hide();
            $('.editCommentButton').hide();
        }
        else {
            if (data.role == 0) {
                var navbarHeader = $('.navbar-header');
                navbarHeader.append(
                    '<a href="http://localhost:55157/Youtube/admin" class="navbar-brand">Admin page</a>'
                );
            }
            else {
                if (data.id != globalOwnerId) {
                    $('.deleteCommentButton').hide();
                    $('.editCommentButton').hide();
                }
            }
            var navbarLogin = $('#navbarLogin');
            navbarLogin.append(
                '<li><a href="http://localhost:55157/Youtube/users/' + data.id + '" ><span class="glyphicon glyphicon-user"></span> My channel</a></li>' +
                '<li><a href="http://localhost:55157/Youtube/logout"><span class="glyphicon glyphicon-log-out"></span> Log out</a></li>'
            );
            var url = location.href;
            var splitovanURL = url.split("/")[5];
            var videoId = splitovanURL;
            $.get('http://localhost:55157/Youtube/videos/one/' + videoId, function (data) {
                globalVideoId = data.id;
                globalOwnerId = data.ownerId;
                globalVideoBlocked = data.blocked;
                videoDivButtons()
            });
        }
    });
    commentsDivButtons();

});

function videoDivButtons() {
    if (globalData.role == 0 || globalData.id == globalOwnerId) {
        var editVideoButtonsDiv = $('#editVideoButtonsDiv');
        editVideoButtonsDiv.append(
            '<button class="btn btn-success" id="editInfo" onclick="editInfoVideo(' + globalVideoId + ')">Edit information</button>'
        );
        var videoButtonsDiv = $('#videoButtonsDiv');
        videoButtonsDiv.append(
            '<button id="btnCancel2" class="btn btn-danger" style="margin-right:1em;" onclick="alertDeleteVideo(' + globalVideoId + ')">Delete</button>'
        );
        if (globalVideoBlocked === true) {
            videoButtonsDiv.append(
                '<button id="btnCancel2" class="btn btn-danger" style="margin-right:1em;" onclick="unBlockVideo(' + globalVideoId + ')">Unblock</button>'
            );
        }
        else {
            videoButtonsDiv.append(
                '<button id="btnCancel2" class="btn btn-danger" style="margin-right:1em;" onclick="blockVideo(' + globalVideoId + ')">Block</button>'
            );
        }
    }
}

function getIds(ownerId, videoId, blocked) {

    globalOwnerId = ownerId;
    globalVideoId = videoId;
    globalVideoBlocked = blocked;
}

function commentsDivButtons(comment) {
    if (globalData != null && globalData.blocked == false) {
        if (globalData.id == comment.user.id || globalData.role == 0) {
            var komenatButtonsDiv = $('#komentarButtonsDiv' + comment.id);
            komenatButtonsDiv.empty();
            komenatButtonsDiv.append(
                '<button name="post"  id="editComment" onclick="editComment(' + comment.id + ')">Edit</button>' +
                '<button id="deleteVideo" onclick="alertDeleteComment(' + comment.id + ')">Delete</button>'
            );
        }
    }
}

function disableCommentsButtons(ownerId, commnetId) {

    if (globalData == null) {
        var commentDiv = $("#" + commnetId);
        commentDiv.empty();
        return;

    } else {
        if (globalData.blocked == true) {
            var commentDiv = $("#" + commnetId);
            commentDiv.empty();
            return;
        }
        if (globalData.id != ownerId || globalData.role == 1) {
            var commentDiv = $("#" + commnetId);
            commentDiv.empty();
            return;
        }

    }

}

function searchAll() {
    var searchInput = $('#searchInput');
    var searchText = searchInput.val().trim();

    var url = 'http://localhost:55157/Youtube/search/' + searchText;
    window.location.href = url;
};

function editInfoVideo(id) {

    window.location = 'http://localhost:55157/Youtube/videos/edit/' + id;
}

function alertDeleteVideo(id) {

    var r = confirm("Are u sure u want to delete this video?");
    if (r == true) {
        $.ajax({
            url: 'http://localhost:55157/Youtube/videos/delete/' + id,
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

function blockVideo(id) {

    var r = confirm("Are u sure u want to block this video?");
    if (r == true) {
        $.ajax({
            url: 'http://localhost:55157/Youtube/videos/block/' + id,
            type: 'Patch',
            dataType: 'json',
            success: function (response) {
                alert(response);
                if (response == "Success") {
                    window.location.href = window.location.href;
                }
            },
            error: function (response) {
                alert("Something went wrong");
            }
        });
    }
}

function unBlockVideo(id) {

    var r = confirm("Are u sure u want to block this video?");
    if (r == true) {
        $.ajax({
            url: 'http://localhost:55157/Youtube/videos/unblock/' + id,
            type: 'Patch',
            dataType: 'json',
            success: function (response) {
                alert(response);
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

function likeVideo(id) {

    $.ajax({
        url: 'http://localhost:55157/Youtube/videos/like/' + id,
        type: 'Post',
        dataType: 'json',
        success: function (response) {
            alert(response);
            if (response == "Success") {
                location.reload();
            }
        },
        error: function (response) {
            alert("Something wnet wrong");
        }
    });
}

function dislikeVideo(id) {

    $.ajax({
        url: 'http://localhost:55157/Youtube/videos/dislike/' + id,
        type: 'Post',
        dataType: 'json',
        success: function (response) {
            alert(response);
            if (response == "Success") {
                location.reload();
            }
        },
        error: function (response) {
            alert("Something wnet wrong");
        }
    });
}

function likeComment(id) {

    $.ajax({
        url: 'http://localhost:55157/Youtube/comments/like/' + id,
        type: 'Post',
        dataType: 'json',
        success: function (response) {
            alert(response);
            if (response == "Success") {
                location.reload();
            }

        },
        error: function (response) {
            alert("Something wnet wrong");
        }
    });
}

function dislikeComment(id) {

    $.ajax({
        url: 'http://localhost:55157/Youtube/comments/dislike/' + id,
        type: 'Post',
        dataType: 'json',
        success: function (response) {
            alert(response);
            if (response == "Success") {
                location.reload();
            }

        },
        error: function (response) {
            alert("Something wnet wrong");
        }
    });
}

function postComment(id) {
    if (globalData != null) {
        if (globalData.blocked == false) {
            var description = $('#komentarInput').val().trim();
            if (description == "") {
                alert("Comment may not be empty");
                return;
            }
            $.ajax({
                url: "http://localhost:55157/Youtube/videos/postcomment",
                type: 'Post',
                dataType: 'json',
                data: {
                    'description': description,
                    'id': id
                },
                success: function (response) {
                    alert(response)
                    location.reload();
                },
                error: function (response) {
                    alert(response);
                }
            });
        }
        else {
            alert("Your channel is blocked");
        }
    }
    else {
        var r = confirm("You need to login first \n  Do you want to login?");
        if (r == true) {
            window.location.href = "http://localhost:55157/Youtube/login";
        }
    }
}

function alertDeleteComment(id) {

    var r = confirm("Are u sure u want to delete this comment?");
    if (r == true) {
        $.ajax({
            url: 'http://localhost:55157/Youtube/comments/delete/' + id,
            type: 'Delete',
            dataType: 'json',
            contentType: "application/json",
            success: function (response) {
                alert(response)
                location.reload();
            },
            error: function (response) {
                alert(response);
            }
        });
    }
}

function editComment(commentId, commentDescription) {
    oldDescription = commentDescription;
    var commentDesciptionInput = document.getElementById(commentId + commentDescription);
    commentDesciptionInput.readOnly = false;
    commentDesciptionInput.focus();
    var komentarButtonsDiv = $('#' + commentId);
    komentarButtonsDiv.empty();
    komentarButtonsDiv.append(
        '<button class="btn btn-primary" id="editComment" onclick="saveEditComment(' + commentId + ')">Save</button>' +
        '<button id="deleteVideo" class="btn btn-danger" onclick="cancelEditComment(' + commentId + ')">Cancel</button>'
    );

}

function saveEditComment(id) {
    if (globalData != null) {
        var description = document.getElementById(id + oldDescription).value;
        $.ajax({
            url: 'http://localhost:55157/Youtube/comments/edit/' + id,
            type: 'Patch',
            dataType: 'json',
            data: {
                'id': id,
                'description': description
            },
            success: function (response) {
                if (response == "Success") {
                    alert("Success");
                    document.getElementById(id + oldDescription).readOnly = true;
                    var komenatButtonsDiv = $('#komentarButtonsDiv' + id);
                    komenatButtonsDiv.empty();
                    komenatButtonsDiv.append(
                        '<button class="btn btn-success" name="post"  id="editComment" onclick="editComment(' + id + ')">Edit</button>' +
                        '<button class="btn btn-danger" id="deleteVideo" onclick="alertDeleteComment(' + id + ')">Delete</button>'
                    );
                    location.reload();
                }
            },
            error: function (response) {
                alert("Something went wrong");
            }
        });
    } else {
        alert("You need to login first");
        cancelEditComment(id);
    }
}

function cancelEditComment(id) {
    var commentDesciptionInput = document.getElementById(id + oldDescription);
    commentDesciptionInput.readOnly = true;
    var commentButtonsDiv = $('#' + id);
    var c = id + "," + "'" + oldDescription + "'";
    commentButtonsDiv.empty();
    commentButtonsDiv.append(
        '<button class="btn btn-success" name="post" id="editComment" onclick="editComment(' + id + ')">Edit</button>' +
        '<button class="btn btn-danger" id="deleteVideo" onclick="alertDeleteComment(' + id + ')">Delete</button>'
    );
    commentDesciptionInput.value = oldDescription;
}

function cancelPostComment() {
    var komentarInput = $('#komentarInput');
    komentarInput.val() = "";

}