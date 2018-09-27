using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class AddVideoDTO
    {
        [Required]
        public String VideoUrl { get; set; }
        [Required, MaxLength(80)]
        public String Name { get; set; }
        [Required, MaxLength(250)]
        public String Description { get; set; }
        [Required]
        public Visibility Visibility { get; set; }
        [Required]
        public Boolean AllowComments { get; set; }
        [Required]
        public Boolean AllowRaiting { get; set; }
        [Display(Name = "Upload video picture")]
        public IFormFile Image { get; set; }
        public String PhotoUrl { get; set; }

        public static AddVideoDTO ConvertDTO (Video video)
        {
            AddVideoDTO newDTO = new AddVideoDTO();
            newDTO.VideoUrl = video.VideoUrl;
            newDTO.Name = video.Name;
            newDTO.Description = video.Description;
            newDTO.Visibility = video.Visibility;
            newDTO.AllowComments = video.AllowComments;
            newDTO.AllowRaiting = video.AllowRaiting;
            newDTO.PhotoUrl = video.PhotoUrl;

            return newDTO;
        }
    }
}
