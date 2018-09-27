using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class SingleVideoDTO
    {
        public SingleVideoDTO()
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
        public Boolean Deleted { get; set; }
        public UserForVideoComment Owner { get; set; }
        public virtual List<CommentForVideoDTO> Comments { get; set; }

        public static SingleVideoDTO ConvertVideoToDTO(Video video)
        {
            SingleVideoDTO newDTO = new SingleVideoDTO
            {
                Id = video.Id,
                VideoUrl = video.VideoUrl,
                PhotoUrl = video.PhotoUrl,
                Name = video.Name,
                Description = video.Description,
                Visibility = video.Visibility.ToString(),
                Blocked = video.Blocked,
                Deleted = video.Deleted,
                AllowComments = video.AllowComments,
                AllowRaiting = video.AllowRaiting,
                NumberOfLikes = video.NumberOfLikes,
                NumberOfDislikes = video.NumberOfDislikes,
                NumberOfViews = video.NumberOfViews,
                CreationDate = video.CreationDate.ToString("dd.mm.yyyy"),
                Owner = UserForVideoComment.ConvertUserForVideoComment(video.Owner),
                Comments = CommentForVideoDTO.ConvertCommentToDTO(video.Comments)
            };
            return newDTO;
        }
        public static List<SingleVideoDTO> ConvertLikedVideosToDTO(IEnumerable<Video> video)
        {
            List<SingleVideoDTO> singleVideoDTO = new List<SingleVideoDTO>();
            foreach (var v in video)
            {
                SingleVideoDTO newDTO = new SingleVideoDTO
                {
                    Id = v.Id,
                    VideoUrl = v.VideoUrl,
                    PhotoUrl = v.PhotoUrl,
                    Name = v.Name,
                    Description = v.Description,
                    Visibility = v.Visibility.ToString(),
                    Blocked = v.Blocked,
                    Deleted = v.Deleted,
                    AllowComments = v.AllowComments,
                    AllowRaiting = v.AllowRaiting,
                    NumberOfLikes = v.NumberOfLikes,
                    NumberOfDislikes = v.NumberOfDislikes,
                    NumberOfViews = v.NumberOfViews,
                    CreationDate = v.CreationDate.ToString("dd.mm.yyyy"),
                    Owner = UserForVideoComment.ConvertUserForVideoComment(v.Owner),
                    Comments = CommentForVideoDTO.ConvertCommentToDTO(v.Comments)
                };
                singleVideoDTO.Add(newDTO);
            }
            return singleVideoDTO;
        }
    }
}
