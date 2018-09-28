using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Youtube.Models;
using Youtube.Services;
using Youtube.ViewModels;

namespace Youtube.Controllers
{
    public class HomeController : Controller
    {
        private IUserData _userData;
        private IFollowData _followData;
        private IVideoData _videoData;
        private ICommentData _commentData;

        public HomeController(IUserData userData,
            IFollowData followData, IVideoData videoData, ICommentData commentData)
        {
            _userData = userData;
            _followData = followData;
            _videoData = videoData;
            _commentData = commentData;
        }

        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            var loggedInUserUsername = HttpContext.Session.GetString("LoggedInUserUsername");
            var loggedInUserRole = HttpContext.Session.GetString("LoggedInUserRole");
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");
            

            IEnumerable<Video> top5Videos = new List<Video>();
            IEnumerable<Video> allVideos = new List<Video>();
            List<VideoDTO> top5VideosDTO = new List<VideoDTO>();
            List<VideoDTO> allVideosDTO = new List<VideoDTO>();

            if (loggedInUserRole == null)
            {
                top5Videos = _videoData.GetTop5("NoUser", 0);
                allVideos = _videoData.GetAllNoUser();
            }
            else
            {
                if (loggedInUserRole.Equals("Admin"))
                {
                    top5Videos = _videoData.GetTop5("Admin", long.Parse(loggedInUserId));
                    allVideos = _videoData.GetAllForAdmin();
                }
                else
                {
                    top5Videos = _videoData.GetTop5("User", long.Parse(loggedInUserId));
                    allVideos = _videoData.GetAllForUser(long.Parse(loggedInUserId));
                }
            }
            foreach (var v in top5Videos)
            {
                v.Owner = _userData.GetById(v.OwnerId);
                v.Owner.UserVideos = null;
                VideoDTO videoDTO = VideoDTO.ConvertVideoToDTO(v);
                top5VideosDTO.Add(videoDTO);
            }

            foreach (var v in allVideos)
            {
                v.Owner = _userData.GetById(v.OwnerId);
                v.Owner.UserVideos = null;
                VideoDTO videoDTO = VideoDTO.ConvertVideoToDTO(v);
                allVideosDTO.Add(videoDTO);
            }

            HomeVideoViewModel homeVideoViewModel = new HomeVideoViewModel();
            homeVideoViewModel.AllVideos = allVideosDTO;
            homeVideoViewModel.Top5Videos = top5VideosDTO;
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(homeVideoViewModel);
                }
                else if (contentType.Equals("text/html"))
                {
                    return View("~/Views/Shared/HomePage.cshtml", homeVideoViewModel);
                }
                return StatusCode(415);
            }
            return View("~/Views/Shared/HomePage.cshtml", homeVideoViewModel);

        }
    }
}
