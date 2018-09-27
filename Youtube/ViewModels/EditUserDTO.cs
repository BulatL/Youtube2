using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class EditUserDTO
    {
        public long Id { get; set; }
        [Required, MaxLength(20)]
        public String Username { get; set; }
        [MaxLength(30)]
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
        [Display(Name = "Upload profile picture")]
        public IFormFile Image { get; set; }
        public String PhotoUrl { get; set; }

        public static EditUserDTO ConvertDTO(User user)
        {
            EditUserDTO userDTO = new EditUserDTO();
            userDTO.Id = user.Id;
            userDTO.Username = user.Username;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.Description = user.Description;
            userDTO.Email = user.Email;
            userDTO.PhotoUrl = user.ProfilePictureUrl;
            return userDTO;
        }
    }
}
