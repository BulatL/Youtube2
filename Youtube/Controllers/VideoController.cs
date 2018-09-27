using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Youtube.Models;
using Youtube.Services;
using Youtube.ViewModels;

namespace Youtube.Controllers
{
    public class VideoController : Controller
    {
        private IVideoData _videoData;
        private IUserData _userData;
        private ICommentData _commentData;
        private IHostingEnvironment _hostingEnvironment;
        private IFollowData _followData;
        private ILikeVideoData _likeDislikeData;

        public VideoController(IVideoData videoData, IUserData userData, 
            IHostingEnvironment hostingEnvironment, IFollowData followData, ICommentData commentData
            , ILikeVideoData likeDislike)
        {
            _videoData = videoData;
            _userData = userData;
            _commentData = commentData;
            _hostingEnvironment = hostingEnvironment;
            _followData = followData;
            _likeDislikeData = likeDislike;
        }
        [HttpGet]
        [Route("videos")]
        public IActionResult GetAllVideos()
        {
            IEnumerable<Video> videos = new List<Video>();
            List<VideoDTO> videosDTO = new List<VideoDTO>();
            videos = _videoData.GetAllForAdmin();
            if(videos.Count() <= 0)
            {
                return NoContent();
            }
            foreach (var v in videos)
            {
                v.Owner = _userData.GetById(v.OwnerId);
                v.Owner.UserVideos = null;
                VideoDTO videoDTO = VideoDTO.ConvertVideoToDTO(v);
                videosDTO.Add(videoDTO);
            }
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(videosDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    return View("Videos", videosDTO);
                }
                return StatusCode(415);
            }
            return Json(videosDTO);
        }

        [HttpGet]
        [Route("videos/{id}")]
        public IActionResult GetVideoById(long id)
        {
            var contentType = Request.ContentType;
            Video video = new Video();
            if(contentType != null)
            {
                video = _videoData.GetById(id);
                if (video.Name == null)
                {
                    return NotFound();
                }
            }
            else
            {
                var loggedInUserRole = HttpContext.Session.GetString("LoggedInUserRole");
                if (loggedInUserRole == null)
                {
                    video = _videoData.GetById(id);
                    if (video == null)
                    {
                        return NotFound();
                    }
                    if(video.Blocked == true || video.Visibility == Visibility.Private)
                    {
                        return StatusCode(401);
                    }
                }
                else
                {
                    if (loggedInUserRole.Equals("0"))
                    {
                        video = _videoData.GetById(id);
                        if (video == null)
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        long loggedInUserId = long.Parse(HttpContext.Session.GetString("LoggedInUserId"));
                        video = _videoData.GetById(id);
                        if (video == null)
                        {
                            return NotFound();
                        }
                        if(video.OwnerId != loggedInUserId && (video.Blocked == true || video.Visibility == Visibility.Private))
                        {
                            return StatusCode(401);
                        }
                    }
                }
            }
            video.Owner = _userData.GetById(video.OwnerId);
            video.Comments = _commentData.GetCommentsByVideo(video.Id);
            foreach (var comment in video.Comments)
            {
                comment.User = _userData.GetById(comment.UserId);
            }
            video.NumberOfViews++;
            video = _videoData.Update(video);
            SingleVideoDTO singleVideoDTO = SingleVideoDTO.ConvertVideoToDTO(video);

            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(singleVideoDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    VideoDTO newDTO = VideoDTO.ConvertVideoToDTO(video);
                    return View("VideoInfo", newDTO);
                }
                return StatusCode(415);
            }
            return View("VideoPage", singleVideoDTO);
        }

        [HttpGet]
        [Route("videos/add")]
        public IActionResult AddVideo()
        {
            return View("AddVideoPage");
        }

        [HttpPost]
        [Route("videos/add")]
        public IActionResult AddVideo(AddVideoDTO videoDTO, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

                if (loggedInUserId == null)
                {
                    return StatusCode(401);
                }
                User user = _userData.GetById(long.Parse(loggedInUserId));
                if (user == null)
                {
                    return StatusCode(401);
                }
                if(user.Blocked == true)
                {
                    return StatusCode(401);
                }

                Video newVideo = new Video();
                newVideo.AllowComments = videoDTO.AllowComments;
                newVideo.AllowRaiting = videoDTO.AllowRaiting;
                newVideo.Blocked = false;
                newVideo.Deleted = false;
                newVideo.Description = videoDTO.Description;
                newVideo.CreationDate = DateTime.Today;
                newVideo.Name = videoDTO.Name;
                newVideo.NumberOfDislikes = 0;
                newVideo.NumberOfLikes = 0;
                newVideo.NumberOfViews = 0;
                newVideo.OwnerId = user.Id;
                newVideo.VideoUrl = videoDTO.VideoUrl;
                newVideo.Visibility = videoDTO.Visibility;

                if (image != null)
                {
                    newVideo.PhotoUrl = AddImage(newVideo.Name, user.Username, image);
                }
                else
                {
                    newVideo.PhotoUrl = "defaultVideoImage.jpg";
                }


                newVideo = _videoData.Create(newVideo);
                VideoDTO video = VideoDTO.ConvertVideoToDTO(newVideo);
                var contentType = Request.ContentType;

                user.Followers = _followData.GetFollowers(user.Id);
                user.Following = _followData.GetFollowings(user.Id);
                user.UserVideos = _videoData.GetByOwnerId(user.Id);
                user.LikedVideos = _videoData.GetLikedVideos(user);
                foreach (var u in user.LikedVideos)
                {
                    u.Owner = _userData.GetById(u.OwnerId);
                }

                SingleUserDTO singleUserDTO = SingleUserDTO.ConvertUserToDTO(user);

                if (contentType != null)
                {
                    if (contentType.Equals("application/json"))
                    {
                        return Json(video);
                    }
                    else if (contentType.Equals("text/html"))
                    {
                        return View("UserPage", singleUserDTO);
                    }
                    return View("UserPage", singleUserDTO);
                }
                return Json(video);
            }
            else
            {
                return Json(new VideoDTO());
            }
        }

        [HttpGet]
        [Route("videos/edit/{id}")]
        public IActionResult EditVideo(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            Video video = _videoData.GetById(id);
            User owner = _userData.GetById(long.Parse(loggedInUserId));
            if (owner == null || video == null)
            {
                return StatusCode(401);
            }
            if (owner.Blocked == true || (owner.Role == RoleType.User && owner.Id != video.OwnerId))
            {
                return StatusCode(401);
            }
            AddVideoDTO videoDTO = AddVideoDTO.ConvertDTO(video);
            return View("EditVideoPage", videoDTO);

        }

        [HttpPost]
        [Route("videos/edit/{id}")]
        public IActionResult EditVideo(VideoDTO videoDTO, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                Video editVideo = _videoData.GetById(videoDTO.Id);
                if(editVideo == null)
                {
                    return NotFound();
                }
                editVideo.AllowComments = videoDTO.AllowComments;
                editVideo.AllowRaiting = videoDTO.AllowRaiting;
                editVideo.Description = videoDTO.Description;
                editVideo.Name = videoDTO.Name;
                editVideo.Visibility = videoDTO.Visibility;

                if(image != null)
                {
                    var username = HttpContext.Session.GetString("LoggedInUserUsername");
                    editVideo.PhotoUrl = AddImage(editVideo.Name, username, image);
                }
                editVideo.Owner = _userData.GetById(editVideo.OwnerId);
                VideoDTO video = VideoDTO.ConvertVideoToDTO(editVideo);
                editVideo = _videoData.Update(editVideo);
                var contentType = Request.ContentType;
                if (contentType != null)
                {
                    if (contentType.Equals("application/json"))
                    {
                        return Json(video);
                    }
                    else if (contentType.Equals("text/html"))
                    {
                        //return RedirectToAction(nameof(GetAllUsers));
                    }
                    return RedirectToAction(nameof(GetVideoById), video.Id);
                }
                return Json(video);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("videos/delete/{id}")]
        public IActionResult DeleteVideo(int id)
        {
            Video video = _videoData.GetById(id);
            if(video == null)
            {
                return NotFound();
            }
            List<Comment> comments = _commentData.GetCommentsByVideo(id).ToList();
            foreach (var c in comments)
            {
                _commentData.Delete(c);
            }
            _videoData.Delete(video);
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                return NoContent();
            }
            return Json("Success");
        }

        [HttpPatch]
        [Route("videos/block/{id}")]
        public IActionResult BlockVideo(int id)
        {
            var contentType = Request.ContentType;
            Video video = _videoData.GetById(id);
            if (video == null)
            {
                return NotFound();
            }
            if (video.Blocked == true)
            {
                return BadRequest();
            }
            video.Blocked = true;
            video = _videoData.Update(video);
            video.Owner = _userData.GetById(video.OwnerId);
            VideoDTO videoDTO = VideoDTO.ConvertVideoToDTO(video);
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(videoDTO);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }

        [HttpPatch]
        [Route("videos/unblock/{id}")]
        public IActionResult UnblockVideo(int id)
        {
            var contentType = Request.ContentType;
            Video video = _videoData.GetById(id);
            if (video == null)
            {
                return NotFound();
            }
            if (video.Blocked == false)
            {
                return BadRequest();
            }
            video.Blocked = false;
            video = _videoData.Update(video);
            video.Owner = _userData.GetById(video.OwnerId);
            VideoDTO videoDTO = VideoDTO.ConvertVideoToDTO(video);
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(videoDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return RedirectToAction(nameof(GetVideoById), video.Id);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }

        [HttpGet]
        [Route("videos/sort/{sortBy}/{ascDesc}")]
        public IActionResult SortVideo(string sortBy, string ascDesc)
        {
            if (sortBy == null)
            {
                return BadRequest();
            }
            if (ascDesc == null)
            {
                return BadRequest();
            }
            
            IEnumerable<Video> videos = _videoData.GetAllForAdmin();
            if(videos.Count() <= 0)
            {
                return NoContent();
            }
            IEnumerable<Video> sortedVideos = new List<Video>();
            ascDesc.ToLower();
            sortBy.ToLower();
            if (ascDesc.Equals("asc"))
            {
                switch (sortBy)
                {
                    case "name":
                        sortedVideos = videos.OrderBy(v => v.Name).ToList();
                        break;
                    case "description":
                        sortedVideos = videos.OrderBy(v => v.Description).ToList();
                        break;
                    case "date":
                        sortedVideos = videos.OrderBy(v => v.CreationDate).ToList();
                        break;
                    case "user":
                        sortedVideos = videos.OrderBy(v => v.Owner.Id).ToList();
                        break;
                    case "view":
                        sortedVideos = videos.OrderBy(v => v.NumberOfViews).ToList();
                        break;
                    case "like":
                        sortedVideos = videos.OrderBy(v => v.NumberOfLikes - v.NumberOfDislikes).ToList();
                        break;
                    default:
                        sortedVideos = videos.OrderBy(v => v.Name).ToList();
                        break;
                }
            }
            else
            {
                switch (sortBy)
                {
                    case "name":
                        sortedVideos = videos.OrderByDescending(v => v.Name).ToList();
                        break;
                    case "description":
                        sortedVideos = videos.OrderByDescending(v => v.Description).ToList();
                        break;
                    case "date":
                        sortedVideos = videos.OrderByDescending(v => v.CreationDate).ToList();
                        break;
                    case "user":
                        sortedVideos = videos.OrderByDescending(v => v.Owner.Id).ToList();
                        break;
                    case "view":
                        sortedVideos = videos.OrderByDescending(v => v.NumberOfViews).ToList();
                        break;
                    case "like":
                        sortedVideos = videos.OrderByDescending(v => v.NumberOfLikes - v.NumberOfDislikes).ToList();
                        break;
                    default:
                        sortedVideos = videos.OrderByDescending(v => v.Name).ToList();
                        break;
                }
            }

            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(sortedVideos);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return Json(model.Users);
                }
                return StatusCode(415);
            }
            return Json(sortedVideos);
        }

        [HttpGet]
        [Route("videos/search/{searchText}/{searchBy?}")]
        public IActionResult Search(string searchText, string searchBy)
        {
            if (searchText == null)
            {
                return BadRequest();
            }
            IEnumerable<Video> videos = new List<Video>();
            videos = _videoData.Search(searchText, searchBy);
            if (videos.Count() == 0)
            {
                return NoContent();
            }
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(videos);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return Json(model.Users);
                }
                return StatusCode(415);
            }
            return Json(videos);
        }

        [HttpPost]
        [Route("videos/like/{id}")]
        public IActionResult LikeVideo(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));
            if(user == null)
            {
                return StatusCode(401);
            }
            Video video = _videoData.GetById(id);
            if (_likeDislikeData.Check(true,user.Id, video.Id))
            {
                return Json("Already liked");
            }
            LikeDislikeVideo ld = new LikeDislikeVideo();
            ld.LikeOrDislike = true;
            ld.Owner = user;
            ld.Video = video;
            ld.CreationDate = DateTime.Today;
            _likeDislikeData.Add(ld);
            video.NumberOfLikes++;
            _videoData.Update(video);
            return Json("Success");
        }

        [HttpPost]
        [Route("videos/dislike/{id}")]
        public IActionResult DislikeVideo(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));
            if (user == null)
            {
                return StatusCode(401);
            }
            Video video = _videoData.GetById(id);
            if (_likeDislikeData.Check(false, user.Id, video.Id))
            {
                return Json("Already liked");
            }
            LikeDislikeVideo ld = new LikeDislikeVideo();
            ld.LikeOrDislike = false;
            ld.Owner = user;
            ld.Video = video;
            ld.CreationDate = DateTime.Today;
            _likeDislikeData.Add(ld);
            video.NumberOfDislikes++;
            _videoData.Update(video);
            return Json("Success");
        }

        [HttpPost]
        [Route("videos/postcomment")]
        public IActionResult PostComment(long id,string description)
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
                return StatusCode(401);
            }
            comment.User = user;
            comment.Video = video;
            comment.Description = description;
            comment.CreationDate = DateTime.Today;
            CommentDTO commentDTO = CommentDTO.ConvertCommentToDTO(comment);
            comment = _commentData.Create(comment);
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
                }else if (contentType.Equals("application/x-www-form-urlencoded; charset=UTF-8"))
                {
                    return Json("Success");
                }
                return StatusCode(415);
            }
            return Json(commentDTO);
        }

        public string AddImage(string videoName,string username, IFormFile image)
        {
            var fileName = DateTime.Now.ToString("yyyyMMdd")+ videoName + username + Path.GetExtension(image.FileName);
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, fileName);
            image.CopyTo(new FileStream(filePath, FileMode.Create));
            return fileName;
        }

    }
}
