using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class SingleUserDTO
    {
        public SingleUserDTO()
        {

        }
        public long Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Description { get; set; }
        public String RegistrationDate { get; set; }
        public String Role { get; set; }
        public Boolean Blocked { get; set; }
        public String profilePictureUrl { get; set; }
        public List<VideoForUser> UsersVideos { get; set; }
        public List<SingleVideoDTO> LikedVideos { get; set; }
        public List<CommentForUserDTO> UserComments { get; set; }
        public List<CommentForUserDTO> LikedComments { get; set; }
        public List<UserForVideoComment> Followers { get; set; }
        public List<UserForVideoComment> Following { get; set; }

        public static SingleUserDTO ConvertUserToDTO(User user)
        {
            RoleType role = (RoleType)user.Role;
            SingleUserDTO newVDTO = new SingleUserDTO
            {
                Id = user.Id,
                profilePictureUrl = user.ProfilePictureUrl,
                Username = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Description = user.Description,
                RegistrationDate = user.RegistrationDate.ToString("dd.mm.yyyy"),
                Role = role.ToString(),
                Blocked = user.Blocked,
                UsersVideos = VideoForUser.ConvertVideoToDTO(user.UserVideos),
                LikedVideos = SingleVideoDTO.ConvertLikedVideosToDTO(user.LikedVideos),
                UserComments = CommentForUserDTO.ConvertCommentToDTO(user.UserComments),
                LikedComments = CommentForUserDTO.ConvertCommentToDTO(user.LikedComments),
                Followers = UserForVideoComment.ConvertFollowers(user.Followers),
                Following = UserForVideoComment.ConvertFollowers(user.Following)
            };
            return newVDTO;
        }
    }
}
