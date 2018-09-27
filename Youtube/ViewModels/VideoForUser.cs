using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class VideoForUser
    {
        public VideoForUser()
        {

        }
        public long Id { get; set; }
        public String VideoUrl { get; set; }
        public String PhotoUrl { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Visibility { get; set; }
        public Boolean AllowComments { get; set; }
        public Boolean AllowRaiting { get; set; }
        public long NumberOfLikes { get; set; }
        public long NumberOfDislikes { get; set; }
        public int NumberOfViews { get; set; }
        public String CreationDate { get; set; }
        public Boolean Blocked { get; set; }

        public static List<VideoForUser> ConvertVideoToDTO(IEnumerable<Video> video)
        {
            List<VideoForUser> videos = new List<VideoForUser>();
            foreach (var v in video)
            {
                VideoForUser newVDTO = new VideoForUser
                {
                    Id = v.Id,
                    VideoUrl = v.VideoUrl,
                    PhotoUrl = v.PhotoUrl,
                    Name = v.Name,
                    Description = v.Description,
                    Visibility = v.Visibility.ToString(),
                    Blocked = v.Blocked,
                    AllowComments = v.AllowComments,
                    AllowRaiting = v.AllowRaiting,
                    NumberOfLikes = v.NumberOfLikes,
                    NumberOfDislikes = v.NumberOfDislikes,
                    NumberOfViews = v.NumberOfViews,
                    CreationDate = v.CreationDate.ToString("dd.mm.yyyy")
                };
                videos.Add(newVDTO);
            }
            return videos;
        }

        public static VideoForUser ConvertVideo(Video v)
        {
                VideoForUser newVDTO = new VideoForUser
                {
                    Id = v.Id,
                    VideoUrl = v.VideoUrl,
                    PhotoUrl = v.PhotoUrl,
                    Name = v.Name,
                    Description = v.Description,
                    Visibility = v.Visibility.ToString(),
                    Blocked = v.Blocked,
                    AllowComments = v.AllowComments,
                    AllowRaiting = v.AllowRaiting,
                    NumberOfLikes = v.NumberOfLikes,
                    NumberOfDislikes = v.NumberOfDislikes,
                    NumberOfViews = v.NumberOfViews,
                    CreationDate = v.CreationDate.ToString("dd.mm.yyyy")
                };
            return newVDTO;
        }
    }
}
