using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class CommentForUserDTO
    {
        public CommentForUserDTO()
        {

        }
        public long Id { get; set; }
        public String Description { get; set; }
        public long NumberOfLikes { get; set; }
        public long NumberOfDislikes { get; set; }
        public String CreationDate { get; set; }

        public static List<CommentForUserDTO> ConvertCommentToDTO(IEnumerable<Comment> comments)
        {
            List<CommentForUserDTO> commentsDTO = new List<CommentForUserDTO>();
            foreach (var comment in comments)
            {
                CommentForUserDTO newDto = new CommentForUserDTO
                {
                    Id = comment.Id,
                    Description = comment.Description,
                    NumberOfLikes = comment.NumberOfLikes,
                    NumberOfDislikes = comment.NumberOfDislikes,
                    CreationDate = comment.CreationDate.ToString("dd.mm.yyyy"),
                };
                commentsDTO.Add(newDto);
            }

            return commentsDTO;
        }

        public static CommentForUserDTO SingleConvertCommentToDTO(Comment comment)
        {
            CommentForUserDTO newDto = new CommentForUserDTO
            {
                Id = comment.Id,
                Description = comment.Description,
                NumberOfLikes = comment.NumberOfLikes,
                NumberOfDislikes = comment.NumberOfDislikes,
                CreationDate = comment.CreationDate.ToString("dd.mm.yyyy"),
            };
            return newDto;
        }
    }
}
