using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class UserForVideoComment
    {
        public UserForVideoComment()
        {

        }
        public long Id { get; set; }
        [Required, MaxLength(20)]
        public String Username { get; set; }
        [Required, MaxLength(25)]
        [Display(Name = "First name")]
        public String FirstName { get; set; }
        [Required, MaxLength(40)]
        [Display(Name = "Last name")]
        public String LastName { get; set; }
        [Required, EmailAddress]
        public String Email { get; set; }
        public String Description { get; set; }
        public String RegistrationDate { get; set; }
        public Boolean Blocked { get; set; }
        public String profilePictureUrl { get; set; }

        public static UserForVideoComment ConvertUserForVideoComment(User user)
        {
            UserForVideoComment userForVideo = new UserForVideoComment();
            userForVideo.Id = user.Id;
            userForVideo.Username = user.Username;
            userForVideo.FirstName = user.FirstName;
            userForVideo.LastName = user.LastName;
            userForVideo.Email = user.Email;
            userForVideo.Description = user.Description;
            userForVideo.RegistrationDate = user.RegistrationDate.ToString("dd.mm.yyyy");
            userForVideo.Blocked = user.Blocked;
            userForVideo.profilePictureUrl = user.ProfilePictureUrl;
            return userForVideo;
        }

        public static List<UserForVideoComment> ConvertFollowers(IEnumerable<User> followers)
        {
            List<UserForVideoComment> userDTO = new List<UserForVideoComment>();
            foreach (var user in followers)
            {
                UserForVideoComment userForVideo = new UserForVideoComment();
                userForVideo.Id = user.Id;
                userForVideo.Username = user.Username;
                userForVideo.FirstName = user.FirstName;
                userForVideo.LastName = user.LastName;
                userForVideo.Email = user.Email;
                userForVideo.Description = user.Description;
                userForVideo.RegistrationDate = user.RegistrationDate.ToString("dd.mm.yyyy");
                userForVideo.Blocked = user.Blocked;
                userForVideo.profilePictureUrl = user.ProfilePictureUrl;

                userDTO.Add(userForVideo);
            }
            return userDTO;
        }
    }
}
