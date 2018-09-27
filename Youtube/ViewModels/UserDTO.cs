using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class UserDTO
    {
        public long Id { get; set; }
        [Required, MaxLength(20)]
        public String Username { get; set; }
        [Required, MaxLength(30)]
        public String Password { get; set; }
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
        public RoleType Role { get; set; }
        public Boolean Blocked { get; set; }
        public Boolean Deleted { get; set; }
        [Display(Name = "Upload profile picture")]
        public IFormFile Image { get; set; }
        public String profilePictureUrl { get; set; }

        public static UserDTO Convert(User user)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.Id = user.Id;
            userDTO.Username = user.Username;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.Description = user.Description;
            userDTO.Email = user.Email;
            return userDTO;
        }
    }
}
