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
    public class SearchController : Controller
    {
        private IUserData _userData;
        private IVideoData _videoData;
        private ICommentData _commentData;
        public SearchController(IUserData userData, IVideoData videoData, ICommentData commentData)
        {
            _userData = userData;
            _videoData = videoData;
            _commentData = commentData;
        }

        [HttpGet]
        [Route("search/{searchText?}")]
        public IActionResult SearchAll(string searchText)
        {
            var loggedInUserRole = HttpContext.Session.GetString("LoggedInUserRole");

            if (searchText == null) searchText = "";
            IEnumerable<Video> videos = new List<Video>();
            IEnumerable<Comment> comments = new List<Comment>();
            IEnumerable<User> users = new List<User>();

            users = _userData.Search(searchText, null, "desc");
            comments = _commentData.Search(searchText, "desc");
            if (loggedInUserRole == null)
            {
                videos = _videoData.GetAllNoUser().Where(v => v.Name.Contains(searchText)||
                                                              v.Description.Contains(searchText));
            }
            else
            {
                if (loggedInUserRole.Equals("0")){
                    videos = _videoData.GetAllForAdmin().Where(v => v.Name.Contains(searchText) ||
                                                              v.Description.Contains(searchText));
                }
                else
                {
                    long loggedInUserId = long.Parse(HttpContext.Session.GetString("LoggedInUserId"));
                    videos = _videoData.GetAllForUser(loggedInUserId).
                        Where(v => v.Name.Contains(searchText) ||
                                   v.Description.Contains(searchText));
                }
            }
            foreach (var v in videos)
            {
                v.Owner = _userData.GetById(v.OwnerId);
            }
            foreach (var c in comments)
            {
                c.User = _userData.GetById(c.UserId);
                c.Video = _videoData.GetById(c.VideoId);
            }

            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.Users = UserForAdminPage.ConvertDTO(users);
            searchViewModel.Comments = CommentDTO.ConvertCommenstToDTO(comments);
            searchViewModel.Videos = VideoDTO.ConvertVideosToDTO(videos);
            var contentType = Request.ContentType;
            if(contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(searchViewModel);
                }
            }
            return View("SearchPage", searchViewModel);
        }
        
        [HttpGet]
        [Route("search/{searchtext}/{searchVideo}/{searchUser}/{searchComment}/{sortVideoBy}/{sortUserBy}/{sortCommentBy}/{ascDesc}")]
        public IActionResult Search(string searchText, bool searchVideo, bool searchUser, bool searchComment,
            string sortVideoBy, string sortUserBy, string sortCommentBy, string ascDesc)
        {
            var loggedInUserRole = HttpContext.Session.GetString("LoggedInUserRole");
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            List<Video> videos = new List<Video>();
            List<Comment> comments = new List<Comment>();
            List<User> users = new List<User>();

            
            if (loggedInUserRole == null)
            {
                if (searchVideo)
                {
                    videos = _videoData.GetAllNoUser().Where(v => v.Name.Contains(searchText) ||
                                                              v.Description.Contains(searchText)).ToList();
                }
                if (searchUser)
                {
                    users = _userData.Search(searchText, null, "desc").Where(u => u.Blocked == false).ToList();
                }
                if (searchComment)
                {
                    comments = _commentData.Search(searchText, "desc").Where(c => c.Blocked == false).ToList();
                }
            }
            else
            {
                if (loggedInUserRole.Equals("0"))
                {
                    if (searchVideo)
                    {
                        videos = _videoData.GetAllForAdmin().Where(v => v.Name.Contains(searchText) ||
                                                             v.Description.Contains(searchText)).ToList();
                    }
                    if (searchUser)
                    {
                        users = _userData.Search(searchText, null, "desc").ToList();
                    }
                    if (searchComment)
                    {
                        comments = _commentData.Search(searchText, "desc").ToList();
                    } 
                }
                else
                {
                    long id = long.Parse(loggedInUserId);
                    if (searchVideo)
                    {
                        videos = _videoData.GetAllForUser(id).
                        Where(v => v.Name.Contains(searchText) ||
                                   v.Description.Contains(searchText)).ToList();
                    }
                    if (searchUser)
                    {
                        users = _userData.Search(searchText, null, "desc").Where(u => u.Blocked == false || u.Id == id).ToList();
                    }
                    if (searchComment)
                    {
                        comments = _commentData.Search(searchText, "desc").Where(c => c.Blocked == false).ToList();
                    }
                }
            }
            if(videos.Count() > 0)
            {
                foreach (var v in videos)
                {
                    v.Owner = _userData.GetById(v.OwnerId);
                }
            }
            if(comments.Count() > 0)
            {
                foreach (var c in comments)
                {
                    c.User = _userData.GetById(c.UserId);
                    c.Video = _videoData.GetById(c.VideoId);
                }
            }

            switch (sortUserBy)
            {
                case "Username":
                    if (ascDesc.Equals("asc")) users.OrderBy(u => u.Username).ToList();
                    else users.OrderByDescending(u => u.Username).ToList();
                    break;
                case "Firstname":
                    if (ascDesc.Equals("asc")) users.OrderBy(u => u.FirstName).ToList();
                    else users.OrderByDescending(u => u.FirstName).ToList();
                    break;
                case "Lastname":
                    if (ascDesc.Equals("asc")) users.OrderBy(u => u.LastName).ToList();
                    else users.OrderByDescending(u => u.LastName).ToList();
                    break;
                case "Email":
                    if (ascDesc.Equals("asc")) users.OrderBy(u => u.Email).ToList();
                    else users.OrderByDescending(u => u.Email).ToList();
                    break;
                default:
                    if (ascDesc.Equals("asc")) users.OrderBy(u => u.Username).ToList();
                    else users.OrderByDescending(u => u.Username).ToList();
                    break;
            }
            switch (sortVideoBy)
            {
                case "Name":
                    if (ascDesc.Equals("asc")) videos.OrderBy(v => v.Name).ToList();
                    else videos.OrderByDescending(v => v.Name).ToList();
                    break;
                case "Description":
                    if (ascDesc.Equals("asc")) videos.OrderBy(v => v.Description).ToList();
                    else videos.OrderByDescending(v => v.Description).ToList();
                    break;
                case "User":
                    if (ascDesc.Equals("asc")) videos.OrderBy(v => v.Owner).ToList();
                    else videos.OrderByDescending(v => v.Owner).ToList();
                    break;
                default:
                    if (ascDesc.Equals("asc")) videos.OrderBy(v => v.Name).ToList();
                    else videos.OrderByDescending(v => v.Name).ToList();
                    break;
            }
            switch (sortCommentBy)
            {
                case "Description":
                    if (ascDesc.Equals("asc")) comments.OrderBy(v => v.Description).ToList();
                    else comments.OrderByDescending(v => v.Description).ToList();
                    break;
                case "User":
                    if (ascDesc.Equals("asc")) comments.OrderBy(v => v.User).ToList();
                    else comments.OrderByDescending(v => v.User).ToList();
                    break;
                case "Video":
                    if (ascDesc.Equals("asc")) comments.OrderBy(v => v.Video).ToList();
                    else comments.OrderByDescending(v => v.Video).ToList();
                    break;
                default:
                    if (ascDesc.Equals("asc")) comments.OrderBy(v => v.Video).ToList();
                    else comments.OrderByDescending(v => v.Video).ToList();
                    break;
            }
            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.Users = UserForAdminPage.ConvertDTO(users);
            searchViewModel.Comments = CommentDTO.ConvertCommenstToDTO(comments);
            searchViewModel.Videos = VideoDTO.ConvertVideosToDTO(videos);
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(searchViewModel);
                }
            }
            return View("SearchPage", searchViewModel);
        }
    }
}
