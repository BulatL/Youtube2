using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class CommentForVideoDTO
    {
        public long Id { get; set; }
        public String Description { get; set; }
        public String CreationDate { get; set; }
        public long NumberOfLikes { get; set; }
        public long NumberOfDislikes { get; set; }
        public UserForVideoComment User { get; set; }

        public static List<CommentForVideoDTO> ConvertCommentToDTO(IEnumerable<Comment> comments)
        {
            List<CommentForVideoDTO> commentsDTO = new List<CommentForVideoDTO>();
            foreach (var comment in comments)
            {
                CommentForVideoDTO newDto = new CommentForVideoDTO
                {
                    Id = comment.Id,
                    Description = comment.Description,
                    CreationDate = comment.CreationDate.ToString("dd.mm.yyyy"),
                    NumberOfLikes = comment.NumberOfLikes,
                    NumberOfDislikes = comment.NumberOfDislikes,
                    User = UserForVideoComment.ConvertUserForVideoComment(comment.User)
                };
                commentsDTO.Add(newDto);
            }
           
            return commentsDTO;
        }
    }
}
