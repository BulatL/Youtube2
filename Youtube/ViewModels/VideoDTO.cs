using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class VideoDTO
    {
        public VideoDTO()
        {

        }
        public long Id { get; set; }
        [Required]
        public String VideoUrl { get; set; }
        public String PhotoUrl { get; set; }
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
        public long NumberOfLikes { get; set; }
        public long NumberOfDislikes { get; set; }
        public int NumberOfViews { get; set; }
        public String CreationDate { get; set; }
        public Boolean Blocked { get; set; }
        public UserForVideoComment User { get; set; }

        public static VideoDTO ConvertVideoToDTO(Video video)
        {
            VideoDTO newVDTO = new VideoDTO
            {
                Id = video.Id,
                VideoUrl = video.VideoUrl,
                PhotoUrl = video.PhotoUrl,
                Name = video.Name,
                Description = video.Description,
                Visibility = video.Visibility,
                Blocked = video.Blocked,
                AllowComments = video.AllowComments,
                AllowRaiting = video.AllowRaiting,
                NumberOfLikes = video.NumberOfLikes,
                NumberOfDislikes = video.NumberOfDislikes,
                NumberOfViews = video.NumberOfViews,
                CreationDate = video.CreationDate.ToString("dd.mm.yyyy"),
                User = UserForVideoComment.ConvertUserForVideoComment(video.Owner)
            };
            return newVDTO;
        }

        public static List<VideoDTO> ConvertVideosToDTO(IEnumerable<Video> videos)
        {
            List<VideoDTO> videosDto = new List<VideoDTO>();
            foreach (var video in videos)
            {
                VideoDTO newVDTO = new VideoDTO
                {
                    Id = video.Id,
                    VideoUrl = video.VideoUrl,
                    PhotoUrl = video.PhotoUrl,
                    Name = video.Name,
                    Description = video.Description,
                    Visibility = video.Visibility,
                    Blocked = video.Blocked,
                    AllowComments = video.AllowComments,
                    AllowRaiting = video.AllowRaiting,
                    NumberOfLikes = video.NumberOfLikes,
                    NumberOfDislikes = video.NumberOfDislikes,
                    NumberOfViews = video.NumberOfViews,
                    CreationDate = video.CreationDate.ToString("dd.mm.yyyy"),
                    User = UserForVideoComment.ConvertUserForVideoComment(video.Owner)
                };
                videosDto.Add(newVDTO);
            }
            return videosDto;
        }
    }
}
