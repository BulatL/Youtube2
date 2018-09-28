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
    public class UserController : Controller
    {
        private IUserData _userData;
        private IFollowData _followData;
        private IVideoData _videoData;
        private ICommentData _commentData;
        private IHostingEnvironment _hostingEnvironment;
        public UserController(IUserData userData, IHostingEnvironment hostingEnvironment, 
            IFollowData followData, IVideoData videoData, ICommentData commentData)
        {
            _userData = userData;
            _followData = followData;
            _videoData = videoData;
            _commentData = commentData;
            _hostingEnvironment = hostingEnvironment;
        }

        

        [HttpGet]
        [Route("users")]
        public IActionResult GetAllUsers()
        {
            var contentType = Request.ContentType;
            //IEnumerable<UserDTO> userDTO = new List<UserDTO>();
            IEnumerable<User> users = new List<User>();
            users = _userData.GetAll();

            foreach (var u in users)
            {
                u.UserVideos = _videoData.GetByOwnerId(u.Id);
            }
            List<UserForAdminPage> userDTO = UserForAdminPage.ConvertDTO(users);
            if (contentType!= null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(userDTO);
                }else if (contentType.Equals("text/html"))
                {
                    UsersViewModel newDTO = new UsersViewModel();
                    newDTO.Users = users;
                    return View("UserInfo", newDTO);
                }
                return StatusCode(415);
            }
            return Json(userDTO);
        }

        [HttpGet]
        [Route("users/{id}")]
        public IActionResult GetById(long id)
        {
            var loggedInUserRole = HttpContext.Session.GetString("LoggedInUserRole");

            var contentType = Request.ContentType;
            User user = _userData.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            if(contentType == null)
            {
                if(loggedInUserRole != null)
                {
                    if (loggedInUserRole.Equals("1"))
                    {
                        long loggedInUserId = long.Parse(HttpContext.Session.GetString("LoggedInUserId"));
                        if (user.Blocked == true && user.Id != loggedInUserId)
                        {
                            return StatusCode(401);
                        }
                    }
                }
                else
                {
                    if (user.Blocked == true)
                    {
                        return StatusCode(401);
                    }
                }
            }

            user.Followers = _followData.GetFollowers(user.Id);
            user.Following = _followData.GetFollowings(user.Id);
            user.UserVideos = _videoData.GetByOwnerId(user.Id);
            user.LikedVideos = _videoData.GetLikedVideos(user);
            user.UserComments = _commentData.GetCommentsByUser(user.Id);
            foreach (var u in user.LikedVideos)
            {
                u.Owner = _userData.GetById(u.OwnerId);
            }

            SingleUserDTO singleUserDTO = SingleUserDTO.ConvertUserToDTO(user);
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(singleUserDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    SingleUserDTO newDTO = SingleUserDTO.ConvertUserToDTO(user);
                    return View("SingleUserInfo", newDTO);
                }
                return StatusCode(415);
            }
            return View("UserPage", singleUserDTO);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("LoginPage");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username,string password )
        {
            User user = _userData.Login(username, password);
            if(user == null)
            {
                return BadRequest();
            }
            HttpContext.Session.SetString("LoggedInUserUsername", user.Username); 
            HttpContext.Session.SetString("LoggedInUserId", user.Id.ToString());
            HttpContext.Session.SetString("LoggedInUserRole", user.Role.ToString());
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(user);
                }
                else if (contentType.Equals("text/html"))
                {
                    return RedirectToAction(nameof(HomeController.Home));
                }
                else if (contentType.Equals("application/x-www-form-urlencoded; charset=UTF-8"))
                {
                    return Json("success");
                }
                return StatusCode(415);
            }
            return RedirectToAction(nameof(HomeController.Home));
        }


        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedInUserUsername");
            HttpContext.Session.Remove("LoggedInUserId");
            HttpContext.Session.Remove("LoggedInUserRole");

            return RedirectToAction(nameof(HomeController.Home));
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(AddUserDTO user, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (_userData.ExistUsername(user.Username, 0))
                {
                    ModelState.AddModelError("Username", "Username taken");
                    return View(user);
                    //return BadRequest();
                }
                var newUser = new User();
                newUser.Username = user.Username;
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Email = user.Email;
                newUser.Description = user.Description;
                newUser.Password = user.Password;
                newUser.Role = RoleType.User;
                newUser.Blocked = false;
                newUser.Deleted = false;
                newUser.RegistrationDate = DateTime.Today;

                if (user.Image != null)
                {
                    newUser.ProfilePictureUrl = AddImage(newUser.Username, user.Image);
                }
                else if(image != null)
                {
                    newUser.ProfilePictureUrl = AddImage(newUser.Username, image);
                }
                else
                {
                    newUser.ProfilePictureUrl = "DefaultUserImage.png";
                }

                newUser = _userData.Add(newUser);

                HttpContext.Session.SetString("LoggedInUserUsername", newUser.Username);
                HttpContext.Session.SetString("LoggedInUserId", newUser.Id.ToString());
                HttpContext.Session.SetString("LoggedInUserRole", newUser.Role.ToString());

                var contentType = Request.ContentType;
                if (contentType != null)
                {
                    if (contentType.Equals("application/json"))
                    {
                        return Json(newUser);
                    }
                    else if (contentType.Equals("text/html"))
                    {
                        return RedirectToAction(nameof(HomeController.Home));
                    }
                    else if (contentType.Equals("application/x-www-form-urlencoded"))
                    {
                        return RedirectToAction(nameof(HomeController.Home));
                    }
                    return RedirectToAction(nameof(GetById), user.Id);
                }
                return RedirectToAction(nameof(HomeController.Home));
            }
            else
            {
                return Json(new UserDTO());
            }
        }

        [HttpDelete]
        [Route("users/delete/{id}")]
        public IActionResult Delete(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            User user = _userData.GetById(id);
            if(user == null)
            {
                return NotFound();
            }
            IEnumerable<Video> userVideos = _videoData.GetByOwnerId(user.Id);
            IEnumerable<Comment> userComments = _commentData.GetCommentsByUser(user.Id);
            foreach (var v in userVideos)
            {
                _videoData.Delete(v);
            }
            foreach (var c in userComments)
            {
                _commentData.Delete(c);
            }
            _userData.Delete(user);
            int loggedInUserId = int.Parse(HttpContext.Session.GetString("LoggedInUserId"));
            if (loggedInUserId == id)
            {
                HttpContext.Session.Remove("LoggedInUserId");
                HttpContext.Session.Remove("LoggedInUserRole");
                HttpContext.Session.Remove("LoggedInUserUsername");
            }
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return NoContent();
                }
                else if (contentType.Equals("text/html"))
                {
                    return RedirectToAction(nameof(GetById), id);
                }
                else if (contentType.Equals("application/x-www-form-urlencoded"))
                {
                    return RedirectToAction(nameof(GetById), id);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }

        [HttpGet]
        [Route("users/sort/{sortBy}/{ascDesc}")]
        public IActionResult Sort(string sortBy, string ascDesc)
        {
            if(sortBy == null)
            {
                return BadRequest();
            }
            if(ascDesc == null)
            {
                return BadRequest();
            }
            var model = new UsersViewModel();
            model.Users = _userData.Sort(sortBy, ascDesc);
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(model.Users);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return Json(model.Users);
                }
                return StatusCode(415);
            }
            return Json(model.Users);
        }

        [HttpGet]
        [Route("users/search")]
        public IActionResult SearchAdmin(string searchText, string sortBy, string ascDesc)
        {

            IEnumerable<User> users = new List<User>();
            users = _userData.Search(searchText,null,ascDesc);

            List<UserForAdminPage> userDTO = UserForAdminPage.ConvertDTO(users);

            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(userDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    UsersViewModel newDTO = new UsersViewModel();
                    newDTO.Users = users;
                    return View("UserInfo", newDTO);
                }
                return StatusCode(415);
            }
            return Json(userDTO);
        }

        [HttpGet]
        [Route("users/search/{searchText}/{searchBy?}")]
        public IActionResult Search(string searchText, string searchBy)
        {
            if (searchText == null)
            {
                return BadRequest();
            }
            var model = new UsersViewModel();
            model.Users = _userData.Search(searchText, searchBy,"desc");
            if(model.Users.Count() == 0)
            {
                return NoContent();
            }
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    List<UserForAdminPage> userDTO = UserForAdminPage.ConvertDTO(model.Users);
                    return Json(userDTO);
                }
                else if (contentType.Equals("text/html"))
                {
                    UsersViewModel newDTO = new UsersViewModel();
                    newDTO.Users = model.Users;
                    return View("UserInfo", newDTO);
                }
                return StatusCode(415);
            }
            return Json(model.Users);
        }

        [HttpGet]
        [Route("users/edit/{id}")]
        public IActionResult Edit(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User user = _userData.GetById(id);
            if(user == null)
            {
                return StatusCode(401);
            }
            if(user.Role == RoleType.User || user.Id != id)
            {
                return StatusCode(401);
            }
            EditUserDTO userDTO = EditUserDTO.ConvertDTO(user);
            return View("EditUserPage",userDTO);
        }

        [HttpPost]
        [Route("users/edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(AddUserDTO user, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (_userData.ExistUsername(user.Username, user.Id))
                {
                    ViewBag.Message = "Username already exist";
                    return Json("Username aleady exist");
                }

                var editUser = _userData.GetById(user.Id);
                if(editUser == null)
                {
                    return NotFound();
                }
                editUser.FirstName = user.FirstName;
                editUser.LastName = user.LastName;
                if(user.Password != null) editUser.Password = user.Password;
                editUser.Email = user.Email;
                editUser.Description = user.Description;
                if(user.Image != null)
                {
                    editUser.ProfilePictureUrl = AddImage(editUser.Username, user.Image);

                }
                editUser = _userData.Update(editUser);

                var contentType = Request.ContentType;
                if (contentType != null)
                {
                    if (contentType.Equals("application/json"))
                    {
                        UserForVideoComment userDto = UserForVideoComment.ConvertUserForVideoComment(editUser);
                        return Json(userDto);
                    }
                    else if (contentType.Equals("text/html"))
                    {
                        SingleUserDTO newDTO = SingleUserDTO.ConvertUserToDTO(editUser);
                        return View("SingleUserInfo", newDTO);
                    }
                    else if (contentType.Equals("application/x-www-form-urlencoded"))
                    {
                        return RedirectToAction(nameof(GetById), user.Id);
                    }
                    return RedirectToAction(nameof(GetById), user.Id); ;
                }
                return Json("Success");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("users/block/{id}")]
        public IActionResult Block(int id)
        {
            var contentType = Request.ContentType;
            User user = _userData.GetById(id);
            if(user == null)
            {
                return NotFound();
            }
            if(user.Blocked == true)
            {
                return BadRequest();
            }
            user.Blocked = true;
            user = _userData.Update(user);
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    UserForVideoComment userDto = UserForVideoComment.ConvertUserForVideoComment(user);
                    return Json(userDto);
                }
                else if (contentType.Equals("text/html"))
                {
                    SingleUserDTO newDTO = SingleUserDTO.ConvertUserToDTO(user);
                    return View("SingleUserInfo", newDTO);
                }
                else if (contentType.Equals("application/x-www-form-urlencoded"))
                {
                    return RedirectToAction(nameof(GetById), id);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }

        [HttpPatch]
        [Route("users/unblock/{id}")]
        public IActionResult Unblock(int id)
        {
            var contentType = Request.ContentType;
            User user = _userData.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Blocked == false)
            {
                return BadRequest();
            }
            user.Blocked = false;
            user = _userData.Update(user);
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    UserForVideoComment userDto = UserForVideoComment.ConvertUserForVideoComment(user);
                    return Json(userDto);
                }
                else if (contentType.Equals("text/html"))
                {
                    SingleUserDTO newDTO = SingleUserDTO.ConvertUserToDTO(user);
                    return View("SingleUserInfo", newDTO);
                }
                else if (contentType.Equals("application/x-www-form-urlencoded"))
                {
                    return RedirectToAction(nameof(GetById), id);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }

        [HttpPatch]
        [Route("users/promote/{id}")]
        public IActionResult Promote(int id)
        {
            var contentType = Request.ContentType;
            User user = _userData.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Role == RoleType.Admin)
            {
                return BadRequest();
            }
            user.Role = RoleType.Admin;
            user = _userData.Update(user);
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    UserForVideoComment userDto = UserForVideoComment.ConvertUserForVideoComment(user);
                    return Json(userDto);
                }
                else if (contentType.Equals("text/html"))
                {
                    SingleUserDTO newDTO = SingleUserDTO.ConvertUserToDTO(user);
                    return View("SingleUserInfo", newDTO);
                }
                else if (contentType.Equals("application/x-www-form-urlencoded"))
                {
                    return RedirectToAction(nameof(GetById), id);
                }
                return StatusCode(415);
            }
            return Json("Success");

        }

        [HttpPost]
        [Route("users/follow/{id}")]
        public IActionResult Follow(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User userForFollow = _userData.GetById(id);
            User loggedInUser = _userData.GetById(long.Parse(loggedInUserId));
            if (loggedInUser == null || userForFollow == null)
            {
                return BadRequest();
            }
            if(_followData.Exist(loggedInUser.Id, userForFollow.Id)){
                return BadRequest();
            }
            Follow follow = new Follow();
            follow.Follower = loggedInUser;
            follow.Following = userForFollow;
            follow = _followData.Add(follow);
            var contentType = Request.ContentType;
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    return Json(follow);
                }
                else if (contentType.Equals("text/html"))
                {
                    //return RedirectToAction(nameof(GetById), id);
                }
                else if (contentType.Equals("application/x-www-form-urlencoded"))
                {
                    //return RedirectToAction(nameof(GetById), id);
                }
                return StatusCode(415);
            }
            return Json("Success");
        }

        [HttpDelete]
        [Route("users/unfollow/{id}")]
        public IActionResult Unfollow(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User userForFollow = _userData.GetById(id);
            User loggedInUser = _userData.GetById(long.Parse(loggedInUserId));
            if (loggedInUser == null || userForFollow == null)
            {
                return BadRequest();
            }
            if (_followData.Exist(loggedInUser.Id, userForFollow.Id)){
                Follow follow = new Follow();
                follow.Follower = loggedInUser;
                follow.Following = userForFollow;

                _followData.Delete(follow);
            }

            var contentType = Request.ContentType;
            if (contentType != null)
            {
                return NoContent();
            }
            return Json("Success");
        }

        [HttpGet]
        [Route("users/existfollow/{id}")]
        public IActionResult ExistFollow(long id)
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            if (loggedInUserId == null)
            {
                return StatusCode(401);
            }
            User userForFollow = _userData.GetById(id);
            User loggedInUser = _userData.GetById(long.Parse(loggedInUserId));
            if (loggedInUser == null || userForFollow == null)
            {
                return BadRequest();
            }
            if (_followData.Exist(loggedInUser.Id, userForFollow.Id))
            {
                return Json("Follow");
            }

            return Json("No follow");
        }

        [HttpGet]
        [Route("loggedInUser")]
        public IActionResult GetLoggedInUser()
        {
            var loggedInUserId = HttpContext.Session.GetString("LoggedInUserId");

            var contentType = Request.ContentType;
            if (loggedInUserId == null)
            {
                if(contentType!= null)
                    if (contentType.Equals("application/json")) return NoContent();
                return Json(null);
            }
            User user = _userData.GetById(long.Parse(loggedInUserId));

            if(user == null)
            {
                if(contentType!= null)
                    if (contentType.Equals("application/json")) return NoContent();
                return Json(null);
            }
            if (contentType != null)
            {
                if (contentType.Equals("application/json"))
                {
                    UserForVideoComment userDto = UserForVideoComment.ConvertUserForVideoComment(user);
                    return Json(userDto);
                }
                else if (contentType.Equals("text/html"))
                {
                    SingleUserDTO newDTO = SingleUserDTO.ConvertUserToDTO(user);
                    return View("SingleUserInfo", newDTO);
                }
                return StatusCode(415);
            }
            return Json(user);
        }

        [HttpGet]
        [Route("admin")]
        public IActionResult GetAdminPage()
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
            if(user.Role!= RoleType.Admin)
            {
                return StatusCode(401);
            }
            /*IEnumerable<User> users = new List<User>();
            users = _userData.GetAll();
            List<UserForAdminPage> userDTO = UserForAdminPage.ConvertDTO(users);*/
            return View("AdminPage");
        }

        public string AddImage(string username, IFormFile image)
        {
            var fileName = DateTime.Now.ToString("yyyyMMdd") + username + Path.GetExtension(image.FileName);
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, fileName);
            image.CopyTo(new FileStream(filePath, FileMode.Create));
            return fileName;
        }

    }
}
