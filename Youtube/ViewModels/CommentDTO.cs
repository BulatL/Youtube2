using System;
using System.Collections;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public String Description { get; set; }
        public String CreationDate { get; set; }
        public long NumberOfLikes { get; set; }
        public long NumberOfDislikes { get; set; }
        public UserForVideoComment User { get; set; }
        public VideoForUser Video { get; set; }

        public static CommentDTO ConvertCommentToDTO(Comment comment)
        {

            CommentDTO newDto = new CommentDTO
            {
                Id = comment.Id,
                Description = comment.Description,
                CreationDate = comment.CreationDate.ToString("dd.mm.yyyy"),
                NumberOfLikes = comment.NumberOfLikes,
                NumberOfDislikes =comment.NumberOfDislikes,
                User = UserForVideoComment.ConvertUserForVideoComment(comment.User),
                Video = VideoForUser.ConvertVideo(comment.Video)
            };
            return newDto;
        }

        public static List<CommentDTO> ConvertCommenstToDTO(IEnumerable<Comment> comments)
        {
            List<CommentDTO> commentsDTO = new List<CommentDTO>();
            foreach (var comment in comments)
            {
                CommentDTO newDto = new CommentDTO
                {
                    Id = comment.Id,
                    Description = comment.Description,
                    CreationDate = comment.CreationDate.ToString("dd.mm.yyyy"),
                    NumberOfLikes = comment.NumberOfLikes,
                    NumberOfDislikes = comment.NumberOfDislikes,
                    User = UserForVideoComment.ConvertUserForVideoComment(comment.User),
                    Video = VideoForUser.ConvertVideo(comment.Video)
                };

                commentsDTO.Add(newDto);
            }
            
            return commentsDTO;
        }
    }
}
