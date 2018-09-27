using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Youtube.Models;
using Youtube.Services;
using Youtube.ViewModels;

namespace Youtube.Controllers
{
    public class CommentController : Controller
    {
        private IUserData _userData;
        private IFollowData _followData;
        private IVideoData _videoData;
        private ICommentData _commentData;
        private ILikeCommentData _likeCommentData;

        public CommentController(IUserData userData, IFollowData followData, 
            IVideoData videoData, ICommentData commentData, ILikeCommentData likeCommentData)
        {
            _userData = userData;
            _followData = followData;
            _videoData = videoData;
            _commentData = commentData;
            _likeCommentData = likeCommentData;
        }

        [HttpPost]
        [Route("comments/postcomment")]
        public IActionResult PostComment(long id, string description)
        {
            Comment comment = new Comment();
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));
            Video video = _videoData.GetById(id);
            if (user == null || video == null)
            {
                return BadRequest();
            }
            if(user.Blocked == true)
            {
                return StatusCode(401);
            }
            comment.User = user;
            comment.Video = video;
            comment.Description = description;
            comment.CreationDate = DateTime.Today;
            comment = _commentData.Create(comment);
            CommentDTO commentDTO = CommentDTO.ConvertCommentToDTO(comment);
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(commentDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return View("VideoPage", singleVideoDTO);
                }
                else if (contentType.Equals("application/x-www-form-urlencoded; charset=UTF-8"))
                {
                    return Json("Success");
                }
                return StatusCode(415);
            }
            return Json(commentDTO);
        }

        [HttpPatch]
        [Route("comments/edit/{id}")]
        public IActionResult EditComment(long id, string description)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));
            Comment comment = _commentData.GetById(id);
            if (user == null || comment == null)
            {
                return BadRequest();
            }
            comment.Description = description;
            comment.Video = _videoData.GetById(comment.VideoId);
            comment = _commentData.Update(comment);
            CommentDTO commentDTO = CommentDTO.ConvertCommentToDTO(comment);
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(commentDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return View("VideoPage", singleVideoDTO);
                }
                else if (contentType.Equals("application/x-www-form-urlencoded; charset=UTF-8"))
                {
                    return Json("Success");
                }
                return StatusCode(415);
            }
            return Json(commentDTO);
        }

        [HttpDelete]
        [Route("comments/delete/{id}")]
        public IActionResult DeleteComment(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));
            Comment comment = _commentData.GetById(id);
            if (user == null || comment == null)
            {
                return BadRequest();
            }
            if(user.Blocked == true)
            {
                return StatusCode(401);
            }
            _commentData.Delete(comment);
            return Json("Success");
        }

        [HttpPost]
        [Route("comments/like/{id}")]
        public IActionResult LikeComment(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            var contentType = Request.ContentType;
            if (loggedInUserId == null)
            {
                if (contentType == null) return Json("You need to login first");
                else return StatusCode(401);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));
            Comment comment = _commentData.GetById(id);
            if (user == null || comment == null)
            {
                if (contentType == null) return Json("Bad request");
                else return BadRequest();
            }
            if (user.Blocked == true)
            {
                if (contentType == null) return Json("Your account is blocked");
                else return StatusCode(401);
            }
            if (_likeCommentData.Check(true, user.Id, comment.Id))
            {
                return Json("Already liked");
            }
            LikeDislikeComment likeDislikeComment = new LikeDislikeComment();
            likeDislikeComment.LikeOrDislike = true;
            likeDislikeComment.Owner = user;
            likeDislikeComment.Comment = comment;
            likeDislikeComment.CreationDate = DateTime.Today;
            likeDislikeComment = _likeCommentData.Add(likeDislikeComment);
            comment.NumberOfLikes++;
            _commentData.Update(comment);

            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    LikeCommentDTO likeCommentDTO = LikeCommentDTO.ConvertCommentToDTO(likeDislikeComment);
                    return Json(likeCommentDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return View("VideoPage", singleVideoDTO);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }

        [HttpPost]
        [Route("comments/dislike/{id}")]
        public IActionResult DislikeComment(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");
            var contentType = Request.ContentType;
            if (loggedInUserId == null)
            {
                if (contentType == null) return Json("You need to login first");
                else return StatusCode(401);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));
            Comment comment = _commentData.GetById(id);
            if (user == null || comment == null)
            {
                if (contentType == null) return Json("Bad request");
                else return BadRequest();
            }
            if (user.Blocked == true)
            {
                if (contentType == null) return Json("Your account is blocked");
                else return StatusCode(401);
            }
            if (_likeCommentData.Check(false, user.Id, comment.Id))
            {
                return Json("Already disliked");
            }
            LikeDislikeComment likeDislikeComment = new LikeDislikeComment();
            likeDislikeComment.LikeOrDislike = false;
            likeDislikeComment.Owner = user;
            likeDislikeComment.Comment = comment;
            likeDislikeComment.CreationDate = DateTime.Today;
            likeDislikeComment = _likeCommentData.Add(likeDislikeComment);
            comment.NumberOfDislikes ++;
            _commentData.Update(comment);

            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    LikeCommentDTO likeCommentDTO = LikeCommentDTO.ConvertCommentToDTO(likeDislikeComment);
                    return Json(likeCommentDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return View("VideoPage", singleVideoDTO);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }
    }
}
